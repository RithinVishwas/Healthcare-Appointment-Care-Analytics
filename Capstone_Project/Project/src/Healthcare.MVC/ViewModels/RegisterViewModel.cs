// File: src/Healthcare.MVC/ViewModels/RegisterViewModel.cs
// Purpose: Carries registration form data from the Razor page to AccountController.
// Security: DataAnnotations validate user input before any database insert happens.

using System.ComponentModel.DataAnnotations;

namespace Healthcare.MVC.ViewModels
{
    public class RegisterViewModel
    {
        [Required, StringLength(100), Display(Name = "Full Name")]
        // FullName is stored in AppUsers and shown in the logged-in user card.
        public string FullName { get; set; }

        [Required, EmailAddress, StringLength(150)]
        // Email is used as the username for login and is checked for duplicates before registration.
        public string Email { get; set; }

        [Required, DataType(DataType.Password), MinLength(8)]
        // Password is never stored directly; AccountController hashes it using PBKDF2.
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Compare("Password", ErrorMessage = "Password and confirm password do not match.")]
        // ConfirmPassword prevents accidental typing mistakes during registration.
        public string ConfirmPassword { get; set; }
    }
}
