// File: src/Healthcare.MVC/Program.cs
// Layer: MVC presentation layer
// Purpose: This file is the application startup file that configures dependency injection, routing, authentication, authorization, middleware, and runtime services.
// Security note: authentication, authorization, antiforgery, and validation are handled through ASP.NET Core middleware/attributes.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Startup file: configures dependency injection, authentication, middleware, routing, and application startup.

// Required namespaces are imported here so this file can use framework and project classes.
using Healthcare.Infrastructure;
using Healthcare.Infrastructure.Data;
using Healthcare.MVC.Middleware;
// Required namespaces are imported here so this file can use framework and project classes.
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
// Required namespaces are imported here so this file can use framework and project classes.
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.MVC
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
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
            // Registers framework or application services in the dependency injection container.
            builder.Services.AddHealthcareInfrastructure(configuration);
            // Registers framework or application services in the dependency injection container.
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
                    options.SlidingExpiration = true;
                });
            // Registers framework or application services in the dependency injection container.
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            });

            var app = builder.Build();

            // Required namespaces are imported here so this file can use framework and project classes.
            using (var scope = app.Services.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
                initializer.SeedSecureAdminPasswordAsync().GetAwaiter().GetResult();
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Adds custom middleware to the ASP.NET Core request pipeline.
            app.UseMiddleware<SecurityHeadersMiddleware>();
            // Redirects HTTP traffic to HTTPS to protect data in transit.
            app.UseHttpsRedirection();
            // Serves static CSS, JavaScript, and image files from wwwroot.
            app.UseStaticFiles();
            app.UseRouting();
            // Enables authentication middleware before protected endpoints are executed.
            app.UseAuthentication();
            // Enables authorization checks after authentication identifies the user.
            app.UseAuthorization();
            // Maps controller routes/endpoints so incoming requests reach the correct action.
            app.MapControllerRoute(name: "default", pattern: "{controller=Dashboard}/{action=Index}/{id?}");
            app.Run();
        }
    }
}
