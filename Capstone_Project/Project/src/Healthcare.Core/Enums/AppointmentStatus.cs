// File: src/Healthcare.Core/Enums/AppointmentStatus.cs
// Layer: Core domain layer
// Purpose: This file is the enumeration that defines controlled values used by business rules and database records.
// Best-practice note: comments explain intent only; no business logic has been changed.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Core.Enums
{
    /// <summary>Defines allowed values for AppointmentStatus, preventing magic strings and improving type safety.</summary>
    public enum AppointmentStatus
    {
        Scheduled = 1,
        Completed = 2,
        Cancelled = 3,
        NoShow = 4
    }
}
