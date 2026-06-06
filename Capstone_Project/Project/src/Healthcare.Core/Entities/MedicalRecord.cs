// File: src/Healthcare.Core/Entities/MedicalRecord.cs
// Layer: Core domain layer
// Purpose: This file is the domain entity class that represents a normalized business/database object used by EF Core and SQL Server.
// Best-practice note: comments explain intent only; no business logic has been changed.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Entity file: represents a normalized table/domain object used by EF Core and SQL Server.

// Required namespaces are imported here so this file can use framework and project classes.
using System;
using System.ComponentModel.DataAnnotations;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Core.Entities
{
    /// <summary>MedicalRecord belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class MedicalRecord : BaseEntity
    {
        /// <summary>Foreign key that connects the appointment/medical record to a patient.</summary>
        public int PatientId { get; set; }
        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        public Patient Patient { get; set; }

        /// <summary>Foreign key that links related medical records or audit logs to an appointment.</summary>
        public int? AppointmentId { get; set; }
        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        public Appointment Appointment { get; set; }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required, StringLength(1000)]
        /// <summary>Stores the diagnosis captured after consultation.</summary>
        public string Diagnosis { get; set; }

        // Limits maximum input length to protect data quality and database column size.
        [StringLength(1000)]
        /// <summary>Stores medicine or treatment instructions for the patient.</summary>
        public string Prescription { get; set; }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required]
        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        public DateTime VisitDate { get; set; }
    }
}
