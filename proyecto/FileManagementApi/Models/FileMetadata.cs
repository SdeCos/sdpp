using System;
using System.ComponentModel.DataAnnotations;

namespace FileManagementApi.Models
{
    public class FileMetadata
    {
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public string StoredFileName { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long Size { get; set; }

        public DateTime UploadDate { get; set; } = DateTime.UtcNow;

        public bool IsFolder { get; set; }

        public int? ParentId { get; set; }

        public bool IsStarred { get; set; }

        public int UserId { get; set; }
    }
}
