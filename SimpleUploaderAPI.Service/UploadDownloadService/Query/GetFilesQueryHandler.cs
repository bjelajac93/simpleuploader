using MediatR;
using SimpleUploaderAPI.Data.Repository;
using SimpleUploaderAPI.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleUploaderAPI.Service.UploadDownloadService.Query
{
    public class GetFilesQueryHandler : IRequestHandler<GetFilesQuery, List<FileData>>
    {
        private readonly IUploadDownloadRepository _uploadDownloadRepository;

        public GetFilesQueryHandler(IUploadDownloadRepository uploadDownloadRepository)
        {
            _uploadDownloadRepository = uploadDownloadRepository;
        }

        public async Task<List<FileData>> Handle(GetFilesQuery request, CancellationToken cancellationToken)
        {
            return await _uploadDownloadRepository.GetFiles(cancellationToken);
        }
    }
}