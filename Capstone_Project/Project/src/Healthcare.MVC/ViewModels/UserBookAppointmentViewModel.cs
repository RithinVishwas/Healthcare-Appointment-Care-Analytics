// File: src/Healthcare.MVC/ViewModels/UserBookAppointmentViewModel.cs
// Purpose: Allows a logged-in normal user to book an appointment for their own patient profile only.
// Security: PatientId is intentionally not exposed in this form to prevent booking for another patient.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Healthcare.MVC.ViewModels
{
    public class UserBookAppointmentViewModel
    {
        [Required, Display(Name = "Doctor")]
        public int DoctorId { get; set; }

        [Required, Display(Name = "Appointment Date & Time")]
        public DateTime AppointmentDateTime { get; set; }

        [Range(15, 180), Display(Name = "Duration Minutes")]
        public int DurationMinutes { get; set; }

        [Required, StringLength(500)]
        public string Reason { get; set; }

        // Dropdown list populated by the controller from the Doctors table.
        public IEnumerable<SelectListItem> Doctors { get; set; }
    }
}
