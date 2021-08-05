using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using SimpleUploaderAPI.Domain.Entities;
using SimpleUploaderAPI.Helper;
using SimpleUploaderAPI.Models;
using SimpleUploaderAPI.Service.UploadDownloadService.Command;
using SimpleUploaderAPI.Service.UploadDownloadService.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SimpleUploaderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadDownloadController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IHostEnvironment _hostEnvironment;

        public UploadDownloadController(IHostEnvironment environment, IMapper mapper, IMediator mediator)
        {
            _hostEnvironment = environment;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("upload")]
        public async Task<ActionResult<FileDataModel>> Upload(IFormFile file)
        {
            try
            {
                string guid = Guid.NewGuid().ToString();
                var uploads = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "uploads");

                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                if (file.Length > 0)
                {
                    var filePath = Path.Combine(uploads, guid + "_" + file.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                return _mapper.Map<FileDataModel>(await _mediator.Send(new CreateFileCommand
                {
                    File = _mapper.Map<FileData>(new CreateFileDataModel()
                    {
                        FileName = guid + "_" + file.FileName,
                        FileSize = file.Length,
                        FileType = file.ContentType
                    })
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("download")]
        public async Task<IActionResult> Download([FromQuery] string file)
        {
            var uploads = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "uploads");
            var filePath = Path.Combine(uploads, file);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, FileContentType.GetContentType(filePath), file);
        }

        [HttpGet]
        [Route("files")]
        public async Task<ActionResult<List<FileDataModel>>> Files()
        {
            try
            {
                return _mapper.Map<List<FileDataModel>>(await _mediator.Send(new GetFilesQuery()));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}