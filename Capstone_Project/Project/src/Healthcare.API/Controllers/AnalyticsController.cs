// File: src/Healthcare.API/Controllers/AnalyticsController.cs
// Layer: Web API layer
// Purpose: This file is the Web API controller that exposes care analytics and reporting endpoints.
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
namespace Healthcare.API.Controllers
{
    // Marks this class as a Web API controller and enables automatic model validation behavior.
    [ApiController]
    // Defines the route pattern used to reach this controller or action.
    [Route("api/[controller]")]
    // Restricts this action/controller to authenticated users or a specific role.
    [Authorize(Policy = "ClinicalStaff")]
    /// <summary>AnalyticsController belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class AnalyticsController : ControllerBase
    {
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly IAnalyticsReportService _analyticsReportService;

        ///  <summary>Constructor receives dependencies from ASP.NET Core dependency injection instead of creating them manually.</summary>
        public AnalyticsController(IAnalyticsReportService analyticsReportService)
        {
            _analyticsReportService = analyticsReportService;
        }

        // Maps this action to a specific HTTP verb and route for REST/MVC routing.
        [HttpGet("summary")]
        /// <summary>Executes the Summary workflow while keeping the logic inside the correct project layer.</summary>
        public async Task<IActionResult> Summary()
        {
            // Returns a successful API response with the requested data.
            return Ok(await _analyticsReportService.GetSummaryAsync());
        }

        // Maps this action to a specific HTTP verb and route for REST/MVC routing.
        [HttpGet("department-appointments")]
        /// <summary>Executes the DepartmentAppointments workflow while keeping the logic inside the correct project layer.</summary>
        public async Task<IActionResult> DepartmentAppointments()
        {
            // Returns a successful API response with the requested data.
            return Ok(await _analyticsReportService.GetDepartmentAppointmentReportAsync());
        }
    }
}
