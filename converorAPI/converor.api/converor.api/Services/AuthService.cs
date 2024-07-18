using converor.api.Dtos.Authentication;
using converor.api.Services.Interfaces;
using converor.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace converor.api.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        public AuthService(ITokenService tokenService, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _tokenService = tokenService;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<object> Signin(ApplicationUser user, string password)
        {
            List<string> Message = new();

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (result.Succeeded)
            {
                Message.Add(await _tokenService.GenerateToken(user));
                return new { state = true, Message };
            }
            Message.Add("Un Authorized");
            return new { state = false, Message };
        }

        public void Signout()
        {
            throw new NotImplementedException();
        }

        public async Task<object> Signup(ApplicationUser user, string password)
        {
            List<string> Message = new();
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                Message.Add(await _tokenService.GenerateToken(user));
                return new { state = true, Message };
            }

            foreach(var error in result.Errors)
            {
                Message.Add(error.Code);
            }
            return new { state = false, Message };
        }
    }
}
