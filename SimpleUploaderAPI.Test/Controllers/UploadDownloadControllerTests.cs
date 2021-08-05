using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using SimpleUploaderAPI.Controllers;
using SimpleUploaderAPI.Domain.Entities;
using SimpleUploaderAPI.Service.UploadDownloadService.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Xunit;

namespace SimpleUploaderAPI.Test.Controllers
{
    public class UploadDownloadControllerTests
    {
        private readonly UploadDownloadController _uploadDownloadController;
        private readonly Guid _id = Guid.Parse("35296ce1-e20f-4dc6-83c8-25b9152995e0");
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly List<FileData> files;

        public UploadDownloadControllerTests()
        {
            _mapper = A.Fake<IMapper>();
            _mediator = A.Fake<IMediator>();
            _hostEnvironment = A.Fake<IHostEnvironment>();
            _uploadDownloadController = new UploadDownloadController(_hostEnvironment, _mapper, _mediator);

            files = new List<FileData>
            {
                new FileData
                {
                    Id = _id,
                    FileName = Guid.NewGuid().ToString() + "_" + "avatar.png",
                    FileSize = 3121354,
                    FileType = "image/png",
                    UploadDate = DateTime.Now
                },
                new FileData
                {
                    Id = Guid.Parse("270b0d0f-cfde-4846-a67d-098166c333a1"),
                    FileName = Guid.NewGuid().ToString() + "_" + "test.png",
                    FileSize = 3121354,
                    FileType = "image/png",
                    UploadDate = DateTime.Now
                }
            };

            A.CallTo(() => _mediator.Send(A<CreateFileCommand>._, A<CancellationToken>._)).Returns(files.First());
        }

        [Theory]
        [InlineData("CreateFileDataAsync: file is null")]
        public async void Upload_WhenAnExceptionOccurs_ShouldReturnBadRequest(string exceptionMessage)
        {
            A.CallTo(() => _mediator.Send(A<CreateFileCommand>._, default)).Throws(new ArgumentException(exceptionMessage));

            var file = A.Fake<IFormFile>();
            var result = await _uploadDownloadController.Upload(file);

            (result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            (result.Result as BadRequestObjectResult)?.Value.Should().Be(exceptionMessage);
        }

        [Fact]
        public async void Upload_File_ShouldReturnOk()
        {
            A.CallTo(() => _mediator.Send(A<CreateFileCommand>._, default)).Returns(files.First());

            var filePath = Path.Combine(AppContext.BaseDirectory, "images", "avatar.png");

            using (var stream = File.OpenRead(filePath))
            {
                var file = new FormFile(stream, 0, stream.Length, null, "avatar.png")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg"
                };

                var result = await _uploadDownloadController.Upload(file);

                (result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
            }
        }

        [Fact]
        public async void Files_WhenAnExceptionOccurs_ShouldReturnInternalServerError()
        {
            A.CallTo(() => _mediator.Send(A<CreateFileCommand>._, A<CancellationToken>._)).Throws(new Exception());

            var file = A.Fake<IFormFile>();
            var result = await _uploadDownloadController.Upload(file);

            (result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}