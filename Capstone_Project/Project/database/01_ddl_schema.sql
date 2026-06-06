-- DETAILED SQL COMMENT UPDATE
-- The comments below explain each database block for evaluation/demo.
-- No SQL statements or execution order were changed; only comments were added.

-- File: database/01_ddl_schema.sql
-- Layer: Database layer
-- Purpose: This file is the DDL script that creates the SQL Server database schema, constraints, keys, and normalized tables.
-- Security note: data access should use EF Core or parameterized SQL to avoid SQL injection.
-- Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

/*
Healthcare Appointment & Care Analytics - DDL Schema
Run first. Creates normalized SQL Server database objects with constraints.
*/

/*
USE master;
GO

ALTER DATABASE HealthcareCareAnalyticsDB
SET SINGLE_USER
WITH ROLLBACK IMMEDIATE;
GO

DROP DATABASE HealthcareCareAnalyticsDB;
GO
*/

IF DB_ID('HealthcareCareAnalyticsDB') IS NULL
BEGIN
    -- Creates the main SQL Server database used by MVC, API, EF Core, and ADO.NET.
    CREATE DATABASE HealthcareCareAnalyticsDB;
END
-- GO separates SQL batches for SQL Server Management Studio execution.
GO
-- Selects the healthcare database before creating or reading objects.
USE HealthcareCareAnalyticsDB;
-- GO separates SQL batches for SQL Server Management Studio execution.
GO

IF OBJECT_ID('dbo.AuditLogs','U') IS NOT NULL DROP TABLE dbo.AuditLogs;
IF OBJECT_ID('dbo.MedicalRecords','U') IS NOT NULL DROP TABLE dbo.MedicalRecords;
IF OBJECT_ID('dbo.Appointments','U') IS NOT NULL DROP TABLE dbo.Appointments;
IF OBJECT_ID('dbo.AppUsers','U') IS NOT NULL DROP TABLE dbo.AppUsers;
IF OBJECT_ID('dbo.AppRoles','U') IS NOT NULL DROP TABLE dbo.AppRoles;
IF OBJECT_ID('dbo.Doctors','U') IS NOT NULL DROP TABLE dbo.Doctors;
IF OBJECT_ID('dbo.Patients','U') IS NOT NULL DROP TABLE dbo.Patients;
IF OBJECT_ID('dbo.Departments','U') IS NOT NULL DROP TABLE dbo.Departments;
-- GO separates SQL batches for SQL Server Management Studio execution.
GO

-- Creates normalized table dbo.Departments with primary key, validations, and relationship-ready columns.
CREATE TABLE dbo.Departments (
    -- Primary key uniquely identifies each row and supports reliable relationships.
    Id INT IDENTITY(1,1) CONSTRAINT PK_Departments PRIMARY KEY,
    -- UNIQUE constraint/index prevents duplicate business values such as email or role name.
    Name NVARCHAR(100) NOT NULL CONSTRAINT UQ_Departments_Name UNIQUE,
    Description NVARCHAR(500) NULL,
    CreatedAtUtc DATETIME2 NOT NULL CONSTRAINT DF_Departments_Created DEFAULT SYSUTCDATETIME(),
    UpdatedAtUtc DATETIME2 NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_Departments_IsActive DEFAULT 1
);

-- Creates normalized table dbo.Patients with primary key, validations, and relationship-ready columns.
CREATE TABLE dbo.Patients (
    -- Primary key uniquely identifies each row and supports reliable relationships.
    Id INT IDENTITY(1,1) CONSTRAINT PK_Patients PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    -- UNIQUE constraint/index prevents duplicate business values such as email or role name.
    Email NVARCHAR(150) NOT NULL CONSTRAINT UQ_Patients_Email UNIQUE,
    PhoneNumber NVARCHAR(15) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Gender NVARCHAR(20) NOT NULL,
    Address NVARCHAR(500) NOT NULL,
    BloodGroup NVARCHAR(100) NULL,
    CreatedAtUtc DATETIME2 NOT NULL CONSTRAINT DF_Patients_Created DEFAULT SYSUTCDATETIME(),
    UpdatedAtUtc DATETIME2 NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_Patients_IsActive DEFAULT 1,
    -- CHECK constraint enforces allowed business values at database level.
    CONSTRAINT CK_Patients_Email CHECK (Email LIKE '%_@_%._%'),
    -- CHECK constraint enforces allowed business values at database level.
    CONSTRAINT CK_Patients_Phone CHECK (PhoneNumber NOT LIKE '%[^0-9]%' AND LEN(PhoneNumber) BETWEEN 10 AND 15),
    -- CHECK constraint enforces allowed business values at database level.
    CONSTRAINT CK_Patients_DOB CHECK (DateOfBirth < CAST(GETDATE() AS DATE)),
    -- CHECK constraint enforces allowed business values at database level.
    CONSTRAINT CK_Patients_Gender CHECK (Gender IN ('Male','Female','Other'))
);

