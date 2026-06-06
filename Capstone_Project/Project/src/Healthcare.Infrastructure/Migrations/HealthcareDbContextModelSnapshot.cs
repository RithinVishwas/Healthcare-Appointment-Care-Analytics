// File: src/Healthcare.Infrastructure/Migrations/HealthcareDbContextModelSnapshot.cs
// Layer: Infrastructure/data-access layer
// Purpose: This file is the EF Core migration file that describes database schema creation or schema snapshot metadata.
// Security note: data access should use EF Core or parameterized SQL to avoid SQL injection.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Migration file: describes database schema changes generated for EF Core.

// Required namespaces are imported here so this file can use framework and project classes.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Healthcare.Infrastructure.Data;

#nullable disable

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Infrastructure.Migrations
{
    [DbContext(typeof(HealthcareDbContext))]
    /// <summary>HealthcareDbContextModelSnapshot belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class HealthcareDbContextModelSnapshot : ModelSnapshot
    {
        /// <summary>Executes the BuildModel workflow while keeping the logic inside the correct project layer.</summary>
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "8.0.5");
        }
    }
}
