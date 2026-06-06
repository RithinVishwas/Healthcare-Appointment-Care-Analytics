USE HealthcareCareAnalyticsDB;
GO

/* 
    STEP 1:
    Add the PatientId column to AppUsers only if it does not already exist.

    Why?
    The User Portal links each normal login user to one patient profile.
    Admin users do not need a patient profile, so PatientId is nullable.
*/
IF COL_LENGTH('dbo.AppUsers', 'PatientId') IS NULL
BEGIN
    ALTER TABLE dbo.AppUsers
    ADD PatientId INT NULL;

    PRINT 'PatientId column added to AppUsers table.';
END
ELSE
BEGIN
    PRINT 'PatientId column already exists in AppUsers table.';
END
GO


/*
    STEP 2:
    Add foreign key only if it does not already exist.

    Why?
    This makes sure AppUsers.PatientId points to a valid Patients.Id.
    It keeps the database normalized and prevents invalid patient links.
*/
IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_AppUsers_Patients'
)
BEGIN
    ALTER TABLE dbo.AppUsers
    ADD CONSTRAINT FK_AppUsers_Patients
    FOREIGN KEY (PatientId)
    REFERENCES dbo.Patients(Id)
    ON DELETE SET NULL;

    PRINT 'Foreign key FK_AppUsers_Patients added.';
END
ELSE
BEGIN
    PRINT 'Foreign key FK_AppUsers_Patients already exists.';
END
GO


/*
    STEP 3:
    Make sure the User role exists.

    Why?
    New registered users must be assigned to User role.
*/
IF NOT EXISTS (
    SELECT 1
    FROM dbo.AppRoles
    WHERE Name = 'User'
)
BEGIN
    INSERT INTO dbo.AppRoles (Name, CreatedAtUtc, IsActive)
    VALUES ('User', SYSUTCDATETIME(), 1);

    PRINT 'User role added.';
END
ELSE
BEGIN
    PRINT 'User role already exists.';
END
GO


/*
    STEP 4:
    Verify the column is now available.
*/
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'AppUsers'
ORDER BY ORDINAL_POSITION;
GO

/*
USE HealthcareCareAnalyticsDB;

SELECT * FROM AppUsers;
SELECT * FROM Patients;
SELECT * FROM Appointments;
SELECT * FROM AuditLogs;
*/