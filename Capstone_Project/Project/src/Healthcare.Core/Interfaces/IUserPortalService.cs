// File: src/Healthcare.Core/Interfaces/IUserPortalService.cs
// Purpose: Defines user-portal business operations using an abstraction to follow Dependency Inversion.
// SOLID: Controllers depend on this interface, not directly on a concrete data-access class.

using System.Collections.Generic;
using System.Threading.Tasks;
using Healthcare.Core.Entities;

namespace Healthcare.Core.Interfaces
{
    public interface IUserPortalService
    {
        // Gets the patient profile linked to the currently logged-in user.
        Task<Patient> GetPatientProfileAsync(int userId);

        // Creates a first-time patient profile or updates the existing profile for the logged-in user.
        Task<Patient> CreateOrUpdatePatientProfileAsync(int userId, Patient patient, string performedBy);

        // Books an appointment only for the patient profile linked to the logged-in user.
        Task BookAppointmentForUserAsync(int userId, int doctorId, System.DateTime appointmentDateTime, int durationMinutes, string reason, string performedBy);

        // Returns only the logged-in user's appointment list, not all patients' appointments.
        Task<IReadOnlyList<Appointment>> GetMyAppointmentsAsync(int userId);
    }
}
