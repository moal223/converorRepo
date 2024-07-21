using converor.api.Dtos;
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

                return Ok(fileDescription);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "an error occurred while uploading the file.");
                return StatusCode(500, new BaseResponse(false, new List<string> { "An error occurred while processing your request" }, null));
            }
        }

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
    }
}
