// File: src/Healthcare.API/Controllers/AppointmentsController.cs
// Layer: Web API layer
// Purpose: Handles appointment API operations such as list, create, upcoming, get by id, and status update.
// Important fix: GET methods now return AppointmentDto instead of full Appointment entity.
// Why? Returning full EF Core entities can cause JSON serialization issues because entities may contain navigation properties.

using System.Linq;
using System.Threading.Tasks;
using Healthcare.Core.DTOs;
using Healthcare.Core.Entities;
using Healthcare.Core.Enums;
using Healthcare.Core.Exceptions;
using Healthcare.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healthcare.API.Controllers
{
    // Enables Web API behavior such as automatic request model validation.
    [ApiController]

    // Route becomes /api/Appointments.
    [Route("api/[controller]")]

    // Requires JWT token and ClinicalStaff policy to access this controller.
    [Authorize(Policy = "ClinicalStaff")]
    public class AppointmentsController : ControllerBase
    {
        // UnitOfWork gives access to repositories such as Appointments.
        private readonly IUnitOfWork _unitOfWork;

        // Domain service contains appointment business rules,
        // such as checking duplicate doctor time slots.
        private readonly IAppointmentDomainService _appointmentDomainService;

        /// <summary>
        /// Constructor receives dependencies through ASP.NET Core Dependency Injection.
        /// This follows Dependency Inversion Principle from SOLID.
        /// </summary>
        public AppointmentsController(
            IUnitOfWork unitOfWork,
            IAppointmentDomainService appointmentDomainService)
        {
            _unitOfWork = unitOfWork;
            _appointmentDomainService = appointmentDomainService;
        }

        // ------------------------------------------------------------
        // GET: /api/Appointments
        // ------------------------------------------------------------
        // Returns all appointment records.
        // Fix: Converts Appointment entity to AppointmentDto before returning JSON.
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Gets appointments from repository.
            var appointments = await _unitOfWork.Appointments.GetAllAsync();

            // Converts entities to DTOs.
            // This avoids returning full EF Core navigation objects,
            // preventing JSON cycle/serialization problems.
            var result = appointments.Select(x => ToDto(x));

            // Returns HTTP 200 OK with clean appointment data.
            return Ok(result);
        }

        // ------------------------------------------------------------
        // GET: /api/Appointments/upcoming
        // ------------------------------------------------------------
        // Returns future/upcoming appointments.
        [HttpGet("upcoming")]
        public async Task<IActionResult> Upcoming()
        {
            // Gets only upcoming appointments from repository.
            var appointments = await _unitOfWork.Appointments.GetUpcomingAsync();

            // Converts entities to DTOs for safe API output.
            var result = appointments.Select(x => ToDto(x));

            return Ok(result);
        }

        // ------------------------------------------------------------
        // POST: /api/Appointments
        // ------------------------------------------------------------
        // Creates a new appointment using JSON request body.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AppointmentDto dto)
        {
            // Stops invalid data before it reaches business logic/database.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Converts incoming DTO into Appointment entity.
            // Entity is used internally for EF Core/database operations.
            var appointment = new Appointment
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                AppointmentDateTime = dto.AppointmentDateTime,
                DurationMinutes = dto.DurationMinutes,
                Reason = dto.Reason,

                // New appointment is always created as Scheduled by default.
                Status = AppointmentStatus.Scheduled
            };

            // Applies business rules before saving.
            // Example: prevents duplicate appointment for same doctor/time slot.
            await _appointmentDomainService.ValidateAppointmentAsync(appointment, null);

            // Adds appointment to repository.
            await _unitOfWork.Appointments.AddAsync(appointment);

            // Saves database changes.
            // User.Identity.Name is used for audit tracking.
            await _unitOfWork.SaveChangesAsync(User.Identity?.Name ?? "api");

            // Return DTO instead of entity to keep response clean.
            var response = ToDto(appointment);

            // Returns HTTP 201 Created with location of the new appointment.
            return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, response);
        }

        // ------------------------------------------------------------
        // GET: /api/Appointments/{id}
        // ------------------------------------------------------------
        // Returns one appointment by ID.
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Finds appointment by primary key.
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);

            // If appointment does not exist, return proper not found error.
            if (appointment == null)
            {
                throw new NotFoundException("Appointment not found.");
            }

            // Converts entity into DTO before returning.
            return Ok(ToDto(appointment));
        }

        // ------------------------------------------------------------
        // PATCH: /api/Appointments/{id}/status/{status}
        // ------------------------------------------------------------
        // Updates appointment status.
        // Example: /api/Appointments/1/status/2
        [HttpPatch("{id:int}/status/{status:int}")]
        public async Task<IActionResult> ChangeStatus(int id, AppointmentStatus status)
        {
            // Finds appointment by ID.
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);

            // If appointment is missing, return not found.
            if (appointment == null)
            {
                throw new NotFoundException("Appointment not found.");
            }

            // Updates appointment status.
            appointment.Status = status;

            // Marks appointment as updated in repository.
            _unitOfWork.Appointments.Update(appointment);

            // Saves status change to SQL Server.
            await _unitOfWork.SaveChangesAsync(User.Identity?.Name ?? "api");

            // 204 No Content means update succeeded but no body is returned.
            return NoContent();
        }

        // ------------------------------------------------------------
        // ENTITY TO DTO MAPPING
        // ------------------------------------------------------------
        // Converts Appointment entity into AppointmentDto.
        // This keeps API output clean and avoids JSON serialization issues.
        private static AppointmentDto ToDto(Appointment x)
        {
            return new AppointmentDto
            {
                Id = x.Id,
                PatientId = x.PatientId,
                DoctorId = x.DoctorId,
                AppointmentDateTime = x.AppointmentDateTime,
                DurationMinutes = x.DurationMinutes,
                Reason = x.Reason,
                Status = x.Status
            };
        }
    }
}