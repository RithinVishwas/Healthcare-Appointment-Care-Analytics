// File: src/Healthcare.Core/Interfaces/IPasswordHasher.cs
// Layer: Core domain layer
// Purpose: This file is the interface abstraction used to support Dependency Inversion, testability, and SOLID-compliant design.
// Security note: credentials and tokens are handled through hashing and JWT-based authentication practices.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Core.Interfaces
{
    /// <summary>Defines the contract for IPasswordHasher; controllers/services depend on this abstraction instead of a concrete class.</summary>
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string storedHash);
    }
}
