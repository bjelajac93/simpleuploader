using System;

namespace SimpleUploaderAPI.Models
{
    public class FileDataModel
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }
        public DateTime? UploadDate { get; set; }
    }
}