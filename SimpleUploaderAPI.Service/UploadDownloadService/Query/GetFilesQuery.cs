using MediatR;
using SimpleUploaderAPI.Domain.Entities;
using System.Collections.Generic;

namespace SimpleUploaderAPI.Service.UploadDownloadService.Query
{
    public class GetFilesQuery : IRequest<List<FileData>>
    {
    }
}