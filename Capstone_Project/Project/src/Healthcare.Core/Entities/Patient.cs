// File: src/Healthcare.Core/Entities/Patient.cs
// Layer: Core domain layer
// Purpose: This file is the domain entity class that represents a normalized business/database object used by EF Core and SQL Server.
// Best-practice note: comments explain intent only; no business logic has been changed.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Entity file: represents a normalized table/domain object used by EF Core and SQL Server.

// Required namespaces are imported here so this file can use framework and project classes.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Core.Entities
{
    /// <summary>Patient belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class Patient : BaseEntity
    {
        ///  <summary>Constructor receives dependencies from ASP.NET Core dependency injection instead of creating them manually.</summary>
        public Patient()
        {
            Appointments = new List<Appointment>();
            MedicalRecords = new List<MedicalRecord>();
        }

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
        [Required, DataType(DataType.Date)]
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

        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        /// <summary>Navigation collection for related Appointment records; EF Core uses it to model one-to-many relationships.</summary>
        public ICollection<Appointment> Appointments { get; set; }
        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        /// <summary>Navigation collection for related MedicalRecord records; EF Core uses it to model one-to-many relationships.</summary>
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}
