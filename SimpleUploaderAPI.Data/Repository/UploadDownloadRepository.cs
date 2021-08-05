using ApplicantsApi.Data.Database;
using ApplicantsApi.Data.Repository;
using Microsoft.EntityFrameworkCore;
using SimpleUploaderAPI.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleUploaderAPI.Data.Repository
{
    public class UploadDownloadRepository : Repository<FileData>, IUploadDownloadRepository
    {
        public UploadDownloadRepository(ApplicantContext applicantContext) : base(applicantContext)
        {
        }

        public async Task<List<FileData>> GetFiles(CancellationToken cancellationToken)
        {
            return await ApplicantContext.Files.ToListAsync(cancellationToken);
        }
    }
}