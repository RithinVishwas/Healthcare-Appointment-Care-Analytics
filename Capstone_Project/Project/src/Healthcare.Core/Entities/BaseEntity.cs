// File: src/Healthcare.Core/Entities/BaseEntity.cs
// Layer: Core domain layer
// Purpose: This file is the domain entity class that represents a normalized business/database object used by EF Core and SQL Server.
// Best-practice note: comments explain intent only; no business logic has been changed.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Entity file: represents a normalized table/domain object used by EF Core and SQL Server.

// Required namespaces are imported here so this file can use framework and project classes.
using System;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Core.Entities
{
    public abstract class BaseEntity
    {
        /// <summary>Primary key value used by EF Core and SQL Server to uniquely identify the record.</summary>
        public int Id { get; set; }
        /// <summary>Audit field that records when the row was created in UTC.</summary>
        public DateTime CreatedAtUtc { get; set; }
        /// <summary>Audit field that records when the row was last updated in UTC.</summary>
        public DateTime? UpdatedAtUtc { get; set; }
        /// <summary>Soft-delete/status flag used to hide inactive data without physically deleting rows.</summary>
        public bool IsActive { get; set; }
    }
}
