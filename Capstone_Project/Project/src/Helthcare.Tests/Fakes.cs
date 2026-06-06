// File: src/Healthcare.Tests/Fakes.cs
// Layer: Unit testing layer
// Purpose: This file is the code/configuration file that supports the Healthcare Appointment & Care Analytics application.
// Best-practice note: comments explain intent only; no business logic has been changed.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: C# source file: part of the layered healthcare appointment system.

// Required namespaces are imported here so this file can use framework and project classes.
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
// Required namespaces are imported here so this file can use framework and project classes.
using Healthcare.Core.Entities;
using Healthcare.Core.Interfaces;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Tests
{
    /// <summary>FakeRepository belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class FakeRepository<T> : IRepository<T> where T : BaseEntity
    {
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly T _entity;
        /// <summary>Executes the FakeRepository workflow while keeping the logic inside the correct project layer.</summary>
        public FakeRepository(T entity) { _entity = entity; }
        /// <summary>Executes the AddAsync workflow while keeping the logic inside the correct project layer.</summary>
        public Task AddAsync(T entity) { return Task.CompletedTask; }
        /// <summary>Handles removal or deactivation of a selected record.</summary>
        public void Delete(T entity) { }
        /// <summary>Executes the GetAllAsync workflow while keeping the logic inside the correct project layer.</summary>
        public Task<IReadOnlyList<T>> GetAllAsync() { return Task.FromResult((IReadOnlyList<T>)new List<T> { _entity }); }
        /// <summary>Executes the GetByIdAsync workflow while keeping the logic inside the correct project layer.</summary>
        public Task<T> GetByIdAsync(int id) { return Task.FromResult(_entity); }
        /// <summary>Executes the Update workflow while keeping the logic inside the correct project layer.</summary>
        public void Update(T entity) { }
    }

    /// <summary>FakeAppointmentRepository belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class FakeAppointmentRepository : FakeRepository<Appointment>, IAppointmentRepository
    {
        // Dependency stored in a readonly field to follow dependency injection and immutability best practice.
        private readonly bool _doctorConflict;
        /// <summary>Executes the FakeAppointmentRepository workflow while keeping the logic inside the correct project layer.</summary>
        public FakeAppointmentRepository(bool doctorConflict) : base(new Appointment { Id = 1 }) { _doctorConflict = doctorConflict; }
        /// <summary>Executes the GetUpcomingAsync workflow while keeping the logic inside the correct project layer.</summary>
        public Task<IReadOnlyList<Appointment>> GetUpcomingAsync() { return Task.FromResult((IReadOnlyList<Appointment>)new List<Appointment>()); }
        /// <summary>Executes the HasDoctorConflictAsync workflow while keeping the logic inside the correct project layer.</summary>
        public Task<bool> HasDoctorConflictAsync(int doctorId, DateTime start, int durationMinutes, int? ignoredAppointmentId) { return Task.FromResult(_doctorConflict); }
        /// <summary>Executes the HasPatientConflictAsync workflow while keeping the logic inside the correct project layer.</summary>
        public Task<bool> HasPatientConflictAsync(int patientId, DateTime start, int durationMinutes, int? ignoredAppointmentId) { return Task.FromResult(false); }
    }
}
