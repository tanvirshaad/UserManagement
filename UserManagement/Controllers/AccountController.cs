using Microsoft.AspNetCore.Mvc;
using UserManagement.DTOs;
using UserManagement.Interfaces;
using UserManagement.Models.Entities;

namespace UserManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AccountController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login(string? error = null)
        {
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = error switch
            {
                "invalid_credentials" => "Invalid email or password.",
                "account_blocked" => "Your account has been blocked.",
                "authentication_required" => "Please log in to continue.",
                _ => null
            };

            return View(new LoginDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userService.GetUserByEmailAsync(model.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            if (user.Status == "Blocked")
            {
                ModelState.AddModelError("", "Your account has been blocked.");
                return View(model);
            }

            if (user.IsDeleted)
            {
                ModelState.AddModelError("", "Account not found.");
                return View(model);
            }

            // Update last login
            await _userService.UpdateLastLoginAsync(user.Id);

            // Set session
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new RegisterDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = await _userService.CreateUserAsync(model);
            if (!success)
            {
                Console.WriteLine(model);
                ModelState.AddModelError("Email", "Email already exists.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Registration successful! Please log in.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
} 