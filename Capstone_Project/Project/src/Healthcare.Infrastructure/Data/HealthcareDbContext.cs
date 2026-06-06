// File: src/Healthcare.Infrastructure/Data/HealthcareDbContext.cs
// Layer: Infrastructure/data-access layer
// Purpose: This file is the EF Core database context that maps domain entities to SQL Server tables and configures relationships, constraints, and seed behavior.
// Security note: data access should use EF Core or parameterized SQL to avoid SQL injection.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: C# source file: part of the layered healthcare appointment system.

// Required namespaces are imported here so this file can use framework and project classes.
using System;
using System.Linq;
using System.Threading;
// Required namespaces are imported here so this file can use framework and project classes.
using System.Threading.Tasks;
using Healthcare.Core.Entities;
using Healthcare.Core.Enums;
// Required namespaces are imported here so this file can use framework and project classes.
using Microsoft.EntityFrameworkCore;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Infrastructure.Data
{
    /// <summary>HealthcareDbContext belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class HealthcareDbContext : DbContext
    {
        ///  <summary>Constructor receives dependencies from ASP.NET Core dependency injection instead of creating them manually.</summary>
        public HealthcareDbContext(DbContextOptions<HealthcareDbContext> options) : base(options)
        {
        }

        // EF Core DbSet maps this entity to a SQL Server table and enables LINQ queries.
        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        public DbSet<Patient> Patients { get; set; }
        // EF Core DbSet maps this entity to a SQL Server table and enables LINQ queries.
        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        public DbSet<Doctor> Doctors { get; set; }
        // EF Core DbSet maps this entity to a SQL Server table and enables LINQ queries.
        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        public DbSet<Department> Departments { get; set; }
        // EF Core DbSet maps this entity to a SQL Server table and enables LINQ queries.
        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        public DbSet<Appointment> Appointments { get; set; }
        // EF Core DbSet maps this entity to a SQL Server table and enables LINQ queries.
        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        // EF Core DbSet maps this entity to a SQL Server table and enables LINQ queries.
        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        public DbSet<AppUser> Users { get; set; }
        // EF Core DbSet maps this entity to a SQL Server table and enables LINQ queries.
        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        public DbSet<AppRole> Roles { get; set; }
        // EF Core DbSet maps this entity to a SQL Server table and enables LINQ queries.
        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        public DbSet<AuditLog> AuditLogs { get; set; }

        /// <summary>Configures entity relationships, constraints, indexes, and seed data for EF Core.</summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ------------------------------------------------------------
            // TABLE NAME MAPPING
            // ------------------------------------------------------------
            // EF Core uses DbSet names as table names by default.
            // Our SQL scripts use dbo.AppUsers and dbo.AppRoles, so we map them explicitly.
            // This prevents runtime errors such as "Invalid object name 'Users'".
            modelBuilder.Entity<AppUser>().ToTable("AppUsers");
            modelBuilder.Entity<AppRole>().ToTable("AppRoles");

            // ------------------------------------------------------------
            // TRIGGER-AWARE APPOINTMENT TABLE MAPPING
            // ------------------------------------------------------------
            // The Appointments table has insert/update triggers for audit logging.
            // EF Core 8 normally uses SQL Server OUTPUT clause during SaveChanges.
            // SQL Server does not allow OUTPUT without INTO on tables that have enabled triggers.
            // This mapping tells EF Core that the table has triggers and disables the incompatible OUTPUT clause.
            modelBuilder.Entity<Appointment>()
                .ToTable("Appointments", tableBuilder =>
                {
                    tableBuilder.HasTrigger("trg_Appointments_Audit_Insert");
                    tableBuilder.HasTrigger("trg_Appointments_Audit_Update");
                    tableBuilder.UseSqlOutputClause(false);
                });

            // ------------------------------------------------------------
            // USER ACCOUNT - PATIENT PROFILE RELATIONSHIP
            // ------------------------------------------------------------
            // One normal User account can be linked to one Patient profile.
            // PatientId is nullable because Admin/Doctor/Staff accounts do not need patient profiles.
            // SetNull keeps login data safe even if a patient profile is removed by admin.
            modelBuilder.Entity<AppUser>()
                .HasOne(x => x.Patient)
                .WithMany()
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configures EF Core mapping rules for the selected entity.
            modelBuilder.Entity<Patient>().HasIndex(x => x.Email).IsUnique();
            // Configures EF Core mapping rules for the selected entity.
            modelBuilder.Entity<Doctor>().HasIndex(x => x.Email).IsUnique();
            // Configures EF Core mapping rules for the selected entity.
            modelBuilder.Entity<AppUser>().HasIndex(x => x.Email).IsUnique();
            // Configures EF Core mapping rules for the selected entity.
            modelBuilder.Entity<AppRole>().HasIndex(x => x.Name).IsUnique();

            // Configures EF Core mapping rules for the selected entity.
            modelBuilder.Entity<Doctor>()
                // Defines relationship behavior between normalized database tables.
                .HasOne(x => x.Department)
                // Defines relationship behavior between normalized database tables.
                .WithMany(x => x.Doctors)
                // Defines relationship behavior between normalized database tables.
                .HasForeignKey(x => x.DepartmentId)
                // Defines relationship behavior between normalized database tables.
                .OnDelete(DeleteBehavior.Restrict);

            // Configures EF Core mapping rules for the selected entity.
            modelBuilder.Entity<Appointment>()
                // Defines relationship behavior between normalized database tables.
                .HasOne(x => x.Patient)
                // Defines relationship behavior between normalized database tables.
                .WithMany(x => x.Appointments)
                // Defines relationship behavior between normalized database tables.
                .HasForeignKey(x => x.PatientId)
                // Defines relationship behavior between normalized database tables.
                .OnDelete(DeleteBehavior.Restrict);

            // Configures EF Core mapping rules for the selected entity.
            modelBuilder.Entity<Appointment>()
                // Defines relationship behavior between normalized database tables.
                .HasOne(x => x.Doctor)
                // Defines relationship behavior between normalized database tables.
                .WithMany(x => x.Appointments)
                // Defines relationship behavior between normalized database tables.
                .HasForeignKey(x => x.DoctorId)
                // Defines relationship behavior between normalized database tables.
                .OnDelete(DeleteBehavior.Restrict);

            // Configures EF Core mapping rules for the selected entity.
            modelBuilder.Entity<MedicalRecord>()
                // Defines relationship behavior between normalized database tables.
                .HasOne(x => x.Patient)
                // Defines relationship behavior between normalized database tables.
                .WithMany(x => x.MedicalRecords)
                // Defines relationship behavior between normalized database tables.
                .HasForeignKey(x => x.PatientId)
                // Defines relationship behavior between normalized database tables.
                .OnDelete(DeleteBehavior.Restrict);

            // Configures EF Core mapping rules for the selected entity.
            modelBuilder.Entity<MedicalRecord>()
                // Defines relationship behavior between normalized database tables.
                .HasOne(x => x.Appointment)
                // Defines relationship behavior between normalized database tables.
                .WithMany(x => x.MedicalRecords)
                // Defines relationship behavior between normalized database tables.
                .HasForeignKey(x => x.AppointmentId)
                // Defines relationship behavior between normalized database tables.
                .OnDelete(DeleteBehavior.SetNull);

            // Configures EF Core mapping rules for the selected entity.
            modelBuilder.Entity<AuditLog>()
                // Defines relationship behavior between normalized database tables.
                .HasOne(x => x.Appointment)
                // Defines relationship behavior between normalized database tables.
                .WithMany(x => x.AuditLogs)
                // Defines relationship behavior between normalized database tables.
                .HasForeignKey(x => x.AppointmentId)
                // Defines relationship behavior between normalized database tables.
                .OnDelete(DeleteBehavior.SetNull);

            // Calls seed data configuration so demo records are available after migration.
            Seed(modelBuilder);
        }

        /// <summary>Saves pending EF Core changes and applies audit fields before committing.</summary>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ApplyAuditFields("system");
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>Saves pending EF Core changes and applies audit fields before committing.</summary>
        public Task<int> SaveChangesAsync(string performedBy, CancellationToken cancellationToken)
        {
            ApplyAuditFields(performedBy);
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>Automatically fills audit fields such as created and updated timestamps.</summary>
        private void ApplyAuditFields(string performedBy)
        {
            var now = DateTime.UtcNow;
            // Loops through tracked entities to apply audit values before saving.
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                // Detects newly inserted records and fills creation audit values.
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAtUtc = now;
                    entry.Entity.IsActive = true;
                }

                // Detects updated records and refreshes update audit values.
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAtUtc = now;
                }
            }
        }

        /// <summary>Adds initial sample data so the project can run immediately after setup.</summary>
        private static void Seed(ModelBuilder modelBuilder)
        {
            var created = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            // Configures EF Core mapping rules for the selected entity.
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "General Medicine", Description = "Primary care and consultation", CreatedAtUtc = created, IsActive = true },
                new Department { Id = 2, Name = "Cardiology", Description = "Heart care and related treatment", CreatedAtUtc = created, IsActive = true },
                new Department { Id = 3, Name = "Orthopedics", Description = "Bone, joint, and mobility care", CreatedAtUtc = created, IsActive = true }
            );

            // Configures EF Core mapping rules for the selected entity.
            modelBuilder.Entity<Doctor>().HasData(
                new Doctor { Id = 1, FullName = "Dr. Ananya Rao", Email = "ananya.rao@healthcare.local", PhoneNumber = "9876543210", Specialization = "General Physician", DepartmentId = 1, CreatedAtUtc = created, IsActive = true },
                new Doctor { Id = 2, FullName = "Dr. Karthik Menon", Email = "karthik.menon@healthcare.local", PhoneNumber = "9876543211", Specialization = "Cardiologist", DepartmentId = 2, CreatedAtUtc = created, IsActive = true },
                new Doctor { Id = 3, FullName = "Dr. Priya Shah", Email = "priya.shah@healthcare.local", PhoneNumber = "9876543212", Specialization = "Orthopedic Surgeon", DepartmentId = 3, CreatedAtUtc = created, IsActive = true }
            );

            // Configures EF Core mapping rules for the selected entity.
            modelBuilder.Entity<Patient>().HasData(
                new Patient { Id = 1, FullName = "Arun Kumar", Email = "arun@example.com", PhoneNumber = "9000000001", DateOfBirth = new DateTime(1997, 5, 10), Gender = "Male", Address = "Madurai", BloodGroup = "O+", CreatedAtUtc = created, IsActive = true },
                new Patient { Id = 2, FullName = "Meera Nair", Email = "meera@example.com", PhoneNumber = "9000000002", DateOfBirth = new DateTime(1995, 8, 21), Gender = "Female", Address = "Chennai", BloodGroup = "A+", CreatedAtUtc = created, IsActive = true }
            );

            // Configures EF Core mapping rules for the selected entity.
            modelBuilder.Entity<AppRole>().HasData(
                new AppRole { Id = 1, Name = "Admin", CreatedAtUtc = created, IsActive = true },
                new AppRole { Id = 2, Name = "Doctor", CreatedAtUtc = created, IsActive = true },
                new AppRole { Id = 3, Name = "Staff", CreatedAtUtc = created, IsActive = true },
                // User role is used for normal patients who register through the MVC portal.
                new AppRole { Id = 4, Name = "User", CreatedAtUtc = created, IsActive = true }
            );

            // Configures EF Core mapping rules for the selected entity.
            modelBuilder.Entity<AppUser>().HasData(
                // Password handling uses hashed values; this protects the application if database rows are exposed.
                new AppUser { Id = 1, FullName = "System Administrator", Email = "admin@healthcare.local", PasswordHash = "10000:DemoSaltOnlyForSeed:DemoHashChangeUsingDbInitializer", RoleId = 1, CreatedAtUtc = created, IsActive = true }
            );

            // Configures EF Core mapping rules for the selected entity.
            modelBuilder.Entity<Appointment>().HasData(
                new Appointment { Id = 1, PatientId = 1, DoctorId = 1, AppointmentDateTime = new DateTime(2026, 7, 10, 10, 0, 0), DurationMinutes = 30, Status = AppointmentStatus.Scheduled, Reason = "Fever and headache", Notes = "Initial consultation", CreatedAtUtc = created, IsActive = true },
                new Appointment { Id = 2, PatientId = 2, DoctorId = 2, AppointmentDateTime = new DateTime(2026, 7, 11, 11, 0, 0), DurationMinutes = 45, Status = AppointmentStatus.Completed, Reason = "Routine heart checkup", Notes = "ECG recommended", CreatedAtUtc = created, IsActive = true }
            );
        }
    }
}
