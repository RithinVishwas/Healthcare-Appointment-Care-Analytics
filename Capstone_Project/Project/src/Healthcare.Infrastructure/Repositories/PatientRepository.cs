// File: src/Healthcare.Infrastructure/Repositories/PatientRepository.cs
// Layer: Infrastructure/data-access layer
// Purpose: This file is the patient-specific repository for data queries that are more specific than generic CRUD operations.
// Security note: data access should use EF Core or parameterized SQL to avoid SQL injection.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Repository file: isolates data-access queries from controllers and services.

// Required namespaces are imported here so this file can use framework and project classes.
using System.Linq;
using System.Threading.Tasks;
using Healthcare.Core.Entities;
// Required namespaces are imported here so this file can use framework and project classes.
using Healthcare.Core.Interfaces;
using Healthcare.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Infrastructure.Repositories
{
    /// <summary>PatientRepository belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        ///  <summary>Constructor receives dependencies from ASP.NET Core dependency injection instead of creating them manually.</summary>
        public PatientRepository(HealthcareDbContext context) : base(context)
        {
        }

        /// <summary>Executes the GetAllAsync workflow while keeping the logic inside the correct project layer.</summary>
        public override async Task<System.Collections.Generic.IReadOnlyList<Patient>> GetAllAsync()
        {
            // ToListAsync executes the query asynchronously and materializes the result for display/API output.
            return await Context.Patients.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.FullName).ToListAsync();
        }

        /// <summary>Executes the EmailExistsAsync workflow while keeping the logic inside the correct project layer.</summary>
        public async Task<bool> EmailExistsAsync(string email, int? ignoredPatientId)
        {
            // AnyAsync checks existence efficiently in SQL Server without loading full rows into memory.
            return await Context.Patients.AnyAsync(x => x.Email == email && (!ignoredPatientId.HasValue || x.Id != ignoredPatientId.Value));
        }
    }
}