-- Creates normalized table dbo.Doctors with primary key, validations, and relationship-ready columns.
CREATE TABLE dbo.Doctors (
    -- Primary key uniquely identifies each row and supports reliable relationships.
    Id INT IDENTITY(1,1) CONSTRAINT PK_Doctors PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Specialization NVARCHAR(100) NOT NULL,
    -- UNIQUE constraint/index prevents duplicate business values such as email or role name.
    Email NVARCHAR(150) NOT NULL CONSTRAINT UQ_Doctors_Email UNIQUE,
    PhoneNumber NVARCHAR(15) NOT NULL,
    DepartmentId INT NOT NULL,
    CreatedAtUtc DATETIME2 NOT NULL CONSTRAINT DF_Doctors_Created DEFAULT SYSUTCDATETIME(),
    UpdatedAtUtc DATETIME2 NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_Doctors_IsActive DEFAULT 1,
    -- Foreign key enforces referential integrity between related tables.
    CONSTRAINT FK_Doctors_Departments FOREIGN KEY (DepartmentId) REFERENCES dbo.Departments(Id),
    -- CHECK constraint enforces allowed business values at database level.
    CONSTRAINT CK_Doctors_Email CHECK (Email LIKE '%_@_%._%'),
    -- CHECK constraint enforces allowed business values at database level.
    CONSTRAINT CK_Doctors_Phone CHECK (PhoneNumber NOT LIKE '%[^0-9]%' AND LEN(PhoneNumber) BETWEEN 10 AND 15)
);

-- Creates normalized table dbo.AppRoles with primary key, validations, and relationship-ready columns.
CREATE TABLE dbo.AppRoles (
    -- Primary key uniquely identifies each row and supports reliable relationships.
    Id INT IDENTITY(1,1) CONSTRAINT PK_AppRoles PRIMARY KEY,
    -- UNIQUE constraint/index prevents duplicate business values such as email or role name.
    Name NVARCHAR(50) NOT NULL CONSTRAINT UQ_AppRoles_Name UNIQUE,
    CreatedAtUtc DATETIME2 NOT NULL CONSTRAINT DF_AppRoles_Created DEFAULT SYSUTCDATETIME(),
    UpdatedAtUtc DATETIME2 NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_AppRoles_IsActive DEFAULT 1
);

-- Creates normalized table dbo.AppUsers with primary key, validations, and relationship-ready columns.
CREATE TABLE dbo.AppUsers (
    -- Primary key uniquely identifies each row and supports reliable relationships.
    Id INT IDENTITY(1,1) CONSTRAINT PK_AppUsers PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    -- UNIQUE constraint/index prevents duplicate business values such as email or role name.
    Email NVARCHAR(150) NOT NULL CONSTRAINT UQ_AppUsers_Email UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    RoleId INT NOT NULL,
    CreatedAtUtc DATETIME2 NOT NULL CONSTRAINT DF_AppUsers_Created DEFAULT SYSUTCDATETIME(),
    UpdatedAtUtc DATETIME2 NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_AppUsers_IsActive DEFAULT 1,
    -- Foreign key enforces referential integrity between related tables.
    CONSTRAINT FK_AppUsers_AppRoles FOREIGN KEY (RoleId) REFERENCES dbo.AppRoles(Id),
    -- CHECK constraint enforces allowed business values at database level.
    CONSTRAINT CK_AppUsers_Email CHECK (Email LIKE '%_@_%._%')
);

