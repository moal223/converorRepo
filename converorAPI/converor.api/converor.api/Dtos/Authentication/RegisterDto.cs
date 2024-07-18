using System.ComponentModel.DataAnnotations;

namespace converor.api.Dtos.Authentication
{
    public class RegisterDto : BaseUserAuthDto
    {
        [Required(ErrorMessage = "Comfirm password is required.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and comfirm password are not the same.")]
        public string ConfirmPassword { get; set; }
    }
}
