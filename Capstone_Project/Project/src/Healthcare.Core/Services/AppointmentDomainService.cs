// File: src/Healthcare.Core/Services/AppointmentDomainService.cs
// Layer: Core domain layer
// Purpose: This file is the domain service that enforces appointment business rules such as scheduling, validation, and overlap checks.
// Best-practice note: comments explain intent only; no business logic has been changed.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Service file: contains reusable business/security logic separated from UI and controllers.

// Required namespaces are imported here so this file can use framework and project classes.
using System;
using System.Threading.Tasks;
using Healthcare.Core.Entities;
// Required namespaces are imported here so this file can use framework and project classes.
using Healthcare.Core.Enums;
using Healthcare.Core.Exceptions;
using Healthcare.Core.Interfaces;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Core.Services
{
    // Single Responsibility: this class validates appointment business rules only.
    /// <summary>AppointmentDomainService belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class AppointmentDomainService : IAppointmentDomainService
    {
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly IAppointmentRepository _appointmentRepository;
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly IRepository<Patient> _patients;
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly IRepository<Doctor> _doctors;

        ///  <summary>Constructor receives dependencies from ASP.NET Core dependency injection instead of creating them manually.</summary>
        public AppointmentDomainService(IAppointmentRepository appointmentRepository, IRepository<Patient> patients, IRepository<Doctor> doctors)
        {
            _appointmentRepository = appointmentRepository;
            _patients = patients;
            _doctors = doctors;
        }

        /// <summary>Executes the ValidateAppointmentAsync workflow while keeping the logic inside the correct project layer.</summary>
        public async Task ValidateAppointmentAsync(Appointment appointment, int? ignoredAppointmentId)
        {
            if (appointment == null)
            {
                throw new BusinessRuleException("Appointment information is required.");
            }

            if (appointment.PatientId <= 0)
            {
                throw new BusinessRuleException("A valid patient is required.");
            }

            if (appointment.DoctorId <= 0)
            {
                throw new BusinessRuleException("A valid doctor is required.");
            }

            if (appointment.AppointmentDateTime <= DateTime.Now.AddMinutes(5))
            {
                throw new BusinessRuleException("Appointment must be scheduled for a future time.");
            }

            if (appointment.DurationMinutes < 15 || appointment.DurationMinutes > 180)
            {
                throw new BusinessRuleException("Appointment duration must be between 15 and 180 minutes.");
            }

            var patient = await _patients.GetByIdAsync(appointment.PatientId);
            if (patient == null || !patient.IsActive)
            {
                throw new BusinessRuleException("Selected patient does not exist or is inactive.");
            }

            var doctor = await _doctors.GetByIdAsync(appointment.DoctorId);
            if (doctor == null || !doctor.IsActive)
            {
                throw new BusinessRuleException("Selected doctor does not exist or is inactive.");
            }

            if (await _appointmentRepository.HasDoctorConflictAsync(appointment.DoctorId, appointment.AppointmentDateTime, appointment.DurationMinutes, ignoredAppointmentId))
            {
                throw new BusinessRuleException("Doctor already has another appointment in this time slot.");
            }

            if (await _appointmentRepository.HasPatientConflictAsync(appointment.PatientId, appointment.AppointmentDateTime, appointment.DurationMinutes, ignoredAppointmentId))
            {
                throw new BusinessRuleException("Patient already has another appointment in this time slot.");
            }

            if (appointment.Status == 0)
            {
                appointment.Status = AppointmentStatus.Scheduled;
            }
        }
    }
}
