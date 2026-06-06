// File: src/Healthcare.Core/DTOs/AnalyticsSummaryDto.cs
// Layer: Core domain layer
// Purpose: This file is the data transfer object used to move validated data between controllers, services, and clients.
// Best-practice note: comments explain intent only; no business logic has been changed.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Core.DTOs
{
    /// <summary>AnalyticsSummaryDto belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class AnalyticsSummaryDto
    {
        /// <summary>Analytics value showing number of active patients.</summary>
        public int TotalPatients { get; set; }
        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        public int TotalDoctors { get; set; }
        /// <summary>Analytics value showing number of appointments in the system.</summary>
        public int TotalAppointments { get; set; }
        /// <summary>Analytics value showing appointments still scheduled.</summary>
        public int ScheduledAppointments { get; set; }
        /// <summary>Analytics value showing completed appointments.</summary>
        public int CompletedAppointments { get; set; }
        /// <summary>Analytics value showing cancelled appointments.</summary>
        public int CancelledAppointments { get; set; }
    }
}
