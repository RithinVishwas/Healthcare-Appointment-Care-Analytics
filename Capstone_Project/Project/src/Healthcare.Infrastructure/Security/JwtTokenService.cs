// File: src/Healthcare.Infrastructure/Security/JwtTokenService.cs
// Layer: Infrastructure/data-access layer
// Purpose: This file is the security service that creates JWT bearer tokens for Web API authentication.
// Security note: credentials and tokens are handled through hashing and JWT-based authentication practices.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Service file: contains reusable business/security logic separated from UI and controllers.

// Required namespaces are imported here so this file can use framework and project classes.
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
// Required namespaces are imported here so this file can use framework and project classes.
using System.Security.Claims;
using System.Text;
using Healthcare.Core.DTOs;
// Required namespaces are imported here so this file can use framework and project classes.
using Healthcare.Core.Entities;
using Healthcare.Core.Interfaces;
using Microsoft.Extensions.Configuration;
// Required namespaces are imported here so this file can use framework and project classes.
using Microsoft.IdentityModel.Tokens;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Infrastructure.Security
{
    /// <summary>JwtTokenService belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class JwtTokenService : IJwtTokenService
    {
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly IConfiguration _configuration;

        ///  <summary>Constructor receives dependencies from ASP.NET Core dependency injection instead of creating them manually.</summary>
        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>Creates a signed JWT token containing user identity and role claims.</summary>
        public LoginResponseDto CreateToken(AppUser user)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = _configuration["Jwt:Key"];
            var expiresInMinutes = int.Parse(_configuration["Jwt:ExpiresInMinutes"] ?? "60");

            var claims = new List<Claim>
            {
                // Claims store identity/role information that authorization policies can read later.
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                // Claims store identity/role information that authorization policies can read later.
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                // Claims store identity/role information that authorization policies can read later.
                new Claim(ClaimTypes.Name, user.FullName),
                // Claims store identity/role information that authorization policies can read later.
                new Claim(ClaimTypes.Role, user.Role.Name),
                // Claims store identity/role information that authorization policies can read later.
                new Claim("uid", user.Id.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            // JWT signing ensures clients cannot forge or tamper with authentication tokens.
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(expiresInMinutes);

            // JWT signing ensures clients cannot forge or tamper with authentication tokens.
            var token = new JwtSecurityToken(issuer, audience, claims, DateTime.UtcNow, expires, credentials);
            return new LoginResponseDto
            {
                // JWT signing ensures clients cannot forge or tamper with authentication tokens.
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.Name,
                ExpiresInMinutes = expiresInMinutes
            };
        }
    }
}
