// File: src/Healthcare.MVC/ViewModels/LoginViewModel.cs
// Layer: MVC presentation layer
// Purpose: This file is the MVC view model that carries validated form/display data between Razor views and controllers.
// Security note: MVC validation and antiforgery tokens help protect forms from invalid input and CSRF attacks.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: ViewModel file: shapes form/display data for Razor pages and applies UI validation attributes.

// Required namespaces are imported here so this file can use framework and project classes.
using System.ComponentModel.DataAnnotations;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.MVC.ViewModels
{
    /// <summary>LoginViewModel belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class LoginViewModel
    {
        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required, EmailAddress]
        /// <summary>Stores the email address; validation and unique indexes prevent invalid or duplicate records.</summary>
        public string Email { get; set; }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required, DataType(DataType.Password), MinLength(8)]
        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        public string Password { get; set; }
    }
}
