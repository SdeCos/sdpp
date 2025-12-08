using FileManagementApi.Models;

namespace FileManagementApi.Services
{
    public interface IFileService
    {
        Task<FileMetadata> UploadFileAsync(IFormFile file, int? parentId, int userId);
        Task<(Stream?, FileMetadata?)> DownloadFileAsync(int id, int userId);
        Task<IEnumerable<FileMetadata>> ListFilesAsync(int? parentId, int userId, bool starredOnly = false);
        Task<bool> DeleteFileAsync(int id, int userId);
        Task<FileMetadata> CreateFolderAsync(string name, int? parentId, int userId);
        Task<(Stream, string)> DownloadFolderAsZipAsync(int folderId, int userId);
        Task<bool> ToggleFileStarAsync(int id);
        Task<bool> ShareFileAsync(int fileId, string targetUsername, int currentUserId);
        Task<IEnumerable<FileMetadata>> ListSharedFilesAsync(int userId);
    }
}