-- Creates normalized table dbo.Appointments with primary key, validations, and relationship-ready columns.
CREATE TABLE dbo.Appointments (
    -- Primary key uniquely identifies each row and supports reliable relationships.
    Id INT IDENTITY(1,1) CONSTRAINT PK_Appointments PRIMARY KEY,
    PatientId INT NOT NULL,
    DoctorId INT NOT NULL,
    AppointmentDateTime DATETIME2 NOT NULL,
    DurationMinutes INT NOT NULL,
    Status INT NOT NULL,
    Reason NVARCHAR(500) NOT NULL,
    Notes NVARCHAR(1000) NULL,
    CreatedAtUtc DATETIME2 NOT NULL CONSTRAINT DF_Appointments_Created DEFAULT SYSUTCDATETIME(),
    UpdatedAtUtc DATETIME2 NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_Appointments_IsActive DEFAULT 1,
    -- Foreign key enforces referential integrity between related tables.
    CONSTRAINT FK_Appointments_Patients FOREIGN KEY (PatientId) REFERENCES dbo.Patients(Id),
    -- Foreign key enforces referential integrity between related tables.
    CONSTRAINT FK_Appointments_Doctors FOREIGN KEY (DoctorId) REFERENCES dbo.Doctors(Id),
    -- CHECK constraint enforces allowed business values at database level.
    CONSTRAINT CK_Appointments_Duration CHECK (DurationMinutes BETWEEN 15 AND 180),
    -- CHECK constraint enforces allowed business values at database level.
    CONSTRAINT CK_Appointments_Status CHECK (Status IN (1,2,3,4))
);

-- Creates normalized table dbo.MedicalRecords with primary key, validations, and relationship-ready columns.
CREATE TABLE dbo.MedicalRecords (
    -- Primary key uniquely identifies each row and supports reliable relationships.
    Id INT IDENTITY(1,1) CONSTRAINT PK_MedicalRecords PRIMARY KEY,
    PatientId INT NOT NULL,
    AppointmentId INT NULL,
    Diagnosis NVARCHAR(1000) NOT NULL,
    Prescription NVARCHAR(1000) NULL,
    VisitDate DATETIME2 NOT NULL,
    CreatedAtUtc DATETIME2 NOT NULL CONSTRAINT DF_MedicalRecords_Created DEFAULT SYSUTCDATETIME(),
    UpdatedAtUtc DATETIME2 NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_MedicalRecords_IsActive DEFAULT 1,
    -- Foreign key enforces referential integrity between related tables.
    CONSTRAINT FK_MedicalRecords_Patients FOREIGN KEY (PatientId) REFERENCES dbo.Patients(Id),
    -- Foreign key enforces referential integrity between related tables.
    CONSTRAINT FK_MedicalRecords_Appointments FOREIGN KEY (AppointmentId) REFERENCES dbo.Appointments(Id)
);

-- Creates normalized table dbo.AuditLogs with primary key, validations, and relationship-ready columns.
CREATE TABLE dbo.AuditLogs (
    -- Primary key uniquely identifies each row and supports reliable relationships.
    Id INT IDENTITY(1,1) CONSTRAINT PK_AuditLogs PRIMARY KEY,
    EntityName NVARCHAR(100) NOT NULL,
    EntityId INT NOT NULL,
    Action NVARCHAR(50) NOT NULL,
    PerformedBy NVARCHAR(150) NOT NULL,
    PerformedAtUtc DATETIME2 NOT NULL CONSTRAINT DF_AuditLogs_Performed DEFAULT SYSUTCDATETIME(),
    Details NVARCHAR(1000) NULL,
    AppointmentId INT NULL,
    CreatedAtUtc DATETIME2 NOT NULL CONSTRAINT DF_AuditLogs_Created DEFAULT SYSUTCDATETIME(),
    UpdatedAtUtc DATETIME2 NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_AuditLogs_IsActive DEFAULT 1,
    -- Foreign key enforces referential integrity between related tables.
    CONSTRAINT FK_AuditLogs_Appointments FOREIGN KEY (AppointmentId) REFERENCES dbo.Appointments(Id)
);
-- GO separates SQL batches for SQL Server Management Studio execution.
GO

CREATE INDEX IX_Appointments_Doctor_Time ON dbo.Appointments(DoctorId, AppointmentDateTime);
CREATE INDEX IX_Appointments_Patient_Time ON dbo.Appointments(PatientId, AppointmentDateTime);
CREATE INDEX IX_MedicalRecords_Patient ON dbo.MedicalRecords(PatientId);
-- GO separates SQL batches for SQL Server Management Studio execution.
GO
