// File: src/Healthcare.Infrastructure/Repositories/UnitOfWork.cs
// Layer: Infrastructure/data-access layer
// Purpose: This file is the Unit of Work implementation that coordinates repository operations and database commits.
// Best-practice note: comments explain intent only; no business logic has been changed.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Repository file: isolates data-access queries from controllers and services.

// Required namespaces are imported here so this file can use framework and project classes.
using System.Threading;
using System.Threading.Tasks;
using Healthcare.Core.Entities;
// Required namespaces are imported here so this file can use framework and project classes.
using Healthcare.Core.Interfaces;
using Healthcare.Infrastructure.Data;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Infrastructure.Repositories
{
    /// <summary>UnitOfWork belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class UnitOfWork : IUnitOfWork
    {
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly HealthcareDbContext _context;

        ///  <summary>Constructor receives dependencies from ASP.NET Core dependency injection instead of creating them manually.</summary>
        public UnitOfWork(HealthcareDbContext context)
        {
            _context = context;
            Patients = new PatientRepository(context);
            Appointments = new AppointmentRepository(context);
            Doctors = new Repository<Doctor>(context);
            Departments = new Repository<Department>(context);
            Users = new Repository<AppUser>(context);
        }

        public IPatientRepository Patients { get; private set; }
        public IAppointmentRepository Appointments { get; private set; }
        public IRepository<Doctor> Doctors { get; private set; }
        public IRepository<Department> Departments { get; private set; }
        public IRepository<AppUser> Users { get; private set; }

        /// <summary>Saves pending EF Core changes and applies audit fields before committing.</summary>
        public Task<int> SaveChangesAsync(string performedBy)
        {
            return _context.SaveChangesAsync(performedBy, CancellationToken.None);
        }
    }
}
