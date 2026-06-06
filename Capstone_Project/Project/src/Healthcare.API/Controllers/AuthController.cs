// File: src/Healthcare.API/Controllers/AuthController.cs
// Layer: Web API layer
// Purpose: This file is the Web API controller responsible for login and JWT token generation.
// Security note: authentication, authorization, antiforgery, and validation are handled through ASP.NET Core middleware/attributes.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Controller file: receives HTTP/MVC requests, validates input, calls services/repositories, and returns views or API responses.

// Required namespaces are imported here so this file can use framework and project classes.
using System.Linq;
using System.Threading.Tasks;
using Healthcare.Core.DTOs;
// Required namespaces are imported here so this file can use framework and project classes.
using Healthcare.Core.Interfaces;
using Healthcare.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
// Required namespaces are imported here so this file can use framework and project classes.
using Microsoft.EntityFrameworkCore;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.API.Controllers
{
    // Marks this class as a Web API controller and enables automatic model validation behavior.
    [ApiController]
    // Defines the route pattern used to reach this controller or action.
    [Route("api/[controller]")]
    /// <summary>AuthController belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class AuthController : ControllerBase
    {
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly HealthcareDbContext _context;
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly IPasswordHasher _passwordHasher;
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly IJwtTokenService _jwtTokenService;

        ///  <summary>Constructor receives dependencies from ASP.NET Core dependency injection instead of creating them manually.</summary>
        public AuthController(HealthcareDbContext context, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        // Maps this action to a specific HTTP verb and route for REST/MVC routing.
        [HttpPost("login")]
        /// <summary>Handles user login, validates credentials, and returns a JWT token or MVC authentication cookie.</summary>
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            // Stops processing when validation fails, preventing invalid input from reaching the database.
            if (!ModelState.IsValid)
            {
                // Returns validation errors to the API client instead of saving bad data.
                return BadRequest(ModelState);
            }

            // Loads the user with role details so authentication and authorization can be checked.
            // FirstOrDefaultAsync safely returns null when no matching row is found, so the code can handle missing records.
            // Include eagerly loads related data to avoid missing navigation details in the response/view.
            var user = await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == request.Email && x.IsActive);
            // Password handling uses hashed values; this protects the application if database rows are exposed.
            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                // Returns 401 when credentials are missing or invalid.
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // Returns a successful API response with the requested data.
            return Ok(_jwtTokenService.CreateToken(user));
        }
    }
}
