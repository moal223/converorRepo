using converor.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using converor.api.Services.Interfaces;

namespace converor.api.Services
{
    public class TokenService : ITokenService
    {
        private readonly Jwt _jwtSettings;
        private readonly UserManager<IdentityUser> _userManager;
        public TokenService(IOptions<Jwt> jwtSettings, UserManager<IdentityUser> userManager)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
        }

        public async Task<string> GenerateToken(IdentityUser user)
        {
            // user main info
            var claims = new List<Claim>
            {
                new ("userid", user.Id.ToString()),
                new ("username", user.UserName),
                new ("email", user.Email),
                new ("id", Guid.NewGuid().ToString())
            };
            // user roles
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            // create the jwt token with the specified parameters
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.DurationMin),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)), SecurityAlgorithms.HmacSha256)
            );

            return await WriteTokenAsync(token);
        }

        private async Task<string> WriteTokenAsync(JwtSecurityToken token)
        {
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
