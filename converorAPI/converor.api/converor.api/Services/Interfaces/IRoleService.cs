using converor.Core.Models;

namespace converor.api.Services.Interfaces
{
    public interface IRoleService
    {
        Task<bool> CreateRoleAsync(string role);
        void AddRoleToUserAsync(string role, ApplicationUser user);
    }
}
