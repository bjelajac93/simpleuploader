using System;

namespace SimpleUploaderAPI.Models
{
    public class CreateFileDataModel {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }
    }
}