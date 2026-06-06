// File: src/Healthcare.Tests/AppointmentDomainServiceTests.cs
// Layer: Unit testing layer
// Purpose: This file is the code/configuration file that supports the Healthcare Appointment & Care Analytics application.
// Best-practice note: comments explain intent only; no business logic has been changed.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: C# source file: part of the layered healthcare appointment system.

// Required namespaces are imported here so this file can use framework and project classes.
using System;
using System.Threading.Tasks;
using Healthcare.Core.Entities;
// Required namespaces are imported here so this file can use framework and project classes.
using Healthcare.Core.Exceptions;
using Healthcare.Core.Interfaces;
using Healthcare.Core.Services;
// Required namespaces are imported here so this file can use framework and project classes.
using Xunit;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Tests
{
    /// <summary>AppointmentDomainServiceTests belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class AppointmentDomainServiceTests
    {
        [Fact]
        /// <summary>Executes the ValidateAppointmentAsync_RejectsPastAppointment workflow while keeping the logic inside the correct project layer.</summary>
        public async Task ValidateAppointmentAsync_RejectsPastAppointment()
        {
            var repo = new FakeAppointmentRepository(false);
            var patients = new FakeRepository<Patient>(new Patient { Id = 1, IsActive = true });
            var doctors = new FakeRepository<Doctor>(new Doctor { Id = 1, IsActive = true });
            var service = new AppointmentDomainService(repo, patients, doctors);

            var appointment = new Appointment { PatientId = 1, DoctorId = 1, AppointmentDateTime = DateTime.Now.AddMinutes(-10), DurationMinutes = 30, Reason = "Test" };
            await Assert.ThrowsAsync<BusinessRuleException>(() => service.ValidateAppointmentAsync(appointment, null));
        }

        [Fact]
        /// <summary>Executes the ValidateAppointmentAsync_RejectsDoctorConflict workflow while keeping the logic inside the correct project layer.</summary>
        public async Task ValidateAppointmentAsync_RejectsDoctorConflict()
        {
            var repo = new FakeAppointmentRepository(true);
            var patients = new FakeRepository<Patient>(new Patient { Id = 1, IsActive = true });
            var doctors = new FakeRepository<Doctor>(new Doctor { Id = 1, IsActive = true });
            var service = new AppointmentDomainService(repo, patients, doctors);

            var appointment = new Appointment { PatientId = 1, DoctorId = 1, AppointmentDateTime = DateTime.Now.AddDays(1), DurationMinutes = 30, Reason = "Test" };
            await Assert.ThrowsAsync<BusinessRuleException>(() => service.ValidateAppointmentAsync(appointment, null));
        }
    }
}
