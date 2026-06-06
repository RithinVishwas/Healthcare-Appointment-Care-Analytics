// File: src/Healthcare.MVC/ViewModels/PatientViewModel.cs
// Layer: MVC presentation layer
// Purpose: This file is the MVC view model that carries validated form/display data between Razor views and controllers.
// Security note: MVC validation and antiforgery tokens help protect forms from invalid input and CSRF attacks.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: ViewModel file: shapes form/display data for Razor pages and applies UI validation attributes.

// Required namespaces are imported here so this file can use framework and project classes.
using System;
using System.ComponentModel.DataAnnotations;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.MVC.ViewModels
{
    /// <summary>PatientViewModel belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class PatientViewModel
    {
        /// <summary>Primary key value used by EF Core and SQL Server to uniquely identify the record.</summary>
        public int Id { get; set; }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required, Display(Name = "Full Name"), StringLength(100)]
        /// <summary>Stores the person's complete name and is validated before saving.</summary>
        public string FullName { get; set; }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required, EmailAddress, StringLength(150)]
        /// <summary>Stores the email address; validation and unique indexes prevent invalid or duplicate records.</summary>
        public string Email { get; set; }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required, Display(Name = "Phone Number"), RegularExpression(@"^[0-9]{10,15}$", ErrorMessage = "Enter a valid phone number.")]
        /// <summary>Stores a contact number and uses validation to reduce invalid input.</summary>
        public string PhoneNumber { get; set; }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required, DataType(DataType.Date), Display(Name = "Date of Birth")]
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

        [Display(Name = "Blood Group"), StringLength(100)]
        /// <summary>Stores optional blood group information for medical reference.</summary>
        public string BloodGroup { get; set; }
    }
}
