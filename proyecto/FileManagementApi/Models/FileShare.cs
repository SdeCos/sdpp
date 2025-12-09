using System;

namespace FileManagementApi.Models
{
    /// Representa un registro de archivo compartido entre usuarios.
    public class FileShare
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public int SharedByUserId { get; set; }
        public int SharedWithUserId { get; set; }
        public DateTime SharedDate { get; set; } = DateTime.UtcNow;
    }
}
