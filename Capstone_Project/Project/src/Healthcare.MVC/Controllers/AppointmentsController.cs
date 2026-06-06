// File: src/Healthcare.MVC/Controllers/AppointmentsController.cs
// Layer: MVC presentation layer
// Purpose: This file is the controller responsible for appointment booking, listing, status updates, and scheduling workflows.
// Security note: authentication, authorization, antiforgery, and validation are handled through ASP.NET Core middleware/attributes.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Controller file: receives HTTP/MVC requests, validates input, calls services/repositories, and returns views or API responses.

// Required namespaces are imported here so this file can use framework and project classes.
using System.Linq;
using System.Threading.Tasks;
using Healthcare.Core.Entities;
// Required namespaces are imported here so this file can use framework and project classes.
using Healthcare.Core.Enums;
using Healthcare.Core.Exceptions;
using Healthcare.Core.Interfaces;
// Required namespaces are imported here so this file can use framework and project classes.
using Healthcare.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
// Required namespaces are imported here so this file can use framework and project classes.
using Microsoft.AspNetCore.Mvc.Rendering;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.MVC.Controllers
{
    // Restricts this action/controller to authenticated users or a specific role.
    [Authorize(Policy = "AdminOnly")]
    /// <summary>AppointmentsController belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class AppointmentsController : Controller
    {
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly IUnitOfWork _unitOfWork;
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly IAppointmentDomainService _appointmentDomainService;

        ///  <summary>Constructor receives dependencies from ASP.NET Core dependency injection instead of creating them manually.</summary>
        public AppointmentsController(IUnitOfWork unitOfWork, IAppointmentDomainService appointmentDomainService)
        {
            _unitOfWork = unitOfWork;
            _appointmentDomainService = appointmentDomainService;
        }

        /// <summary>Loads the default listing or dashboard page for this controller.</summary>
        public async Task<IActionResult> Index()
        {
            // Returns the Razor view that renders the HTML page for the browser.
            return View(await _unitOfWork.Appointments.GetAllAsync());
        }

        /// <summary>Displays or processes the create form for a new record.</summary>
        public async Task<IActionResult> Create()
        {
            // Returns the Razor view that renders the HTML page for the browser.
            return View(await BuildModel(new AppointmentViewModel { AppointmentDateTime = System.DateTime.Now.AddDays(1), DurationMinutes = 30 }));
        }

        // Maps this action to a specific HTTP verb and route for REST/MVC routing.
        [HttpPost]
        // Validates the anti-forgery token to protect MVC forms from CSRF attacks.
        [ValidateAntiForgeryToken]
        /// <summary>Displays or processes the create form for a new record.</summary>
        public async Task<IActionResult> Create(AppointmentViewModel model)
        {
            // Stops processing when validation fails, preventing invalid input from reaching the database.
            if (!ModelState.IsValid) return View(await BuildModel(model));

            var appointment = new Appointment
            {
                PatientId = model.PatientId,
                DoctorId = model.DoctorId,
                AppointmentDateTime = model.AppointmentDateTime,
                DurationMinutes = model.DurationMinutes,
                Reason = model.Reason,
                Status = AppointmentStatus.Scheduled
            };

            // The try block protects the request from unhandled runtime/database errors.
            try
            {
                await _appointmentDomainService.ValidateAppointmentAsync(appointment, null);
                // Uses Unit of Work to coordinate repository operations and commit them together.
                await _unitOfWork.Appointments.AddAsync(appointment);
                // Uses Unit of Work to coordinate repository operations and commit them together.
                await _unitOfWork.SaveChangesAsync(User.Identity.Name);
                // Stores a one-time success/error message that appears after redirect.
                TempData["Success"] = "Appointment booked successfully.";
                // Redirects after a successful post to avoid duplicate form submissions.
                return RedirectToAction(nameof(Index));
            }
            // The catch block converts exceptions into a controlled response instead of exposing stack traces.
            catch (BusinessRuleException ex)
            {
                // Adds a validation message that will be displayed back to the user safely.
                ModelState.AddModelError(string.Empty, ex.Message);
                // Returns the Razor view that renders the HTML page for the browser.
                return View(await BuildModel(model));
            }
        }

        // Maps this action to a specific HTTP verb and route for REST/MVC routing.
        [HttpPost]
        // Validates the anti-forgery token to protect MVC forms from CSRF attacks.
        [ValidateAntiForgeryToken]
        /// <summary>Executes the Complete workflow while keeping the logic inside the correct project layer.</summary>
        public async Task<IActionResult> Complete(int id)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
            if (appointment == null) return NotFound();
            appointment.Status = AppointmentStatus.Completed;
            _unitOfWork.Appointments.Update(appointment);
            // Uses Unit of Work to coordinate repository operations and commit them together.
            await _unitOfWork.SaveChangesAsync(User.Identity.Name);
            // Redirects after a successful post to avoid duplicate form submissions.
            return RedirectToAction(nameof(Index));
        }

        // Maps this action to a specific HTTP verb and route for REST/MVC routing.
        [HttpGet]
        /// <summary>Executes the CheckSlot workflow while keeping the logic inside the correct project layer.</summary>
        public async Task<IActionResult> CheckSlot(int doctorId, System.DateTime dateTime, int durationMinutes)
        {
            var conflict = await _unitOfWork.Appointments.HasDoctorConflictAsync(doctorId, dateTime, durationMinutes, null);
            return Json(new { available = !conflict, message = conflict ? "Slot already booked." : "Slot available." });
        }

        /// <summary>Executes the BuildModel workflow while keeping the logic inside the correct project layer.</summary>
        private async Task<AppointmentViewModel> BuildModel(AppointmentViewModel model)
        {
            model.Patients = (await _unitOfWork.Patients.GetAllAsync()).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.FullName });
            model.Doctors = (await _unitOfWork.Doctors.GetAllAsync()).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.FullName + " - " + x.Specialization });
            return model;
        }
    }
}
