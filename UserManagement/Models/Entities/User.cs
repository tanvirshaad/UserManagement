using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;

        [StringLength(20)]
        public string Status { get; set; } = "Active";

        public DateTime? LastLogin { get; set; }

        public DateTime RegistrationTime { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;
    }
}
