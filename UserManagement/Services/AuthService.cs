using UserManagement.Data;
using UserManagement.DTOs;
using UserManagement.Interfaces;
using UserManagement.Models.Entities;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net; // Add this namespace import


namespace UserManagement.Services
{
    public class AuthService: IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> ValidateUserAsync(LoginDTO model)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email && !u.IsDeleted);

            if (user == null || user.Status == "Blocked")
                return null;

            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                return null;

            return user;
        }

        public async Task<bool> IsUserValidAsync(int userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

            return user != null && user.Status != "Blocked";
        }
    }
}
