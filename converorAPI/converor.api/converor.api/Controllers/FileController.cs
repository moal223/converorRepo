using System.Security.Claims;
using converor.api.Dtos;
using converor.api.Dtos.Files;
using converor.api.Mapping;
using converor.api.Services.Interfaces;
using converor.Core.Models;
using converor.EF.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace converor.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly IFileDescriptionRepo _fileRepo;
        private readonly ILogger<FileController> _logger;

        public FileController(IFileDescriptionRepo fileRepo, ILogger<FileController> logger)
        {
            _fileRepo = fileRepo;
            _logger = logger;
            
        }
        [RequestSizeLimit(100_000_000)] // 100 MB
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm]IFormFile file)
        {
            try
            {
                // check the properties validation
                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponse(state: false, message: ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage).ToList(), null));
                }
                var userId = User.FindFirstValue("userid");

                // extract the file description
                var fileDescription = GetDescription(file);

                // save the file 
                // fileDescription.UserId = userId;
                await _fileRepo.InsertAsync(fileDescription);
                await _fileRepo.SaveAsync();

                return Ok(new BaseResponse(true, new List<string> { "Uploaded Successfuly" }, new GetRequestDto { 
                    Id = fileDescription.Id,
                    ContentDisposition = fileDescription.ContentDisposition,
                    ContentType = fileDescription.ContentType,
                    FileName = fileDescription.FileName,
                    Size = fileDescription.Size,
                    UploadedDate = fileDescription.UploadedDate,
                    UserId = userId
                }));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "an error occurred while uploading the file.");
                return StatusCode(500, new BaseResponse(false, new List<string> { "An error occurred while processing your request" }, null));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Download([FromRoute]int id)
        {
            try
            {
                var fileDescription = await _fileRepo.GetByIdAsync(id);
                if (fileDescription == null)
                    return NotFound();

                var cd = $"attachment; filename=\"{fileDescription.FileName}\"";
                Response.Headers.TryAdd("Content-Disposition", cd);

                return File(fileDescription.Content.Content, fileDescription.ContentType);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "an error occurred while downloading the file.");
                return StatusCode(500, new BaseResponse(false, new List<string> { "An error occurred while processing your request" }, null));
            }
        }

        [Authorize]
        [HttpGet("get-files")]
        public async Task<IActionResult> GetAllWithoutContent(){
            string UID = User.FindFirstValue("userid");
            if(UID == null || UID == "")
                return Unauthorized();
            
            var files = await _fileRepo.GetAllAsync(UID);
            var filesDto = files.ToGetFileDescDto();
            return Ok(filesDto);
        }

        #region Methods
        private FileDescription GetDescription(IFormFile file)
        {
            byte[] fileBytes;

            using(var fs = file.OpenReadStream())
            {
                using(var sr = new BinaryReader(fs)) {
                    fileBytes = sr.ReadBytes((int)file.Length);
                }
            }
            var fileContent = new FileContent
            {
                Content = fileBytes
            };
            return new FileDescription { FileName = Path.GetFileName(file.FileName),
                Content = fileContent,
                UploadedDate = DateTime.UtcNow,
                ContentType = file.ContentType,
                ContentDisposition = file.ContentDisposition,
                Size = file.Length
            };
        }
        #endregion
    }
}
