// File: src/Healthcare.Core/Entities/AppUser.cs
// Layer: Core domain layer
// Purpose: This file is the domain entity class that represents a normalized business/database object used by EF Core and SQL Server.
// Best-practice note: comments explain intent only; no business logic has been changed.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Entity file: represents a normalized table/domain object used by EF Core and SQL Server.

// Required namespaces are imported here so this file can use framework and project classes.
using System.ComponentModel.DataAnnotations;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Core.Entities
{
    /// <summary>AppUser belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class AppUser : BaseEntity
    {
        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required, StringLength(100)]
        /// <summary>Stores the person's complete name and is validated before saving.</summary>
        public string FullName { get; set; }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required, EmailAddress, StringLength(150)]
        /// <summary>Stores the email address; validation and unique indexes prevent invalid or duplicate records.</summary>
        public string Email { get; set; }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required, StringLength(256)]
        /// <summary>Stores only the hashed password; raw passwords should never be stored.</summary>
        public string PasswordHash { get; set; }

        /// <summary>Foreign key that assigns authorization permissions through the role table.</summary>
        public int RoleId { get; set; }

        /// <summary>Stores the user role included in authentication/authorization responses.</summary>
        public AppRole Role { get; set; }

        // PatientId is nullable because Admin/Doctor/Staff login accounts do not need a patient profile.
        // Only normal portal users will be linked to a Patient record after they create their profile.
        public int? PatientId { get; set; }

        // Navigation property used by EF Core to connect a normal User account to one Patient profile.
        // This allows the user portal to show only the logged-in user's profile and appointments.
        public Patient Patient { get; set; }
    }
}
