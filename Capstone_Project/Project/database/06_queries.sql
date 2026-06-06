-- DETAILED SQL COMMENT UPDATE
-- The comments below explain each database block for evaluation/demo.
-- No SQL statements or execution order were changed; only comments were added.

-- File: database/06_queries.sql
-- Layer: Database layer
-- Purpose: This file is the SQL query script containing joins, reports, and verification queries for capstone demonstration.
-- Security note: data access should use EF Core or parameterized SQL to avoid SQL injection.
-- Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

-- Selects the healthcare database before creating or reading objects.
USE HealthcareCareAnalyticsDB;
-- GO separates SQL batches for SQL Server Management Studio execution.
GO
-- 1. List appointment details with joins.
-- SELECT query retrieves data for report output, verification, or demo screenshots.
-- ORDER BY makes report output predictable and easier to read during demo.
SELECT * FROM dbo.vw_AppointmentDetails ORDER BY AppointmentDateTime DESC;

-- 2. Count appointments by status.
-- SELECT query retrieves data for report output, verification, or demo screenshots.
SELECT CASE Status WHEN 1 THEN 'Scheduled' WHEN 2 THEN 'Completed' WHEN 3 THEN 'Cancelled' ELSE 'NoShow' END AS StatusName,
       COUNT(*) AS AppointmentCount
FROM dbo.Appointments
-- GROUP BY aggregates rows for analytics such as appointment counts by department/status.
GROUP BY Status;

-- 3. Department-wise analytics report.
EXEC dbo.usp_GetDepartmentAppointmentReport;

-- 4. Patient appointment history.
DECLARE @PatientEmail NVARCHAR(150) = 'arun@example.com';
-- SELECT query retrieves data for report output, verification, or demo screenshots.
SELECT p.FullName, d.FullName AS DoctorName, a.AppointmentDateTime, a.Reason
FROM dbo.Appointments a
-- JOIN combines normalized tables so reports can show meaningful business information.
JOIN dbo.Patients p ON a.PatientId = p.Id
-- JOIN combines normalized tables so reports can show meaningful business information.
JOIN dbo.Doctors d ON a.DoctorId = d.Id
WHERE p.Email = @PatientEmail;

-- 5. Audit log review.
-- SELECT query retrieves data for report output, verification, or demo screenshots.
-- ORDER BY makes report output predictable and easier to read during demo.
SELECT TOP 20 * FROM dbo.AuditLogs ORDER BY PerformedAtUtc DESC;
-- GO separates SQL batches for SQL Server Management Studio execution.
GO
