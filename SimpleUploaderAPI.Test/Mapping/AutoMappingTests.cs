using ApplicantsApi.Mapping;
using AutoMapper;
using FluentAssertions;
using SimpleUploaderAPI.Domain.Entities;
using SimpleUploaderAPI.Models;
using System;
using Xunit;

namespace OrderApi.Test.Infrastrucutre.Automapper
{
    public class AutoMappingTests
    {
        private readonly IMapper _mapper;
        private readonly CreateFileDataModel _fileDataModel;

        public AutoMappingTests()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapping());
            });
            _mapper = mockMapper.CreateMapper();

            _fileDataModel = new CreateFileDataModel
            {
                FileName = Guid.NewGuid().ToString() + "_" + "test.txt",
                FileSize = 1546541,
                FileType = "text/plain"
            };
        }

        [Fact]
        public void Map_FileData_FileDataModel_ShouldHaveValidConfig()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FileData, FileDataModel>();
                cfg.CreateMap<CreateFileDataModel, FileData>()
                .ForMember(q => q.Id, option => option.Ignore())
                .ForMember(q => q.UploadDate, option => option.Ignore());
            });

            configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void Map_CreateFileDataModel_FileData()
        {
            var file = _mapper.Map<FileData>(_fileDataModel);
            file.FileName.Should().Be(_fileDataModel.FileName);
            file.FileSize.Should().Be(_fileDataModel.FileSize);
            file.FileType.Should().Be(_fileDataModel.FileType);
        }
    }
}