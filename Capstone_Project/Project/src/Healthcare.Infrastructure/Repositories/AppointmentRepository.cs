// File: src/Healthcare.Infrastructure/Repositories/AppointmentRepository.cs
// Layer: Infrastructure/data-access layer
// Purpose: This file is the appointment-specific repository for appointment queries and scheduling-related data retrieval.
// Security note: data access should use EF Core or parameterized SQL to avoid SQL injection.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Repository file: isolates data-access queries from controllers and services.

// Required namespaces are imported here so this file can use framework and project classes.
using System;
using System.Collections.Generic;
using System.Linq;
// Required namespaces are imported here so this file can use framework and project classes.
using System.Threading.Tasks;
using Healthcare.Core.Entities;
using Healthcare.Core.Enums;
// Required namespaces are imported here so this file can use framework and project classes.
using Healthcare.Core.Interfaces;
using Healthcare.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Infrastructure.Repositories
{
    /// <summary>AppointmentRepository belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        ///  <summary>Constructor receives dependencies from ASP.NET Core dependency injection instead of creating them manually.</summary>
        public AppointmentRepository(HealthcareDbContext context) : base(context)
        {
        }

        /// <summary>Executes the GetAllAsync workflow while keeping the logic inside the correct project layer.</summary>
        public override async Task<IReadOnlyList<Appointment>> GetAllAsync()
        {
            return await Context.Appointments
                // Include eagerly loads related data to avoid missing navigation details in the response/view.
                .Include(x => x.Patient)
                // Include eagerly loads related data to avoid missing navigation details in the response/view.
                .Include(x => x.Doctor).ThenInclude(x => x.Department)
                .AsNoTracking()
                .OrderByDescending(x => x.AppointmentDateTime)
                // ToListAsync executes the query asynchronously and materializes the result for display/API output.
                .ToListAsync();
        }

        /// <summary>Executes the GetByIdAsync workflow while keeping the logic inside the correct project layer.</summary>
        public override async Task<Appointment> GetByIdAsync(int id)
        {
            return await Context.Appointments
                // Include eagerly loads related data to avoid missing navigation details in the response/view.
                .Include(x => x.Patient)
                // Include eagerly loads related data to avoid missing navigation details in the response/view.
                .Include(x => x.Doctor).ThenInclude(x => x.Department)
                // FirstOrDefaultAsync safely returns null when no matching row is found, so the code can handle missing records.
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>Executes the GetUpcomingAsync workflow while keeping the logic inside the correct project layer.</summary>
        public async Task<IReadOnlyList<Appointment>> GetUpcomingAsync()
        {
            return await Context.Appointments
                // Include eagerly loads related data to avoid missing navigation details in the response/view.
                .Include(x => x.Patient)
                // Include eagerly loads related data to avoid missing navigation details in the response/view.
                .Include(x => x.Doctor)
                .Where(x => x.AppointmentDateTime >= DateTime.Now && x.Status == AppointmentStatus.Scheduled)
                .OrderBy(x => x.AppointmentDateTime)
                .AsNoTracking()
                // ToListAsync executes the query asynchronously and materializes the result for display/API output.
                .ToListAsync();
        }

        /// <summary>Executes the HasDoctorConflictAsync workflow while keeping the logic inside the correct project layer.</summary>
        public Task<bool> HasDoctorConflictAsync(int doctorId, DateTime start, int durationMinutes, int? ignoredAppointmentId)
        {
            var end = start.AddMinutes(durationMinutes);
            // AnyAsync checks existence efficiently in SQL Server without loading full rows into memory.
            return Context.Appointments.AnyAsync(x =>
                x.DoctorId == doctorId &&
                x.Status == AppointmentStatus.Scheduled &&
                (!ignoredAppointmentId.HasValue || x.Id != ignoredAppointmentId.Value) &&
                start < x.AppointmentDateTime.AddMinutes(x.DurationMinutes) &&
                end > x.AppointmentDateTime);
        }

        /// <summary>Executes the HasPatientConflictAsync workflow while keeping the logic inside the correct project layer.</summary>
        public Task<bool> HasPatientConflictAsync(int patientId, DateTime start, int durationMinutes, int? ignoredAppointmentId)
        {
            var end = start.AddMinutes(durationMinutes);
            // AnyAsync checks existence efficiently in SQL Server without loading full rows into memory.
            return Context.Appointments.AnyAsync(x =>
                x.PatientId == patientId &&
                x.Status == AppointmentStatus.Scheduled &&
                (!ignoredAppointmentId.HasValue || x.Id != ignoredAppointmentId.Value) &&
                start < x.AppointmentDateTime.AddMinutes(x.DurationMinutes) &&
                end > x.AppointmentDateTime);
        }
    }
}
