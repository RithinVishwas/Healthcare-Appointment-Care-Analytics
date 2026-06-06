// File: src/Healthcare.MVC/Controllers/AccountController.cs
// Purpose: Handles login, logout, registration, authentication cookies, and role-based redirects.
// Security: Uses PBKDF2 password hashing, anti-forgery validation, ModelState validation, and cookie authentication.

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Healthcare.Core.Entities;
using Healthcare.Core.Interfaces;
using Healthcare.Infrastructure.Data;
using Healthcare.MVC.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Healthcare.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly HealthcareDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public AccountController(HealthcareDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Shows the login form for both Admin and normal User accounts.
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Step 1: Reject incomplete or invalid input before querying the database.
            if (!ModelState.IsValid) return View(model);

            // Step 2: Load the active user and their role using EF Core parameterized query.
            // This protects against SQL injection because no raw SQL string is built from user input.
            var user = await _context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == model.Email && x.IsActive);

            // Step 3: Verify the entered password against the stored PBKDF2 hash.
            // Plain text passwords are never stored in the database.
            if (user == null || !_passwordHasher.VerifyPassword(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            // Step 4: Create claims. Claims are stored in the authentication cookie and used by [Authorize].
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            // Step 5: Redirect based on role.
            // Admin goes to the admin dashboard; normal User goes to the patient self-service portal.
            if (user.Role.Name == "User")
            {
                return RedirectToAction("Dashboard", "PatientPortal");
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public IActionResult Register()
        {
            // Shows the new-user registration page when the user does not already have an account.
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Step 1: Validate form fields before creating a database record.
            if (!ModelState.IsValid) return View(model);

            // Step 2: Prevent duplicate accounts using email as the unique login identifier.
            var emailExists = await _context.Users.AnyAsync(x => x.Email == model.Email);
            if (emailExists)
            {
                ModelState.AddModelError(nameof(model.Email), "An account already exists with this email.");
                return View(model);
            }

            // Step 3: Ensure the normal User role exists.
            // If you ran the 07_user_portal_update.sql script, this role will already exist.
            var userRole = await _context.Roles.FirstOrDefaultAsync(x => x.Name == "User" && x.IsActive);
            if (userRole == null)
            {
                userRole = new AppRole
                {
                    Name = "User",
                    CreatedAtUtc = DateTime.UtcNow,
                    IsActive = true
                };
                _context.Roles.Add(userRole);
                await _context.SaveChangesAsync("system", default);
            }

            // Step 4: Create the user login account.
            // PatientId is null here because profile creation happens after first login.
            var user = new AppUser
            {
                FullName = model.FullName,
                Email = model.Email,
                PasswordHash = _passwordHasher.HashPassword(model.Password),
                RoleId = userRole.Id,
                PatientId = null,
                CreatedAtUtc = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync("register", default);

            TempData["Success"] = "Registration successful. Please login and create your patient profile.";
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Clears the authentication cookie and signs the user out safely.
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
