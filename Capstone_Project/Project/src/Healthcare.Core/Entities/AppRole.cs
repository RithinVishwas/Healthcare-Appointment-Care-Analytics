// File: src/Healthcare.Core/Entities/AppRole.cs
// Layer: Core domain layer
// Purpose: This file is the domain entity class that represents a normalized business/database object used by EF Core and SQL Server.
// Best-practice note: comments explain intent only; no business logic has been changed.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Entity file: represents a normalized table/domain object used by EF Core and SQL Server.

// Required namespaces are imported here so this file can use framework and project classes.
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Core.Entities
{
    /// <summary>AppRole belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class AppRole : BaseEntity
    {
        ///  <summary>Constructor receives dependencies from ASP.NET Core dependency injection instead of creating them manually.</summary>
        public AppRole()
        {
            Users = new List<AppUser>();
        }

        // Requires this field so invalid or incomplete form/API input is rejected.
        [Required, StringLength(50)]
        /// <summary>Stores a short readable name such as role name or department name.</summary>
        public string Name { get; set; }

        /// <summary>Stores data used by the healthcare workflow, validation, or reporting screens.</summary>
        /// <summary>Navigation collection for related AppUser records; EF Core uses it to model one-to-many relationships.</summary>
        public ICollection<AppUser> Users { get; set; }
    }
}
