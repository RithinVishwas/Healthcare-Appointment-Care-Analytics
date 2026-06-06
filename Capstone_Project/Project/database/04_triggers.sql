-- DETAILED SQL COMMENT UPDATE
-- The comments below explain each database block for evaluation/demo.
-- No SQL statements or execution order were changed; only comments were added.

-- File: database/04_triggers.sql
-- Layer: Database layer
-- Purpose: This file is the SQL trigger script used for automated auditing and data integrity support.
-- Security note: data access should use EF Core or parameterized SQL to avoid SQL injection.
-- Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

-- Selects the healthcare database before creating or reading objects.
USE HealthcareCareAnalyticsDB;
-- GO separates SQL batches for SQL Server Management Studio execution.
GO
CREATE OR ALTER TRIGGER dbo.trg_Appointments_Audit_Insert
ON dbo.Appointments
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    -- Inserts seed/sample data used for demo, testing, and report generation.
    INSERT INTO dbo.AuditLogs(EntityName, EntityId, Action, PerformedBy, Details, AppointmentId)
    -- SELECT query retrieves data for report output, verification, or demo screenshots.
    SELECT 'Appointment', Id, 'INSERT', 'SQL_TRIGGER', CONCAT('Appointment created for PatientId=', PatientId, ', DoctorId=', DoctorId), Id
    FROM inserted;
END;
-- GO separates SQL batches for SQL Server Management Studio execution.
GO

CREATE OR ALTER TRIGGER dbo.trg_Appointments_Audit_Update
ON dbo.Appointments
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    -- Inserts seed/sample data used for demo, testing, and report generation.
    INSERT INTO dbo.AuditLogs(EntityName, EntityId, Action, PerformedBy, Details, AppointmentId)
    -- SELECT query retrieves data for report output, verification, or demo screenshots.
    SELECT 'Appointment', i.Id, 'UPDATE', 'SQL_TRIGGER', CONCAT('Status changed from ', d.Status, ' to ', i.Status), i.Id
    FROM inserted i
    -- JOIN combines normalized tables so reports can show meaningful business information.
    JOIN deleted d ON i.Id = d.Id
    WHERE i.Status <> d.Status;
END;
-- GO separates SQL batches for SQL Server Management Studio execution.
GO
