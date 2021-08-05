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

        /// <summary>
        ///     Action to create a new file in the database and filesystem.
        /// </summary>
        /// <param name="file">Object to create a new file</param>
        /// <returns>Returns the created file</returns>
        /// <response code="200">Returned if the file was created</response>
        /// <response code="400">Returned if the model couldn't be parsed or saved</response>
        /// <response code="422">Returned when the validation failed</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
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
                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(fileStream);
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

        /// <summary>
        ///     Action to retrieve all files.
        /// </summary>
        /// <returns>Returns a file or handle exception, if no file in db yet and file system yet</returns>
        /// <response code="200">Returned if the file was retrieved</response>
        /// <response code="400">Returned if the file could not be retrieved</response>
        [HttpGet]
        [Route("download")]
        public async Task<IActionResult> Download([FromQuery] string file)
        {
            try
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        ///     Action to retrieve all files.
        /// </summary>
        /// <returns>Returns a list of all files or an empty list, if no files in db yet</returns>
        /// <response code="200">Returned if the list of files was retrieved</response>
        /// <response code="400">Returned if the files could not be retrieved</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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