using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
//using Org.BouncyCastle.Crypto.Generators;
using UserManagement.Data;
using UserManagement.DTOs;
using UserManagement.Interfaces;
using UserManagement.Models.Entities;

namespace UserManagement.Services
{
    public class UserService: IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            return await _context.Users
                .Where(u => !u.IsDeleted)
                .OrderByDescending(u => u.LastLogin ?? DateTime.MinValue)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    LastLogin = u.LastLogin,
                    Status = u.Status,
                    RegistrationTime = u.RegistrationTime
                })
                .ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        }

        public async Task<bool> CreateUserAsync(RegisterDTO model)
        {
            try
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = hashedPassword,
                    Status = "Active",
                    RegistrationTime = DateTime.UtcNow,
                    IsDeleted = false
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Users_Email_Unique") == true)
            {
                return false; // Email already exists
            }
            
        }

        public async Task<bool> BlockUsersAsync(List<int> userIds)
        {
            try
            {
                await _context.Users
                    .Where(u => userIds.Contains(u.Id) && !u.IsDeleted)
                    .ExecuteUpdateAsync(u => u.SetProperty(p => p.Status, "Blocked"));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UnblockUsersAsync(List<int> userIds)
        {
            try
            {
                await _context.Users
                    .Where(u => userIds.Contains(u.Id) && !u.IsDeleted)
                    .ExecuteUpdateAsync(u => u.SetProperty(p => p.Status, "Active"));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteUsersAsync(List<int> userIds)
        {
            try
            {
                await _context.Users
                    .Where(u => userIds.Contains(u.Id))
                    .ExecuteUpdateAsync(u => u.SetProperty(p => p.IsDeleted, true));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task UpdateLastLoginAsync(int userId)
        {
            await _context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(u => u.SetProperty(p => p.LastLogin, DateTime.UtcNow));
        }

        // Helper method to check if database has any users
        public async Task<bool> HasAnyUsersAsync()
        {
            return await _context.Users.AnyAsync();
        }

        // Helper method to get all users (including deleted ones) for debugging
        public async Task<List<User>> GetAllUsersForDebugAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
