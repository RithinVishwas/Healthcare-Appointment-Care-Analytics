// File: src/Healthcare.Infrastructure/Services/SqlAnalyticsReportService.cs
// Layer: Infrastructure/data-access layer
// Purpose: This file is the ADO.NET reporting service used to read analytical reports from SQL Server using parameterized and safe data access patterns.
// Security note: data access should use EF Core or parameterized SQL to avoid SQL injection.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Service file: contains reusable business/security logic separated from UI and controllers.

// Required namespaces are imported here so this file can use framework and project classes.
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
// Required namespaces are imported here so this file can use framework and project classes.
using Healthcare.Core.DTOs;
using Healthcare.Core.Interfaces;
using Microsoft.Data.SqlClient;
// Required namespaces are imported here so this file can use framework and project classes.
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Healthcare.Infrastructure.Data;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Infrastructure.Services
{
    /// <summary>SqlAnalyticsReportService belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class SqlAnalyticsReportService : IAnalyticsReportService
    {
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly HealthcareDbContext _context;
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly string _connectionString;

        ///  <summary>Constructor receives dependencies from ASP.NET Core dependency injection instead of creating them manually.</summary>
        public SqlAnalyticsReportService(HealthcareDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>Executes the GetSummaryAsync workflow while keeping the logic inside the correct project layer.</summary>
        public async Task<AnalyticsSummaryDto> GetSummaryAsync()
        {
            return new AnalyticsSummaryDto
            {
                TotalPatients = await _context.Patients.CountAsync(x => x.IsActive),
                TotalDoctors = await _context.Doctors.CountAsync(x => x.IsActive),
                TotalAppointments = await _context.Appointments.CountAsync(),
                ScheduledAppointments = await _context.Appointments.CountAsync(x => x.Status == Core.Enums.AppointmentStatus.Scheduled),
                CompletedAppointments = await _context.Appointments.CountAsync(x => x.Status == Core.Enums.AppointmentStatus.Completed),
                CancelledAppointments = await _context.Appointments.CountAsync(x => x.Status == Core.Enums.AppointmentStatus.Cancelled)
            };
        }

        /// <summary>Executes the GetDepartmentAppointmentReportAsync workflow while keeping the logic inside the correct project layer.</summary>
        public async Task<IReadOnlyList<DepartmentAppointmentReportDto>> GetDepartmentAppointmentReportAsync()
        {
            var reports = new List<DepartmentAppointmentReportDto>();

            // Required namespaces are imported here so this file can use framework and project classes.
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("usp_GetDepartmentAppointmentReport", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                // Parameterized SQL protects this database operation from SQL injection attacks.
                // No user input is concatenated into SQL. Parameters should be added with SqlParameter when needed.
                await connection.OpenAsync();
                // Required namespaces are imported here so this file can use framework and project classes.
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        reports.Add(new DepartmentAppointmentReportDto
                        {
                            DepartmentName = reader["DepartmentName"].ToString(),
                            TotalAppointments = (int)reader["TotalAppointments"],
                            CompletedAppointments = (int)reader["CompletedAppointments"],
                            ScheduledAppointments = (int)reader["ScheduledAppointments"]
                        });
                    }
                }
            }

            return reports;
        }
    }
}
