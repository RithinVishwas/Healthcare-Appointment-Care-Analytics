// File: src/Healthcare.MVC/Controllers/HomeController.cs
// Layer: MVC presentation layer
// Purpose: This file is the MVC controller for common home and error pages.
// Security note: authentication, authorization, antiforgery, and validation are handled through ASP.NET Core middleware/attributes.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Controller file: receives HTTP/MVC requests, validates input, calls services/repositories, and returns views or API responses.

// Required namespaces are imported here so this file can use framework and project classes.
using Microsoft.AspNetCore.Mvc;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.MVC.Controllers
{
    /// <summary>HomeController belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class HomeController : Controller
    {
        /// <summary>Executes the Error workflow while keeping the logic inside the correct project layer.</summary>
        public IActionResult Error()
        {
            // Returns the Razor view that renders the HTML page for the browser.
            return View();
        }
    }
}
