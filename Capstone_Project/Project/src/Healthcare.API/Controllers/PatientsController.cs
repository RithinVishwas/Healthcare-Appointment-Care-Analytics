// File: src/Healthcare.API/Controllers/PatientsController.cs
// Layer: Web API layer
// Purpose: This file is the controller responsible for patient management operations such as listing, creating, updating, and deleting patients.
// Security note: authentication, authorization, antiforgery, and validation are handled through ASP.NET Core middleware/attributes.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Controller file: receives HTTP/MVC requests, validates input, calls services/repositories, and returns views or API responses.

// Required namespaces are imported here so this file can use framework and project classes.
using System.Linq;
using System.Threading.Tasks;
using Healthcare.Core.DTOs;
// Required namespaces are imported here so this file can use framework and project classes.
using Healthcare.Core.Entities;
using Healthcare.Core.Exceptions;
using Healthcare.Core.Interfaces;
// Required namespaces are imported here so this file can use framework and project classes.
using Microsoft.AspNetCore.Authorization;
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
    /// <summary>PatientsController belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class PatientsController : ControllerBase
    {
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly IUnitOfWork _unitOfWork;

        ///  <summary>Constructor receives dependencies from ASP.NET Core dependency injection instead of creating them manually.</summary>
        public PatientsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Maps this action to a specific HTTP verb and route for REST/MVC routing.
        [HttpGet]
        /// <summary>Returns all active records through the REST API endpoint.</summary>
        public async Task<IActionResult> GetAll()
        {
            var patients = await _unitOfWork.Patients.GetAllAsync();
            // Returns a successful API response with the requested data.
            return Ok(patients.Select(x => ToDto(x)));
        }

        // Maps this action to a specific HTTP verb and route for REST/MVC routing.
        [HttpGet("{id:int}")]
        /// <summary>Returns one record by id and responds with NotFound when it does not exist.</summary>
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(id);
            if (patient == null) throw new NotFoundException("Patient not found.");
            // Returns a successful API response with the requested data.
            return Ok(ToDto(patient));
        }

        // Maps this action to a specific HTTP verb and route for REST/MVC routing.
        [HttpPost]
        // Restricts this action/controller to authenticated users or a specific role.
        [Authorize(Policy = "AdminOnly")]
        /// <summary>Displays or processes the create form for a new record.</summary>
        public async Task<IActionResult> Create([FromBody] PatientDto dto)
        {
            // Stops processing when validation fails, preventing invalid input from reaching the database.
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (await _unitOfWork.Patients.EmailExistsAsync(dto.Email, null)) return BadRequest(new { message = "Patient email already exists." });

            var patient = new Patient
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Address = dto.Address,
                BloodGroup = dto.BloodGroup
            };
            // Uses Unit of Work to coordinate repository operations and commit them together.
            await _unitOfWork.Patients.AddAsync(patient);
            // Uses Unit of Work to coordinate repository operations and commit them together.
            await _unitOfWork.SaveChangesAsync(User.Identity.Name ?? "api");
            dto.Id = patient.Id;
            return CreatedAtAction(nameof(GetById), new { id = patient.Id }, dto);
        }

        // Maps this action to a specific HTTP verb and route for REST/MVC routing.
        [HttpPut("{id:int}")]
        // Restricts this action/controller to authenticated users or a specific role.
        [Authorize(Policy = "AdminOnly")]
        /// <summary>Executes the Update workflow while keeping the logic inside the correct project layer.</summary>
        public async Task<IActionResult> Update(int id, [FromBody] PatientDto dto)
        {
            // Stops processing when validation fails, preventing invalid input from reaching the database.
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var patient = await _unitOfWork.Patients.GetByIdAsync(id);
            if (patient == null) throw new NotFoundException("Patient not found.");
            if (await _unitOfWork.Patients.EmailExistsAsync(dto.Email, id)) return BadRequest(new { message = "Patient email already exists." });

            patient.FullName = dto.FullName;
            patient.Email = dto.Email;
            patient.PhoneNumber = dto.PhoneNumber;
            patient.DateOfBirth = dto.DateOfBirth;
            patient.Gender = dto.Gender;
            patient.Address = dto.Address;
            patient.BloodGroup = dto.BloodGroup;
            _unitOfWork.Patients.Update(patient);
            // Uses Unit of Work to coordinate repository operations and commit them together.
            await _unitOfWork.SaveChangesAsync(User.Identity.Name ?? "api");
            return NoContent();
        }

        /// <summary>Executes the ToDto workflow while keeping the logic inside the correct project layer.</summary>
        private static PatientDto ToDto(Patient x)
        {
            return new PatientDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                DateOfBirth = x.DateOfBirth,
                Gender = x.Gender,
                Address = x.Address,
                BloodGroup = x.BloodGroup
            };
        }
    }
}
