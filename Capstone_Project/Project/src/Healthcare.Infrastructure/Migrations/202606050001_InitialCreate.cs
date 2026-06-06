// File: src/Healthcare.Infrastructure/Migrations/202606050001_InitialCreate.cs
// Layer: Infrastructure/data-access layer
// Purpose: This file is the EF Core migration file that describes database schema creation or schema snapshot metadata.
// Best-practice note: comments explain intent only; no business logic has been changed.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Migration file: describes database schema changes generated for EF Core.

// Required namespaces are imported here so this file can use framework and project classes.
using System;
using Healthcare.Core.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        /// <summary>Executes the Up workflow while keeping the logic inside the correct project layer.</summary>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE TABLE Departments (Id INT IDENTITY PRIMARY KEY, Name NVARCHAR(100) NOT NULL UNIQUE, Description NVARCHAR(500) NULL, CreatedAtUtc DATETIME2 NOT NULL, UpdatedAtUtc DATETIME2 NULL, IsActive BIT NOT NULL)");
            migrationBuilder.Sql("CREATE TABLE Patients (Id INT IDENTITY PRIMARY KEY, FullName NVARCHAR(100) NOT NULL, Email NVARCHAR(150) NOT NULL UNIQUE, PhoneNumber NVARCHAR(15) NOT NULL, DateOfBirth DATE NOT NULL, Gender NVARCHAR(20) NOT NULL, Address NVARCHAR(500) NOT NULL, BloodGroup NVARCHAR(100) NULL, CreatedAtUtc DATETIME2 NOT NULL, UpdatedAtUtc DATETIME2 NULL, IsActive BIT NOT NULL)");
            migrationBuilder.Sql("CREATE TABLE Doctors (Id INT IDENTITY PRIMARY KEY, FullName NVARCHAR(100) NOT NULL, Specialization NVARCHAR(100) NOT NULL, Email NVARCHAR(150) NOT NULL UNIQUE, PhoneNumber NVARCHAR(15) NOT NULL, DepartmentId INT NOT NULL FOREIGN KEY REFERENCES Departments(Id), CreatedAtUtc DATETIME2 NOT NULL, UpdatedAtUtc DATETIME2 NULL, IsActive BIT NOT NULL)");
            migrationBuilder.Sql("CREATE TABLE AppRoles (Id INT IDENTITY PRIMARY KEY, Name NVARCHAR(50) NOT NULL UNIQUE, CreatedAtUtc DATETIME2 NOT NULL, UpdatedAtUtc DATETIME2 NULL, IsActive BIT NOT NULL)");
            migrationBuilder.Sql("CREATE TABLE AppUsers (Id INT IDENTITY PRIMARY KEY, FullName NVARCHAR(100) NOT NULL, Email NVARCHAR(150) NOT NULL UNIQUE, PasswordHash NVARCHAR(256) NOT NULL, RoleId INT NOT NULL FOREIGN KEY REFERENCES AppRoles(Id), CreatedAtUtc DATETIME2 NOT NULL, UpdatedAtUtc DATETIME2 NULL, IsActive BIT NOT NULL)");
            migrationBuilder.Sql("CREATE TABLE Appointments (Id INT IDENTITY PRIMARY KEY, PatientId INT NOT NULL FOREIGN KEY REFERENCES Patients(Id), DoctorId INT NOT NULL FOREIGN KEY REFERENCES Doctors(Id), AppointmentDateTime DATETIME2 NOT NULL, DurationMinutes INT NOT NULL, Status INT NOT NULL, Reason NVARCHAR(500) NOT NULL, Notes NVARCHAR(1000) NULL, CreatedAtUtc DATETIME2 NOT NULL, UpdatedAtUtc DATETIME2 NULL, IsActive BIT NOT NULL)");
            migrationBuilder.Sql("CREATE TABLE MedicalRecords (Id INT IDENTITY PRIMARY KEY, PatientId INT NOT NULL FOREIGN KEY REFERENCES Patients(Id), AppointmentId INT NULL FOREIGN KEY REFERENCES Appointments(Id), Diagnosis NVARCHAR(1000) NOT NULL, Prescription NVARCHAR(1000) NULL, VisitDate DATETIME2 NOT NULL, CreatedAtUtc DATETIME2 NOT NULL, UpdatedAtUtc DATETIME2 NULL, IsActive BIT NOT NULL)");
            migrationBuilder.Sql("CREATE TABLE AuditLogs (Id INT IDENTITY PRIMARY KEY, EntityName NVARCHAR(100) NOT NULL, EntityId INT NOT NULL, Action NVARCHAR(50) NOT NULL, PerformedBy NVARCHAR(150) NOT NULL, PerformedAtUtc DATETIME2 NOT NULL, Details NVARCHAR(1000) NULL, AppointmentId INT NULL FOREIGN KEY REFERENCES Appointments(Id), CreatedAtUtc DATETIME2 NOT NULL, UpdatedAtUtc DATETIME2 NULL, IsActive BIT NOT NULL)");
        }

        /// <summary>Executes the Down workflow while keeping the logic inside the correct project layer.</summary>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE IF EXISTS AuditLogs; DROP TABLE IF EXISTS MedicalRecords; DROP TABLE IF EXISTS Appointments; DROP TABLE IF EXISTS AppUsers; DROP TABLE IF EXISTS AppRoles; DROP TABLE IF EXISTS Doctors; DROP TABLE IF EXISTS Patients; DROP TABLE IF EXISTS Departments;");
        }
    }
}
