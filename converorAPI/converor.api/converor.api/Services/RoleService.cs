using converor.api.Services.Interfaces;
using converor.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace converor.api.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async void AddRoleToUserAsync(string role, ApplicationUser user)
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<bool> CreateRoleAsync(string role)
        {
            // check if the role exist
            var result = await _roleManager.RoleExistsAsync(role);

            // create the role
            if (!result)
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
                return true;
            }
            return false;
        }
    }
}
