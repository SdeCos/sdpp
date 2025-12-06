using FileManagementApi.Models;

namespace FileManagementApi.Services
{
    public interface IFileService
    {
        Task<FileMetadata> UploadFileAsync(IFormFile file, int? parentId, int userId);
        Task<(Stream?, FileMetadata?)> DownloadFileAsync(int id, int userId);
        Task<IEnumerable<FileMetadata>> ListFilesAsync(int? parentId, int userId);
        Task<bool> DeleteFileAsync(int id, int userId);
        Task<FileMetadata> CreateFolderAsync(string name, int? parentId, int userId);
        Task<(Stream, string)> DownloadFolderAsZipAsync(int folderId, int userId);
    }
}
