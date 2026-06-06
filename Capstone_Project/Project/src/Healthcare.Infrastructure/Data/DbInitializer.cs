// File: src/Healthcare.Infrastructure/Data/DbInitializer.cs
// Layer: Infrastructure/data-access layer
// Purpose: This file is the database initializer used to seed required master data and demo records for the application.
// Best-practice note: comments explain intent only; no business logic has been changed.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: C# source file: part of the layered healthcare appointment system.

// Required namespaces are imported here so this file can use framework and project classes.
using System.Linq;
using System.Threading.Tasks;
using Healthcare.Core.Entities;
// Required namespaces are imported here so this file can use framework and project classes.
using Healthcare.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Infrastructure.Data
{
    /// <summary>DbInitializer belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class DbInitializer
    {
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly HealthcareDbContext _context;
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly IPasswordHasher _passwordHasher;

        ///  <summary>Constructor receives dependencies from ASP.NET Core dependency injection instead of creating them manually.</summary>
        public DbInitializer(HealthcareDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        /// <summary>Executes the SeedSecureAdminPasswordAsync workflow while keeping the logic inside the correct project layer.</summary>
        public async Task SeedSecureAdminPasswordAsync()
        {
            // Uses EF Core asynchronous database access to avoid blocking server threads.
            await _context.Database.EnsureCreatedAsync();
            // Reads data asynchronously from SQL Server through EF Core.
            // FirstOrDefaultAsync safely returns null when no matching row is found, so the code can handle missing records.
            // Include eagerly loads related data to avoid missing navigation details in the response/view.
            var admin = await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == "admin@healthcare.local");
            // Password handling uses hashed values; this protects the application if database rows are exposed.
            if (admin != null && admin.PasswordHash.Contains("DemoHashChangeUsingDbInitializer"))
            {
                // Password handling uses hashed values; this protects the application if database rows are exposed.
                admin.PasswordHash = _passwordHasher.HashPassword("Admin@123");
                // Uses EF Core asynchronous database access to avoid blocking server threads.
                await _context.SaveChangesAsync();
            }
        }
    }
}
