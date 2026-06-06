// File: src/Healthcare.API/Controllers/DoctorsController.cs
// Layer: Web API layer
// Purpose: Provides doctor details through REST API endpoints.
// Security: Protected using JWT authorization policy.

using System.Linq;
using System.Threading.Tasks;
using Healthcare.Core.DTOs;
using Healthcare.Core.Entities;
using Healthcare.Core.Exceptions;
using Healthcare.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healthcare.API.Controllers
{
    // Marks this class as a Web API controller.
    [ApiController]

    // Route becomes /api/Doctors.
    [Route("api/[controller]")]

    // Uses same authorization style as PatientsController and AppointmentsController.
    [Authorize(Policy = "ClinicalStaff")]
    public class DoctorsController : ControllerBase
    {
        // UnitOfWork gives access to repositories.
        // This avoids writing direct SQL inside the controller.
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor receives IUnitOfWork using dependency injection.
        /// This follows SOLID Dependency Inversion Principle.
        /// </summary>
        public DoctorsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ------------------------------------------------------------
        // GET: /api/Doctors
        // ------------------------------------------------------------
        // Returns all doctors.
        // This endpoint is mainly used to get DoctorId before booking appointments.
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Gets doctor records from the repository.
            var doctors = await _unitOfWork.Doctors.GetAllAsync();

            // Converts Doctor entity objects into DoctorDto objects.
            // DepartmentName is not included because it is not needed for appointment booking.
            return Ok(doctors.Select(x => ToDto(x)));
        }

        // ------------------------------------------------------------
        // GET: /api/Doctors/{id}
        // ------------------------------------------------------------
        // Example: /api/Doctors/1
        // Returns one doctor by ID.
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Finds doctor by primary key.
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);

            // If doctor does not exist, return a proper API error.
            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found.");
            }

            // Returns selected doctor details.
            return Ok(ToDto(doctor));
        }

        /// <summary>
        /// Converts Doctor entity into DoctorDto.
        /// This keeps API response clean and avoids exposing full database entity.
        /// </summary>
        private static DoctorDto ToDto(Doctor x)
        {
            return new DoctorDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Specialization = x.Specialization,
                DepartmentId = x.DepartmentId,
                IsActive = x.IsActive
            };
        }
    }
}