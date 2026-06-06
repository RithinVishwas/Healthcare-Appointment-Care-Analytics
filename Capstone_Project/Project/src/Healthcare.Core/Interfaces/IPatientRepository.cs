// File: src/Healthcare.Core/Interfaces/IPatientRepository.cs
// Layer: Core domain layer
// Purpose: This file is the interface abstraction used to support Dependency Inversion, testability, and SOLID-compliant design.
// Security note: data access should use EF Core or parameterized SQL to avoid SQL injection.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Interface file: defines contracts so the project follows Dependency Inversion and SOLID design.

// Required namespaces are imported here so this file can use framework and project classes.
using System.Threading.Tasks;
using Healthcare.Core.Entities;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Core.Interfaces
{
    /// <summary>Defines the contract for IPatientRepository; controllers/services depend on this abstraction instead of a concrete class.</summary>
    public interface IPatientRepository : IRepository<Patient>
    {
        Task<bool> EmailExistsAsync(string email, int? ignoredPatientId);
    }
}
