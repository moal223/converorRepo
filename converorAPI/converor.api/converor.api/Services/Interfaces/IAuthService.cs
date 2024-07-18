using converor.api.Dtos.Authentication;
using converor.Core.Models;

namespace converor.api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<object> Signin(ApplicationUser user, string password);
        Task<object> Signup(ApplicationUser user, string password);
        void Signout();
    }
}
