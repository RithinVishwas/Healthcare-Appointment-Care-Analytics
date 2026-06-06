// File: src/Healthcare.Infrastructure/DependencyInjection.cs
// Layer: Infrastructure/data-access layer
// Purpose: This file is the code/configuration file that supports the Healthcare Appointment & Care Analytics application.
// Best-practice note: comments explain intent only; no business logic has been changed.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: C# source file: part of the layered healthcare appointment system.

// Required namespaces are imported here so this file can use framework and project classes.
using Healthcare.Core.Entities;
using Healthcare.Core.Interfaces;
using Healthcare.Core.Services;
// Required namespaces are imported here so this file can use framework and project classes.
using Healthcare.Infrastructure.Data;
using Healthcare.Infrastructure.Repositories;
using Healthcare.Infrastructure.Security;
// Required namespaces are imported here so this file can use framework and project classes.
using Healthcare.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
// Required namespaces are imported here so this file can use framework and project classes.
using Microsoft.Extensions.DependencyInjection;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Infrastructure
{
    public static class DependencyInjection
    {
        /// <summary>Executes the AddHealthcareInfrastructure workflow while keeping the logic inside the correct project layer.</summary>
        public static IServiceCollection AddHealthcareInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<HealthcareDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Registers a scoped dependency so each web request gets its own safe service instance.
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // Registers a scoped dependency so each web request gets its own safe service instance.
            services.AddScoped<IRepository<Patient>, PatientRepository>();
            // Registers a scoped dependency so each web request gets its own safe service instance.
            services.AddScoped<IRepository<Doctor>, Repository<Doctor>>();
            // Registers a scoped dependency so each web request gets its own safe service instance.
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            // Registers a scoped dependency so each web request gets its own safe service instance.
            services.AddScoped<IAppointmentDomainService, AppointmentDomainService>();
            // Registers a scoped dependency so each web request gets its own safe service instance.
            services.AddScoped<IAnalyticsReportService, SqlAnalyticsReportService>();
            // Registers a scoped dependency so each web request gets its own safe service instance.
            services.AddScoped<IPasswordHasher, Pbkdf2PasswordHasher>();
            // Registers a scoped dependency so each web request gets its own safe service instance.
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            // Registers the normal-user portal service for patient profile and self-appointment booking.
            services.AddScoped<IUserPortalService, UserPortalService>();
            // Registers a scoped dependency so each web request gets its own safe service instance.
            services.AddScoped<DbInitializer>();
            return services;
        }
    }
}
