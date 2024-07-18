using converor.api.Dtos;
using converor.api.Dtos.Authentication;
using converor.api.Services.Interfaces;
using converor.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace converor.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        public AuthenticationController(ITokenService tokenService, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ILogger<AuthenticationController> logger)
        {
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }
        // Register
        [HttpPost("auth/register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            try
            {
                // check the properties validation
                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponse(state: false, message: ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage).ToList(), null));
                }

                // create the user
                var user = new ApplicationUser { Email = model.Email, UserName = model.Email.Split('@')[0] };
                var result = await _userManager.CreateAsync(user, model.Password);

                // return the token
                if (result.Succeeded)
                {
                    return Ok(new BaseResponse(true, new List<string> { "Success" }, await _tokenService.GenerateToken(user)));
                }

                return BadRequest(new BaseResponse(false, result.Errors.Select(e => e.Description).ToList(), null));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "an error occurred while registering a user.");
                return StatusCode(500, new BaseResponse(false, new List<string> { "An error occurred while processing your request" }, null));
            }
        }
        // Login
        [HttpPost("auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                // check the properties validation
                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponse(state: false, message: ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage).ToList(), null));
                }

                // check if the user registerd
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
                    if (result.Succeeded)
                    {
                        return Ok(new BaseResponse(true, new List<string> { "Success" }, await _tokenService.GenerateToken(user)));
                    }
                    return Unauthorized(new BaseResponse(false, new List<string> { "the password is not correct." }, null));
                }
                return BadRequest(new BaseResponse(false, new List<string> { "the user not registered." }, null));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "an error occurred while logging a user.");
                return StatusCode(500, new BaseResponse(false, new List<string> { "An error occurred while processing your request" }, null));
            }
        }
        // Logout
    }
}
