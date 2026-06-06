// File: src/Healthcare.MVC/ViewModels/AppointmentViewModel.cs
// Layer: MVC presentation layer
// Purpose: This file is the MVC view model that carries validated form/display data between Razor views and controllers.
// Security note: MVC validation and antiforgery tokens help protect forms from invalid input and CSRF attacks.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: ViewModel file: shapes form/display data for Razor pages and applies UI validation attributes.

// Required namespaces are imported here so this file can use framework and project classes.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
// Required namespaces are imported here so this file can use framework and project classes.
using Healthcare.Core.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.MVC.ViewModels
{
    /// <summary>AppointmentViewModel belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class AppointmentViewModel
    {
        /// <summary>Primary key value used by EF Core and SQL Server to uniquely identify the record.</summary>
        public int Id { get; set; }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required, Display(Name = "Patient")]
        /// <summary>Foreign key that connects the appointment/medical record to a patient.</summary>
        public int PatientId { get; set; }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required, Display(Name = "Doctor")]
        /// <summary>Foreign key that connects an appointment to the selected doctor.</summary>
        public int DoctorId { get; set; }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required, Display(Name = "Appointment Date & Time")]
        /// <summary>Stores the scheduled date and time of the appointment.</summary>
        public DateTime AppointmentDateTime { get; set; }

        [Range(15, 180), Display(Name = "Duration Minutes")]
        /// <summary>Stores appointment duration used for overlap and scheduling validation.</summary>
        public int DurationMinutes { get; set; }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required, StringLength(500)]
        /// <summary>Stores the reason for the appointment entered by the user.</summary>
        public string Reason { get; set; }

        /// <summary>Stores the appointment workflow state such as Scheduled, Completed, Cancelled, or NoShow.</summary>
        public AppointmentStatus Status { get; set; }
        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        public IEnumerable<SelectListItem> Patients { get; set; }
        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        public IEnumerable<SelectListItem> Doctors { get; set; }
    }
}
