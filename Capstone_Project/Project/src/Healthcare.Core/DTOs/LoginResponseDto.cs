// File: src/Healthcare.Core/DTOs/LoginResponseDto.cs
// Layer: Core domain layer
// Purpose: This file is the data transfer object used to move validated data between controllers, services, and clients.
// Best-practice note: comments explain intent only; no business logic has been changed.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Core.DTOs
{
    /// <summary>LoginResponseDto belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class LoginResponseDto
    {
        /// <summary>Stores the JWT token returned to authenticated API clients.</summary>
        public string Token { get; set; }
        /// <summary>Stores the email address; validation and unique indexes prevent invalid or duplicate records.</summary>
        public string Email { get; set; }
        /// <summary>Stores the person's complete name and is validated before saving.</summary>
        public string FullName { get; set; }
        /// <summary>Stores the user role included in authentication/authorization responses.</summary>
        public string Role { get; set; }
        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        public int ExpiresInMinutes { get; set; }
    }
}
