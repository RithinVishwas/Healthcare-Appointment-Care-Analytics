// File: src/Healthcare.MVC/Controllers/DashboardController.cs
// Layer: MVC presentation layer
// Purpose: This file is the MVC controller that loads dashboard metrics and summary cards for the care analytics landing page.
// Security note: authentication, authorization, antiforgery, and validation are handled through ASP.NET Core middleware/attributes.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Controller file: receives HTTP/MVC requests, validates input, calls services/repositories, and returns views or API responses.

// Required namespaces are imported here so this file can use framework and project classes.
using System.Threading.Tasks;
using Healthcare.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
// Required namespaces are imported here so this file can use framework and project classes.
using Microsoft.AspNetCore.Mvc;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.MVC.Controllers
{
    // Restricts this action/controller to authenticated users or a specific role.
    [Authorize(Policy = "AdminOnly")]
    /// <summary>DashboardController belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class DashboardController : Controller
    {
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly IAnalyticsReportService _analyticsReportService;
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly IUnitOfWork _unitOfWork;

        ///  <summary>Constructor receives dependencies from ASP.NET Core dependency injection instead of creating them manually.</summary>
        public DashboardController(IAnalyticsReportService analyticsReportService, IUnitOfWork unitOfWork)
        {
            _analyticsReportService = analyticsReportService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>Loads the default listing or dashboard page for this controller.</summary>
        public async Task<IActionResult> Index()
        {
            ViewBag.Upcoming = await _unitOfWork.Appointments.GetUpcomingAsync();
            // Returns the Razor view that renders the HTML page for the browser.
            return View(await _analyticsReportService.GetSummaryAsync());
        }

        // Maps this action to a specific HTTP verb and route for REST/MVC routing.
        [HttpGet]
        /// <summary>Executes the SummaryJson workflow while keeping the logic inside the correct project layer.</summary>
        public async Task<IActionResult> SummaryJson()
        {
            return Json(await _analyticsReportService.GetSummaryAsync());
        }
    }
}
