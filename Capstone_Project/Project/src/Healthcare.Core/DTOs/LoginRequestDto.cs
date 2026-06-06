// File: src/Healthcare.Core/DTOs/LoginRequestDto.cs
// Layer: Core domain layer
// Purpose: This file is the data transfer object used to move validated data between controllers, services, and clients.
// Best-practice note: comments explain intent only; no business logic has been changed.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: DTO file: safely transfers only the required data between API/UI and business layers.

// Required namespaces are imported here so this file can use framework and project classes.
using System.ComponentModel.DataAnnotations;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Core.DTOs
{
    /// <summary>LoginRequestDto belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class LoginRequestDto
    {
        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required, EmailAddress]
        /// <summary>Stores the email address; validation and unique indexes prevent invalid or duplicate records.</summary>
        public string Email { get; set; }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required, MinLength(8)]
        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        public string Password { get; set; }
    }
}
