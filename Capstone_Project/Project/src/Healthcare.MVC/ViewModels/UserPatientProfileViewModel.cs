// File: src/Healthcare.MVC/ViewModels/UserPatientProfileViewModel.cs
// Purpose: Carries normal-user patient profile data between the user portal UI and controller.
// Security: Validation attributes protect the database from incomplete or invalid form input.

using System;
using System.ComponentModel.DataAnnotations;

namespace Healthcare.MVC.ViewModels
{
    public class UserPatientProfileViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(100), Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; }

        [Required, Display(Name = "Phone Number"), RegularExpression(@"^[0-9]{10,15}$", ErrorMessage = "Enter a valid phone number with 10 to 15 digits.")]
        public string PhoneNumber { get; set; }

        [Required, DataType(DataType.Date), Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required, StringLength(20)]
        public string Gender { get; set; }

        [Required, StringLength(500)]
        public string Address { get; set; }

        [Display(Name = "Blood Group"), StringLength(100)]
        public string BloodGroup { get; set; }
    }
}
