using MediatR;
using SimpleUploaderAPI.Data.Repository;
using SimpleUploaderAPI.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleUploaderAPI.Service.UploadDownloadService.Command
{
    public class CreateFileCommandHandler : IRequestHandler<CreateFileCommand, FileData>
    {
        private readonly IUploadDownloadRepository _uploadDownloadRepository;

        public CreateFileCommandHandler(IUploadDownloadRepository uploadDownloadRepository)
        {
            _uploadDownloadRepository = uploadDownloadRepository;
        }

        public async Task<FileData> Handle(CreateFileCommand request, CancellationToken cancellationToken)
        {
            request.File.UploadDate = DateTime.Now;
            return await _uploadDownloadRepository.AddAsync(request.File);
        }
    }
}