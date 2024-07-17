using Microsoft.AspNetCore.Identity;

namespace converor.api.Services.Interfaces
{
    public interface IAuthService
    {
        void Signin();
        Task<object> Signup(IdentityUser user);
        void Signout();
    }
}
