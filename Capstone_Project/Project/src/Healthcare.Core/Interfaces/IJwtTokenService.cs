// File: src/Healthcare.Core/Interfaces/IJwtTokenService.cs
// Layer: Core domain layer
// Purpose: This file is the interface abstraction used to support Dependency Inversion, testability, and SOLID-compliant design.
// Security note: credentials and tokens are handled through hashing and JWT-based authentication practices.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Interface file: defines contracts so the project follows Dependency Inversion and SOLID design.

// Required namespaces are imported here so this file can use framework and project classes.
using Healthcare.Core.DTOs;
using Healthcare.Core.Entities;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Core.Interfaces
{
    /// <summary>Defines the contract for IJwtTokenService; controllers/services depend on this abstraction instead of a concrete class.</summary>
    public interface IJwtTokenService
    {
        LoginResponseDto CreateToken(AppUser user);
    }
}
