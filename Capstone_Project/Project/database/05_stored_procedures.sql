-- DETAILED SQL COMMENT UPDATE
-- The comments below explain each database block for evaluation/demo.
-- No SQL statements or execution order were changed; only comments were added.

-- File: database/05_stored_procedures.sql
-- Layer: Database layer
-- Purpose: This file is the stored procedure script used to encapsulate reusable database reporting operations.
-- Security note: data access should use EF Core or parameterized SQL to avoid SQL injection.
-- Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

-- Selects the healthcare database before creating or reading objects.
USE HealthcareCareAnalyticsDB;
-- GO separates SQL batches for SQL Server Management Studio execution.
GO
CREATE OR ALTER PROCEDURE dbo.usp_GetDepartmentAppointmentReport
AS
BEGIN
    SET NOCOUNT ON;
    -- SELECT query retrieves data for report output, verification, or demo screenshots.
    SELECT dep.Name AS DepartmentName,
           COUNT(a.Id) AS TotalAppointments,
           SUM(CASE WHEN a.Status = 2 THEN 1 ELSE 0 END) AS CompletedAppointments,
           SUM(CASE WHEN a.Status = 1 THEN 1 ELSE 0 END) AS ScheduledAppointments
    FROM dbo.Departments dep
    -- JOIN combines normalized tables so reports can show meaningful business information.
    LEFT JOIN dbo.Doctors d ON d.DepartmentId = dep.Id
    -- JOIN combines normalized tables so reports can show meaningful business information.
    LEFT JOIN dbo.Appointments a ON a.DoctorId = d.Id
    -- GROUP BY aggregates rows for analytics such as appointment counts by department/status.
    GROUP BY dep.Name
    -- ORDER BY makes report output predictable and easier to read during demo.
    ORDER BY dep.Name;
END;
-- GO separates SQL batches for SQL Server Management Studio execution.
GO

CREATE OR ALTER PROCEDURE dbo.usp_GetDoctorAvailability
    @DoctorId INT,
    @FromDate DATETIME2,
    @ToDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    -- SELECT query retrieves data for report output, verification, or demo screenshots.
    SELECT Id, AppointmentDateTime, DurationMinutes, Status
    FROM dbo.Appointments
    WHERE DoctorId = @DoctorId
      AND AppointmentDateTime >= @FromDate
      AND AppointmentDateTime < @ToDate
      AND Status = 1
    -- ORDER BY makes report output predictable and easier to read during demo.
    ORDER BY AppointmentDateTime;
END;
-- GO separates SQL batches for SQL Server Management Studio execution.
GO
