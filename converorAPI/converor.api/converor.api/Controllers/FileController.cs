using converor.api.Dtos;
using converor.api.Dtos.Files;
using converor.api.Services.Interfaces;
using converor.Core.Models;
using converor.EF.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace converor.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileDescriptionRepo _fileRepo;
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<FileController> _logger;
        public FileController(IFileDescriptionRepo fileRepo, ITokenService tokenService,
            UserManager<ApplicationUser> userManager, ILogger<FileController> logger)
        {
            _fileRepo = fileRepo;
            _tokenService = tokenService;
            _userManager = userManager;
            _logger = logger;

        }
        [RequestSizeLimit(100_000_000)] // 100 MB
        [HttpPost]
        public async Task<IActionResult> Upload([FromQuery]string access, IFormFile file)
        {
            try
            {
                // check the properties validation
                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponse(state: false, message: ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage).ToList(), null));
                }
                var userId = _tokenService.GetUserIdFromToken(access);

                // extract the file description
                var fileDescription = GetDescription(file);

                // save the file 
                fileDescription.UserId = userId;
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

        [HttpGet("/{id}")]
        public async Task<IActionResult> Download(int id)
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
