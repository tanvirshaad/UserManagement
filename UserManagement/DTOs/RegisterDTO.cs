using System.ComponentModel.DataAnnotations;

namespace UserManagement.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Password")]
        [MinLength(1, ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; } = string.Empty;
    }
}
