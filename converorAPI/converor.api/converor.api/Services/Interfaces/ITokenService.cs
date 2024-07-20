using converor.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace converor.api.Services.Interfaces
{
    public interface ITokenService
    {
        Task<(string accessToken, string refreshToken)> GenerateTokensAsync(ApplicationUser user);
    }
}
