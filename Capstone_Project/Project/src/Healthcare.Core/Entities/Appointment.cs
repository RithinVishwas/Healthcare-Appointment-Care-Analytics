// File: src/Healthcare.Core/Entities/Appointment.cs
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
// Required namespaces are imported here so this file can use framework and project classes.
using Healthcare.Core.Enums;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Core.Entities
{
    /// <summary>Appointment belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class Appointment : BaseEntity
    {
        ///  <summary>Constructor receives dependencies from ASP.NET Core dependency injection instead of creating them manually.</summary>
        public Appointment()
        {
            MedicalRecords = new List<MedicalRecord>();
            AuditLogs = new List<AuditLog>();
        }

        /// <summary>Foreign key that connects the appointment/medical record to a patient.</summary>
        public int PatientId { get; set; }
        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        public Patient Patient { get; set; }

        /// <summary>Foreign key that connects an appointment to the selected doctor.</summary>
        public int DoctorId { get; set; }
        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        public Doctor Doctor { get; set; }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required]
        /// <summary>Stores the scheduled date and time of the appointment.</summary>
        public DateTime AppointmentDateTime { get; set; }

        [Range(15, 180)]
        /// <summary>Stores appointment duration used for overlap and scheduling validation.</summary>
        public int DurationMinutes { get; set; }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required]
        /// <summary>Stores the appointment workflow state such as Scheduled, Completed, Cancelled, or NoShow.</summary>
        public AppointmentStatus Status { get; set; }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required, StringLength(500)]
        /// <summary>Stores the reason for the appointment entered by the user.</summary>
        public string Reason { get; set; }

        // Limits maximum input length to protect data quality and database column size.
        [StringLength(1000)]
        /// <summary>Stores additional appointment or medical notes.</summary>
        public string Notes { get; set; }

        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        /// <summary>Navigation collection for related MedicalRecord records; EF Core uses it to model one-to-many relationships.</summary>
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        /// <summary>Navigation collection for related AuditLog records; EF Core uses it to model one-to-many relationships.</summary>
        public ICollection<AuditLog> AuditLogs { get; set; }
    }
}
