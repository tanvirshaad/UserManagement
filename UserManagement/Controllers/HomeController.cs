using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UserManagement.DTOs;
using UserManagement.Interfaces;
using UserManagement.Models;
using UserManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _context;

        public HomeController(IUserService userService, IAuthService authService, ApplicationDbContext context)
        {
            _userService = userService;
            _authService = authService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account", new { error = "authentication_required" });

            if (!await _authService.IsUserValidAsync(userId.Value))
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Account", new { error = "account_blocked" });
            }

            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> BlockUsers([FromBody] List<int> userIds)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue || !await _authService.IsUserValidAsync(userId.Value))
            {
                return Json(new { success = false, redirect = true });
            }

            var success = await _userService.BlockUsersAsync(userIds);
            return Json(new { success, message = success ? $"{userIds.Count} user(s) blocked successfully" : "Failed to block users" });
        }

        [HttpPost]
        public async Task<IActionResult> UnblockUsers([FromBody] List<int> userIds)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue || !await _authService.IsUserValidAsync(userId.Value))
            {
                return Json(new { success = false, redirect = true });
            }

            var success = await _userService.UnblockUsersAsync(userIds);
            return Json(new { success, message = success ? $"{userIds.Count} user(s) unblocked successfully" : "Failed to unblock users" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUsers([FromBody] List<int> userIds)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue || !await _authService.IsUserValidAsync(userId.Value))
            {
                return Json(new { success = false, redirect = true });
            }

            var success = await _userService.DeleteUsersAsync(userIds);
            return Json(new { success, message = success ? $"{userIds.Count} user(s) deleted successfully" : "Failed to delete users" });
        }

        
    }
}
