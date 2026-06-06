// File: src/Healthcare.Core/DTOs/DepartmentAppointmentReportDto.cs
// Layer: Core domain layer
// Purpose: This file is the data transfer object used to move validated data between controllers, services, and clients.
// Best-practice note: comments explain intent only; no business logic has been changed.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Core.DTOs
{
    /// <summary>DepartmentAppointmentReportDto belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class DepartmentAppointmentReportDto
    {
        /// <summary>Report field showing the department name.</summary>
        public string DepartmentName { get; set; }
        /// <summary>Analytics value showing number of appointments in the system.</summary>
        public int TotalAppointments { get; set; }
        /// <summary>Analytics value showing completed appointments.</summary>
        public int CompletedAppointments { get; set; }
        /// <summary>Analytics value showing appointments still scheduled.</summary>
        public int ScheduledAppointments { get; set; }
    }
}
