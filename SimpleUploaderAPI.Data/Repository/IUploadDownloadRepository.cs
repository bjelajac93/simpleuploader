using ApplicantsApi.Data.Repository;
using SimpleUploaderAPI.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleUploaderAPI.Data.Repository
{
    public interface IUploadDownloadRepository : IRepository<FileData>
    {
        Task<List<FileData>> GetFiles(CancellationToken cancellationToken);
    }
}