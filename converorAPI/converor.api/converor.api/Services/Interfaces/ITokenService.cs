using Microsoft.AspNetCore.Identity;

namespace converor.api.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(IdentityUser user);
    }
}
