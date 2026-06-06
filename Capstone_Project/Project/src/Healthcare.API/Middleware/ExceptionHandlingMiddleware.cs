// File: src/Healthcare.API/Middleware/ExceptionHandlingMiddleware.cs
// Layer: Web API layer
// Purpose: This file is the middleware that converts unhandled exceptions into consistent HTTP responses without exposing sensitive details.
// Best-practice note: comments explain intent only; no business logic has been changed.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Middleware file: runs cross-cutting request pipeline logic such as security headers or exception handling.

// Required namespaces are imported here so this file can use framework and project classes.
using System;
using System.Net;
using System.Text.Json;
// Required namespaces are imported here so this file can use framework and project classes.
using System.Threading.Tasks;
using Healthcare.Core.Exceptions;
using Microsoft.AspNetCore.Http;
// Required namespaces are imported here so this file can use framework and project classes.
using Microsoft.Extensions.Logging;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.API.Middleware
{
    /// <summary>ExceptionHandlingMiddleware belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class ExceptionHandlingMiddleware
    {
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly RequestDelegate _next;
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        ///  <summary>Constructor receives dependencies from ASP.NET Core dependency injection instead of creating them manually.</summary>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>Runs middleware logic for each HTTP request in the pipeline.</summary>
        public async Task InvokeAsync(HttpContext context)
        {
            // The try block protects the request from unhandled runtime/database errors.
            try
            {
                await _next(context);
            }
            // The catch block converts exceptions into a controlled response instead of exposing stack traces.
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled API error");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex is BusinessRuleException ? (int)HttpStatusCode.BadRequest :
                    ex is NotFoundException ? (int)HttpStatusCode.NotFound :
                    (int)HttpStatusCode.InternalServerError;

                var response = new { message = ex is BusinessRuleException || ex is NotFoundException ? ex.Message : "Unexpected server error." };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
