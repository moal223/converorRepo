using Microsoft.AspNetCore.Identity;

namespace converor.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<FileDescription>? Files { get; set; } = new();
        public List<RefreshToken>? RefreshTokens { get; set; } = new();
    }
}
