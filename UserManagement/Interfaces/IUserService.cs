using UserManagement.DTOs;
using UserManagement.Models.Entities;

namespace UserManagement.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> CreateUserAsync(RegisterDTO model);
        Task<bool> BlockUsersAsync(List<int> userIds);
        Task<bool> UnblockUsersAsync(List<int> userIds);
        Task<bool> DeleteUsersAsync(List<int> userIds);
        Task UpdateLastLoginAsync(int userId);
        Task<bool> HasAnyUsersAsync();
        Task<List<User>> GetAllUsersForDebugAsync();
    }
}
