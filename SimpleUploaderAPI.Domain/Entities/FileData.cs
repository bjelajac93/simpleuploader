using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleUploaderAPI.Domain.Entities
{
    [Table("files")]
    public partial class FileData
    {
        [Column("file_id")]
        [Key]
        public Guid Id { get; set; }
        [Column("file_name")]
        public string FileName { get; set; }
        [Column("file_size")]
        public long FileSize { get; set; }
        [Column("file_type")]
        public string FileType { get; set; }
        [Column("upload_date")]
        public DateTime? UploadDate { get; set; }
    }
}