using MediatR;
using SimpleUploaderAPI.Domain.Entities;

namespace SimpleUploaderAPI.Service.UploadDownloadService.Command
{
    public class CreateFileCommand : IRequest<FileData>
    {
        public FileData File { get; set; }
    }
}