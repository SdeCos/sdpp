using FileManagementApi.Models;

namespace FileManagementApi.Services
{
    public interface IFileService
    {
        /// Interfaz para el servicio de gestión de archivos.
        /// Define las operaciones principales de negocio.

        /// Sube un archivo al sistema.
        Task<FileMetadata> UploadFileAsync(IFormFile file, int? parentId, int userId);

        /// Prepara un archivo para descarga.
        Task<(Stream?, FileMetadata?)> DownloadFileAsync(int id, int userId);

        /// Lista los archivos de un directorio.
        Task<IEnumerable<FileMetadata>> ListFilesAsync(int? parentId, int userId, bool starredOnly = false);

        /// Elimina un archivo o carpeta.
        Task<bool> DeleteFileAsync(int id, int userId);

        /// Crea una nueva carpeta.
        Task<FileMetadata> CreateFolderAsync(string name, int? parentId, int userId);

        /// Genera un ZIP con el contenido de una carpeta.
        Task<(Stream, string)> DownloadFolderAsZipAsync(int folderId, int userId);

        /// Marca/desmarca como destacado.
        Task<bool> ToggleFileStarAsync(int id);

        /// Comparte un archivo con otro usuario.
        Task<bool> ShareFileAsync(int fileId, string targetUsername, int currentUserId);

        /// Lista los archivos compartidos con el usuario.
        Task<IEnumerable<FileMetadata>> ListSharedFilesAsync(int userId);
    }
}
