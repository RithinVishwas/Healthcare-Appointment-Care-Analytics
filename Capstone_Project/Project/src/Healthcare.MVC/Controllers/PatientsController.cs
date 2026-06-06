// File: src/Healthcare.MVC/Controllers/PatientsController.cs
// Layer: MVC presentation layer
// Purpose: This file is the controller responsible for patient management operations such as listing, creating, updating, and deleting patients.
// Security note: authentication, authorization, antiforgery, and validation are handled through ASP.NET Core middleware/attributes.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Controller file: receives HTTP/MVC requests, validates input, calls services/repositories, and returns views or API responses.

// Required namespaces are imported here so this file can use framework and project classes.
using System.Threading.Tasks;
using Healthcare.Core.Entities;
using Healthcare.Core.Interfaces;
// Required namespaces are imported here so this file can use framework and project classes.
using Healthcare.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.MVC.Controllers
{
    // Restricts this action/controller to authenticated users or a specific role.
    [Authorize]
    /// <summary>PatientsController belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class PatientsController : Controller
    {
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly IUnitOfWork _unitOfWork;

        ///  <summary>Constructor receives dependencies from ASP.NET Core dependency injection instead of creating them manually.</summary>
        public PatientsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>Loads the default listing or dashboard page for this controller.</summary>
        public async Task<IActionResult> Index()
        {
            // Returns the Razor view that renders the HTML page for the browser.
            return View(await _unitOfWork.Patients.GetAllAsync());
        }

        // Restricts this action/controller to authenticated users or a specific role.
        [Authorize(Policy = "AdminOnly")]
        /// <summary>Displays or processes the create form for a new record.</summary>
        public IActionResult Create()
        {
            // Returns the Razor view that renders the HTML page for the browser.
            return View(new PatientViewModel { DateOfBirth = System.DateTime.Today.AddYears(-25) });
        }

        // Maps this action to a specific HTTP verb and route for REST/MVC routing.
        [HttpPost]
        // Restricts this action/controller to authenticated users or a specific role.
        [Authorize(Policy = "AdminOnly")]
        // Validates the anti-forgery token to protect MVC forms from CSRF attacks.
        [ValidateAntiForgeryToken]
        /// <summary>Displays or processes the create form for a new record.</summary>
        public async Task<IActionResult> Create(PatientViewModel model)
        {
            // Stops processing when validation fails, preventing invalid input from reaching the database.
            if (!ModelState.IsValid) return View(model);
            if (await _unitOfWork.Patients.EmailExistsAsync(model.Email, null))
            {
                // Adds a validation message that will be displayed back to the user safely.
                ModelState.AddModelError(nameof(model.Email), "Email already exists.");
                // Returns the Razor view that renders the HTML page for the browser.
                return View(model);
            }

            var patient = new Patient
            {
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                Address = model.Address,
                BloodGroup = model.BloodGroup
            };
            // Uses Unit of Work to coordinate repository operations and commit them together.
            await _unitOfWork.Patients.AddAsync(patient);
            // Uses Unit of Work to coordinate repository operations and commit them together.
            await _unitOfWork.SaveChangesAsync(User.Identity.Name);
            // Stores a one-time success/error message that appears after redirect.
            TempData["Success"] = "Patient created successfully.";
            // Redirects after a successful post to avoid duplicate form submissions.
            return RedirectToAction(nameof(Index));
        }

        // Restricts this action/controller to authenticated users or a specific role.
        [Authorize(Policy = "AdminOnly")]
        /// <summary>Displays or processes the edit form for an existing record.</summary>
        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(id);
            if (patient == null) return NotFound();
            // Returns the Razor view that renders the HTML page for the browser.
            return View(new PatientViewModel
            {
                Id = patient.Id,
                FullName = patient.FullName,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                Address = patient.Address,
                BloodGroup = patient.BloodGroup
            });
        }

        // Maps this action to a specific HTTP verb and route for REST/MVC routing.
        [HttpPost]
        // Restricts this action/controller to authenticated users or a specific role.
        [Authorize(Policy = "AdminOnly")]
        // Validates the anti-forgery token to protect MVC forms from CSRF attacks.
        [ValidateAntiForgeryToken]
        /// <summary>Displays or processes the edit form for an existing record.</summary>
        public async Task<IActionResult> Edit(int id, PatientViewModel model)
        {
            if (id != model.Id) return BadRequest();
            // Stops processing when validation fails, preventing invalid input from reaching the database.
            if (!ModelState.IsValid) return View(model);

            var patient = await _unitOfWork.Patients.GetByIdAsync(id);
            if (patient == null) return NotFound();
            if (await _unitOfWork.Patients.EmailExistsAsync(model.Email, id))
            {
                // Adds a validation message that will be displayed back to the user safely.
                ModelState.AddModelError(nameof(model.Email), "Email already exists.");
                // Returns the Razor view that renders the HTML page for the browser.
                return View(model);
            }

            patient.FullName = model.FullName;
            patient.Email = model.Email;
            patient.PhoneNumber = model.PhoneNumber;
            patient.DateOfBirth = model.DateOfBirth;
            patient.Gender = model.Gender;
            patient.Address = model.Address;
            patient.BloodGroup = model.BloodGroup;
            _unitOfWork.Patients.Update(patient);
            // Uses Unit of Work to coordinate repository operations and commit them together.
            await _unitOfWork.SaveChangesAsync(User.Identity.Name);
            // Stores a one-time success/error message that appears after redirect.
            TempData["Success"] = "Patient updated successfully.";
            // Redirects after a successful post to avoid duplicate form submissions.
            return RedirectToAction(nameof(Index));
        }
    }
}
