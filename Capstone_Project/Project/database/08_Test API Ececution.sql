--Verify Appointment in SQL Server

USE HealthcareCareAnalyticsDB;

SELECT TOP 10 *
FROM Appointments
ORDER BY Id DESC;


--Verify Audit Log Trigger
USE HealthcareCareAnalyticsDB;

SELECT TOP 10 *
FROM AuditLogs
ORDER BY Id DESC;


--Appointment status update verified in SQL Server
USE HealthcareCareAnalyticsDB;
SELECT Id, Status
FROM Appointments
ORDER BY Id DESC;

