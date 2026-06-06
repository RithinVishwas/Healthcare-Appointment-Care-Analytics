// File: src/Healthcare.Core/Exceptions/NotFoundException.cs
// Layer: Core domain layer
// Purpose: This file is the custom exception type used to represent predictable application/business error scenarios.
// Best-practice note: comments explain intent only; no business logic has been changed.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: C# source file: part of the layered healthcare appointment system.

// Required namespaces are imported here so this file can use framework and project classes.
using System;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Core.Exceptions
{
    /// <summary>NotFoundException belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class NotFoundException : Exception
    {
        ///  <summary>Constructor receives dependencies from ASP.NET Core dependency injection instead of creating them manually.</summary>
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
