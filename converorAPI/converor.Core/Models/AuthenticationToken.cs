using System.ComponentModel.DataAnnotations;

namespace converor.Core.Models
{
    public class AuthenticationToken
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        [StringLength(512)]
        public string? Description { get; set; }
        public string Token { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

    }
}
