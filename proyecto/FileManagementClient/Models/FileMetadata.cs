using System;

namespace FileManagementClient.Models
{
    public class FileMetadata
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string StoredFileName { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public DateTime UploadDate { get; set; }
        public bool IsFolder { get; set; }
        public int? ParentId { get; set; }
    }
}
