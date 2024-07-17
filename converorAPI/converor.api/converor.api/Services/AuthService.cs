using converor.api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace converor.api.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        public AuthService(ITokenService tokenService, UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _tokenService = tokenService;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public void Signin()
        {
            throw new NotImplementedException();
        }

        public void Signout()
        {
            throw new NotImplementedException();
        }

        public async Task<object> Signup(IdentityUser user)
        {
            List<string> Message = new();
            var result = await _userManager.CreateAsync(user);
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
