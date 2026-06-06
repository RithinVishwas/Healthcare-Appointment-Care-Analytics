// File: src/Healthcare.API/Middleware/SecurityHeadersMiddleware.cs
// Layer: Web API layer
// Purpose: This file is the middleware that adds security-related HTTP headers to reduce common browser-based attacks.
// Best-practice note: comments explain intent only; no business logic has been changed.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Middleware file: runs cross-cutting request pipeline logic such as security headers or exception handling.

// Required namespaces are imported here so this file can use framework and project classes.
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.API.Middleware
{
    /// <summary>SecurityHeadersMiddleware belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class SecurityHeadersMiddleware
    {
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly RequestDelegate _next;

        ///  <summary>Constructor receives dependencies from ASP.NET Core dependency injection instead of creating them manually.</summary>
        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>Runs middleware logic for each HTTP request in the pipeline.</summary>
        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["X-Frame-Options"] = "DENY";
            context.Response.Headers["Referrer-Policy"] = "no-referrer";
            context.Response.Headers["Content-Security-Policy"] = "default-src 'self'; frame-ancestors 'none';";
            await _next(context);
        }
    }
}
