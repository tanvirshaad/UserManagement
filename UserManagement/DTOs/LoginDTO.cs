using System.ComponentModel.DataAnnotations;

namespace UserManagement.DTOs
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;
    }
}
