// File: src/Healthcare.Core/DTOs/DoctorDto.cs
// Layer: Core DTO layer
// Purpose: Sends only required doctor details to Swagger/Postman/API clients.
// DepartmentName is removed because appointment booking only needs DoctorId.

namespace Healthcare.Core.DTOs
{
    /// <summary>
    /// DoctorDto represents doctor information returned by the API.
    /// This DTO is useful for selecting a doctor while booking an appointment.
    /// </summary>
    public class DoctorDto
    {
        // Unique doctor ID from the Doctors table.
        // This value is required while creating an appointment.
        public int Id { get; set; }

        // Doctor's full name displayed in API response.
        public string FullName { get; set; } = string.Empty;

        // Doctor's email address.
        public string Email { get; set; } = string.Empty;

        // Doctor's phone number.
        public string PhoneNumber { get; set; } = string.Empty;

        // Doctor specialization such as General Physician, Cardiologist, Orthopedic Surgeon.
        public string Specialization { get; set; } = string.Empty;

        // DepartmentId is kept because it shows which department the doctor belongs to.
        // DepartmentName is not required for appointment booking.
        public int DepartmentId { get; set; }

        // Shows whether the doctor is currently active and available.
        public bool IsActive { get; set; }
    }
}