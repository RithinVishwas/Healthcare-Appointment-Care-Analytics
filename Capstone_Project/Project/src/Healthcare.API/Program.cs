// File: src/Healthcare.API/Program.cs
// Layer: Web API layer
// Purpose: This file is the application startup file that configures dependency injection, routing, authentication, authorization, middleware, and runtime services.
// Security note: authentication, authorization, antiforgery, and validation are handled through ASP.NET Core middleware/attributes.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Startup file: configures dependency injection, authentication, middleware, routing, and application startup.

// Required namespaces are imported here so this file can use framework and project classes.
using System.Text;
using Healthcare.API.Middleware;
using Healthcare.Infrastructure;
// Required namespaces are imported here so this file can use framework and project classes.
using Healthcare.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
// Required namespaces are imported here so this file can use framework and project classes.
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
// Required namespaces are imported here so this file can use framework and project classes.
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.API
{
    /// <summary>Program belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class Program
    {
        /// <summary>Executes the Main workflow while keeping the logic inside the correct project layer.</summary>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Registers framework or application services in the dependency injection container.
            builder.Services.AddControllers();
            // Registers framework or application services in the dependency injection container.
            builder.Services.AddHealthcareInfrastructure(configuration);
            // Registers framework or application services in the dependency injection container.
            builder.Services.AddEndpointsApiExplorer();
            // Registers framework or application services in the dependency injection container.
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Healthcare Appointment API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token as: Bearer {token}"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new string[] { }
                    }
                });
            });

            // Registers framework or application services in the dependency injection container.
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                });

            // Registers framework or application services in the dependency injection container.
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("ClinicalStaff", policy => policy.RequireRole("Admin", "Doctor", "Staff"));
            });

            // Registers framework or application services in the dependency injection container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("TrustedClients", policy =>
                    policy.WithOrigins("https://localhost:5003", "http://localhost:5002")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Required namespaces are imported here so this file can use framework and project classes.
            using (var scope = app.Services.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
                initializer.SeedSecureAdminPasswordAsync().GetAwaiter().GetResult();
            }

            // Adds custom middleware to the ASP.NET Core request pipeline.
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            // Adds custom middleware to the ASP.NET Core request pipeline.
            app.UseMiddleware<SecurityHeadersMiddleware>();
            // Redirects HTTP traffic to HTTPS to protect data in transit.
            app.UseHttpsRedirection();
            app.UseCors("TrustedClients");
            // Enables authentication middleware before protected endpoints are executed.
            app.UseAuthentication();
            // Enables authorization checks after authentication identifies the user.
            app.UseAuthorization();
            // Maps controller routes/endpoints so incoming requests reach the correct action.
            app.MapControllers();
            app.Run();
        }
    }
}
