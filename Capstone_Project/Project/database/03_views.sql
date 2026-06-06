-- DETAILED SQL COMMENT UPDATE
-- The comments below explain each database block for evaluation/demo.
-- No SQL statements or execution order were changed; only comments were added.

-- File: database/03_views.sql
-- Layer: Database layer
-- Purpose: This file is the SQL views script used to simplify reporting queries and care analytics output.
-- Security note: data access should use EF Core or parameterized SQL to avoid SQL injection.
-- Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

-- Selects the healthcare database before creating or reading objects.
USE HealthcareCareAnalyticsDB;
-- GO separates SQL batches for SQL Server Management Studio execution.
GO
CREATE OR ALTER VIEW dbo.vw_AppointmentDetails AS
-- SELECT query retrieves data for report output, verification, or demo screenshots.
SELECT a.Id AS AppointmentId, p.FullName AS PatientName, d.FullName AS DoctorName, dep.Name AS DepartmentName,
       a.AppointmentDateTime, a.DurationMinutes,
       CASE a.Status WHEN 1 THEN 'Scheduled' WHEN 2 THEN 'Completed' WHEN 3 THEN 'Cancelled' ELSE 'NoShow' END AS AppointmentStatus,
       a.Reason
FROM dbo.Appointments a
-- JOIN combines normalized tables so reports can show meaningful business information.
JOIN dbo.Patients p ON a.PatientId = p.Id
-- JOIN combines normalized tables so reports can show meaningful business information.
JOIN dbo.Doctors d ON a.DoctorId = d.Id
-- JOIN combines normalized tables so reports can show meaningful business information.
JOIN dbo.Departments dep ON d.DepartmentId = dep.Id;
-- GO separates SQL batches for SQL Server Management Studio execution.
GO

CREATE OR ALTER VIEW dbo.vw_DailyAppointmentSummary AS
-- SELECT query retrieves data for report output, verification, or demo screenshots.
SELECT CAST(AppointmentDateTime AS DATE) AS AppointmentDate,
       COUNT(*) AS TotalAppointments,
       SUM(CASE WHEN Status = 1 THEN 1 ELSE 0 END) AS ScheduledAppointments,
       SUM(CASE WHEN Status = 2 THEN 1 ELSE 0 END) AS CompletedAppointments,
       SUM(CASE WHEN Status = 3 THEN 1 ELSE 0 END) AS CancelledAppointments
FROM dbo.Appointments
-- GROUP BY aggregates rows for analytics such as appointment counts by department/status.
GROUP BY CAST(AppointmentDateTime AS DATE);
-- GO separates SQL batches for SQL Server Management Studio execution.
GO
