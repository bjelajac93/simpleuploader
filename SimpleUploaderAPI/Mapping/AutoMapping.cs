using AutoMapper;
using SimpleUploaderAPI.Domain.Entities;
using SimpleUploaderAPI.Models;

namespace ApplicantsApi.Mapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<FileData, FileDataModel>();

            CreateMap<CreateFileDataModel, FileData>()
                .ForMember(q => q.Id, option => option.Ignore())
                .ForMember(q => q.UploadDate, option => option.Ignore());
        }
    }
}