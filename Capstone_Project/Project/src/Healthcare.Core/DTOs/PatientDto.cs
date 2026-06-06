// File: src/Healthcare.Core/DTOs/PatientDto.cs
// Layer: Core domain layer
// Purpose: This file is the data transfer object used to move validated data between controllers, services, and clients.
// Best-practice note: comments explain intent only; no business logic has been changed.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: DTO file: safely transfers only the required data between API/UI and business layers.

// Required namespaces are imported here so this file can use framework and project classes.
using System;
using System.ComponentModel.DataAnnotations;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Core.DTOs
{
    /// <summary>PatientDto belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class PatientDto
    {
        /// <summary>Primary key value used by EF Core and SQL Server to uniquely identify the record.</summary>
        public int Id { get; set; }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required, StringLength(100)]
        /// <summary>Stores the person's complete name and is validated before saving.</summary>
        public string FullName { get; set; }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required, EmailAddress, StringLength(150)]
        /// <summary>Stores the email address; validation and unique indexes prevent invalid or duplicate records.</summary>
        public string Email { get; set; }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required, RegularExpression(@"^[0-9]{10,15}$")]
        /// <summary>Stores a contact number and uses validation to reduce invalid input.</summary>
        public string PhoneNumber { get; set; }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required]
        /// <summary>Stores date of birth for patient demographics and care analytics.</summary>
        public DateTime DateOfBirth { get; set; }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required, StringLength(20)]
        /// <summary>Stores gender information used in patient registration and reports.</summary>
        public string Gender { get; set; }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required, StringLength(500)]
        /// <summary>Stores the patient address entered through the MVC form.</summary>
        public string Address { get; set; }

        // Limits maximum input length to protect data quality and database column size.
        [StringLength(100)]
        /// <summary>Stores optional blood group information for medical reference.</summary>
        public string BloodGroup { get; set; }
    }
}
