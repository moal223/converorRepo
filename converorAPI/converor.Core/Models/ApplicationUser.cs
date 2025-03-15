using Microsoft.AspNetCore.Identity;

namespace converor.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Folder>? Folders { get; set; } = new();
        public List<RefreshToken>? RefreshTokens { get; set; } = new();
    }
}
