// File: src/Healthcare.Infrastructure/Services/UserPortalService.cs
// Purpose: Implements normal-user patient profile and appointment booking workflows.
// Security: Uses EF Core LINQ queries instead of string-concatenated SQL, preventing SQL injection.
// SOLID: Keeps user portal business rules outside controllers, following Single Responsibility Principle.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Healthcare.Core.Entities;
using Healthcare.Core.Enums;
using Healthcare.Core.Exceptions;
using Healthcare.Core.Interfaces;
using Healthcare.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Healthcare.Infrastructure.Services
{
    public class UserPortalService : IUserPortalService
    {
        private readonly HealthcareDbContext _context;
        private readonly IAppointmentDomainService _appointmentDomainService;

        public UserPortalService(HealthcareDbContext context, IAppointmentDomainService appointmentDomainService)
        {
            _context = context;
            _appointmentDomainService = appointmentDomainService;
        }

        public async Task<Patient> GetPatientProfileAsync(int userId)
        {
            // Include loads the linked Patient navigation property in the same query.
            var user = await _context.Users
                .Include(x => x.Patient)
                .FirstOrDefaultAsync(x => x.Id == userId && x.IsActive);

            // If Patient is null, the user has registered but has not created a patient profile yet.
            return user == null ? null : user.Patient;
        }

        public async Task<Patient> CreateOrUpdatePatientProfileAsync(int userId, Patient patient, string performedBy)
        {
            var user = await _context.Users
                .Include(x => x.Patient)
                .FirstOrDefaultAsync(x => x.Id == userId && x.IsActive);

            if (user == null)
            {
                throw new NotFoundException("User account was not found.");
            }

            if (user.PatientId == null)
            {
                // New registered user: create a patient profile and link it to the AppUsers row.
                // This link ensures the user can only see/book appointments for their own profile.
                patient.CreatedAtUtc = DateTime.UtcNow;
                patient.IsActive = true;
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync(performedBy, default);

                user.PatientId = patient.Id;
                user.UpdatedAtUtc = DateTime.UtcNow;
                await _context.SaveChangesAsync(performedBy, default);

                return patient;
            }

            // Existing user profile: update only the allowed profile fields.
            // We do not accept PatientId from the browser, so users cannot edit another patient's profile.
            var existingPatient = await _context.Patients.FirstOrDefaultAsync(x => x.Id == user.PatientId.Value && x.IsActive);
            if (existingPatient == null)
            {
                throw new NotFoundException("Linked patient profile was not found.");
            }

            existingPatient.FullName = patient.FullName;
            existingPatient.Email = patient.Email;
            existingPatient.PhoneNumber = patient.PhoneNumber;
            existingPatient.DateOfBirth = patient.DateOfBirth;
            existingPatient.Gender = patient.Gender;
            existingPatient.Address = patient.Address;
            existingPatient.BloodGroup = patient.BloodGroup;
            existingPatient.UpdatedAtUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync(performedBy, default);
            return existingPatient;
        }

        public async Task BookAppointmentForUserAsync(int userId, int doctorId, DateTime appointmentDateTime, int durationMinutes, string reason, string performedBy)
        {
            var patient = await GetPatientProfileAsync(userId);
            if (patient == null)
            {
                // Business rule: user must create a patient profile before booking an appointment.
                throw new BusinessRuleException("Please create your patient profile before booking an appointment.");
            }

            var appointment = new Appointment
            {
                // PatientId comes from the logged-in user's linked profile, not from form input.
                // This prevents a user from booking appointments for another patient's ID.
                PatientId = patient.Id,
                DoctorId = doctorId,
                AppointmentDateTime = appointmentDateTime,
                DurationMinutes = durationMinutes,
                Reason = reason,
                Status = AppointmentStatus.Scheduled,
                IsActive = true
            };

            // Reuses the existing domain service so doctor-slot conflict validation remains centralized.
            await _appointmentDomainService.ValidateAppointmentAsync(appointment, null);

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync(performedBy, default);
        }

        public async Task<IReadOnlyList<Appointment>> GetMyAppointmentsAsync(int userId)
        {
            var patient = await GetPatientProfileAsync(userId);
            if (patient == null)
            {
                return new List<Appointment>();
            }

            // User can only see appointments belonging to their linked PatientId.
            return await _context.Appointments
                .Include(x => x.Doctor)
                .Where(x => x.PatientId == patient.Id && x.IsActive)
                .OrderByDescending(x => x.AppointmentDateTime)
                .ToListAsync();
        }
    }
}
