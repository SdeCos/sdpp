using FileManagementApi.Data;
using FileManagementApi.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.IO.Compression;

namespace FileManagementApi.Services
{


    public class FileService : IFileService
    {
        private readonly AppDbContext _context;
        private readonly string _storagePath;

        public FileService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        public async Task<FileMetadata> UploadFileAsync(IFormFile file, int? parentId, int userId)
        {
            var storedFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(_storagePath, storedFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var metadata = new FileMetadata
            {
                FileName = file.FileName,
                StoredFileName = storedFileName,
                ContentType = file.ContentType,
                Size = file.Length,
                UploadDate = DateTime.UtcNow,
                ParentId = parentId,
                IsFolder = false,
                UserId = userId
            };

            _context.Files.Add(metadata);
            await _context.SaveChangesAsync();

            return metadata;
        }

        public async Task<(Stream?, FileMetadata?)> DownloadFileAsync(int id, int userId)
        {
            var metadata = await _context.Files.FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId);
            if (metadata == null)
            {
                return (null, null);
            }

            var filePath = Path.Combine(_storagePath, metadata.StoredFileName);
            if (!File.Exists(filePath))
            {
                return (null, metadata);
            }

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return (stream, metadata);
        }

        public async Task<IEnumerable<FileMetadata>> ListFilesAsync(int? parentId, int userId, bool starredOnly = false)
        {
            var query = _context.Files.Where(f => f.UserId == userId);

            if (starredOnly)
            {
                query = query.Where(f => f.IsStarred);
            }
            else
            {
                query = query.Where(f => f.ParentId == parentId);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> DeleteFileAsync(int id, int userId)
        {
            var metadata = await _context.Files.FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId);
            if (metadata == null)
            {
                return false;
            }

            var filePath = Path.Combine(_storagePath, metadata.StoredFileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            _context.Files.Remove(metadata);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<FileMetadata> CreateFolderAsync(string name, int? parentId, int userId)
        {
            var metadata = new FileMetadata
            {
                FileName = name,
                StoredFileName = string.Empty, // Folders don't have stored files
                ContentType = "folder",
                Size = 0,
                UploadDate = DateTime.UtcNow,
                IsFolder = true,
                ParentId = parentId,
                UserId = userId
            };

            _context.Files.Add(metadata);
            await _context.SaveChangesAsync();

            return metadata;
        }

        public async Task<(Stream, string)> DownloadFolderAsZipAsync(int folderId, int userId)
        {
            var folder = await _context.Files.FirstOrDefaultAsync(f => f.Id == folderId && f.UserId == userId);
            if (folder == null || !folder.IsFolder)
            {
                throw new ArgumentException("Folder not found or invalid.");
            }

            var memoryStream = new MemoryStream();
            using (var archive = new System.IO.Compression.ZipArchive(memoryStream, System.IO.Compression.ZipArchiveMode.Create, true))
            {
                await AddFolderToZipAsync(archive, folderId, "", userId);
            }

            memoryStream.Position = 0;
            return (memoryStream, $"{folder.FileName}.zip");
        }

        private async Task AddFolderToZipAsync(System.IO.Compression.ZipArchive archive, int folderId, string relativePath, int userId)
        {
            var files = await _context.Files.Where(f => f.ParentId == folderId && f.UserId == userId).ToListAsync();

            foreach (var file in files)
            {
                if (file.IsFolder)
                {
                    var newRelativePath = Path.Combine(relativePath, file.FileName);
                    // Create an empty entry for the folder
                    archive.CreateEntry(newRelativePath + "/");
                    await AddFolderToZipAsync(archive, file.Id, newRelativePath, userId);
                }
                else
                {
                    var filePath = Path.Combine(_storagePath, file.StoredFileName);
                    if (File.Exists(filePath))
                    {
                        var entryName = Path.Combine(relativePath, file.FileName);
                        archive.CreateEntryFromFile(filePath, entryName);
                    }
                }
            }
        }

        public async Task<bool> ToggleFileStarAsync(int id)
        {
            var metadata = await _context.Files.FindAsync(id);
            if (metadata == null)
            {
                return false;
            }

            metadata.IsStarred = !metadata.IsStarred;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ShareFileAsync(int fileId, string targetUsername, int currentUserId)
        {
            var targetUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == targetUsername);
            if (targetUser == null)
            {
                return false; // User not found
            }

            // Verify file ownership
            var file = await _context.Files.FirstOrDefaultAsync(f => f.Id == fileId && f.UserId == currentUserId);
            if (file == null)
            {
                return false; // File not found or not owned by user
            }

            // Check if already shared
            if (await _context.FileShares.AnyAsync(fs => fs.FileId == fileId && fs.SharedWithUserId == targetUser.Id))
            {
                return true; // Already shared
            }

            var share = new FileManagementApi.Models.FileShare
            {
                FileId = fileId,
                SharedByUserId = currentUserId,
                SharedWithUserId = targetUser.Id
            };

            _context.FileShares.Add(share);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<FileMetadata>> ListSharedFilesAsync(int userId)
        {
            var sharedWithMe = await _context.FileShares
                .Where(fs => fs.SharedWithUserId == userId)
                .Select(fs => fs.FileId)
                .ToListAsync();

            return await _context.Files
                .Where(f => sharedWithMe.Contains(f.Id))
                .ToListAsync();
        }
    }
}
