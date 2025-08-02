using UserManagement.DTOs;
using UserManagement.Models.Entities;

namespace UserManagement.Interfaces
{
    public interface IAuthService
    {
        Task<User?> ValidateUserAsync(LoginDTO model);
        Task<bool> IsUserValidAsync(int userId);
    }
}
