-- DETAILED SQL COMMENT UPDATE
-- The comments below explain each database block for evaluation/demo.
-- No SQL statements or execution order were changed; only comments were added.

-- File: database/02_dml_seed_data.sql
-- Layer: Database layer
-- Purpose: This file is the DML seed script that inserts sample users, roles, patients, doctors, and appointments for demo/testing.
-- Security note: data access should use EF Core or parameterized SQL to avoid SQL injection.
-- Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

-- Selects the healthcare database before creating or reading objects.
USE HealthcareCareAnalyticsDB;
-- GO separates SQL batches for SQL Server Management Studio execution.
GO
-- Inserts seed/sample data used for demo, testing, and report generation.
INSERT INTO dbo.Departments (Name, Description) VALUES
('General Medicine', 'Primary care and consultation'),
('Cardiology', 'Heart care and treatment'),
('Orthopedics', 'Bone and joint care');

-- Inserts seed/sample data used for demo, testing, and report generation.
INSERT INTO dbo.Doctors (FullName, Specialization, Email, PhoneNumber, DepartmentId) VALUES
('Dr. Ananya Rao', 'General Physician', 'ananya.rao@healthcare.local', '9876543210', 1),
('Dr. Karthik Menon', 'Cardiologist', 'karthik.menon@healthcare.local', '9876543211', 2),
('Dr. Priya Shah', 'Orthopedic Surgeon', 'priya.shah@healthcare.local', '9876543212', 3);

-- Inserts seed/sample data used for demo, testing, and report generation.
INSERT INTO dbo.Patients (FullName, Email, PhoneNumber, DateOfBirth, Gender, Address, BloodGroup) VALUES
('Arun Kumar', 'arun@example.com', '9000000001', '1997-05-10', 'Male', 'Madurai', 'O+'),
('Meera Nair', 'meera@example.com', '9000000002', '1995-08-21', 'Female', 'Chennai', 'A+');

-- Inserts seed/sample data used for demo, testing, and report generation.
INSERT INTO dbo.AppRoles (Name) VALUES ('Admin'), ('Doctor'), ('Staff');

-- The application DbInitializer replaces this demo hash with a PBKDF2 hash for Admin@123.
-- Inserts seed/sample data used for demo, testing, and report generation.
INSERT INTO dbo.AppUsers (FullName, Email, PasswordHash, RoleId) VALUES
('System Administrator', 'admin@healthcare.local', '10000:DemoSaltOnlyForSeed:DemoHashChangeUsingDbInitializer', 1);

-- Inserts seed/sample data used for demo, testing, and report generation.
INSERT INTO dbo.Appointments (PatientId, DoctorId, AppointmentDateTime, DurationMinutes, Status, Reason, Notes) VALUES
(1, 1, '2026-07-10T10:00:00', 30, 1, 'Fever and headache', 'Initial consultation'),
-- CHECK constraint enforces allowed business values at database level.
(2, 2, '2026-07-11T11:00:00', 45, 2, 'Routine heart checkup', 'ECG recommended');
-- GO separates SQL batches for SQL Server Management Studio execution.
GO
