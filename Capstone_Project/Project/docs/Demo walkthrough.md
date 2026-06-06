# Final Demo — End-to-End Walkthrough

Project: **Healthcare Appointment & Care Analytics**
Audience: Client SME Panel / Capstone Evaluation Panel

This walkthrough demonstrates the complete application flow from **MVC UI → Controller → Service Layer → Repository / EF Core → SQL Server Database**, along with API testing, JWT authentication, reporting, audit logging, and security validation.

---

## 0. Pre-Demo Checklist

Before starting the demo, make sure the following are ready:

* [ ] SQL Server is running.
* [ ] Database `HealthcareCareAnalyticsDB` is created.
* [ ] Required SQL scripts are executed in order:

```text
01_ddl_schema.sql
02_dml_seed_data.sql
03_views.sql
04_triggers.sql
05_stored_procedures.sql
06_queries.sql
07_user_portal_update.sql
```

* [ ] MVC connection string is configured in:

```text
src/Healthcare.MVC/appsettings.json
```

* [ ] API connection string is configured in:

```text
src/Healthcare.API/appsettings.json
```

* [ ] MVC application is running:

```bash
dotnet run --project src/Healthcare.MVC
```

* [ ] API application is running:

```bash
dotnet run --project src/Healthcare.API
```

* [ ] Swagger is reachable at:

```text
https://localhost:#######/swagger
```

* [ ] Admin login is ready:

```text
Email: admin@healthcare.local
Password: Admin@123
```

* [ ] SSMS / SQL Server query window is open for database verification.
* [ ] Screenshots or diagrams are ready for architecture, DFD, ER diagram, and report flow.

---

## 1. Problem Framing

Start by explaining the business problem.

A healthcare provider needs a secure and organized platform to manage patient appointments, doctor availability, patient profiles, and care analytics. Manual appointment tracking can lead to missed appointments, duplicate bookings, poor visibility, and limited reporting.

This project solves the problem by providing:

* Admin dashboard for healthcare staff.
* Patient portal for registered users.
* Secure appointment booking.
* JWT-secured Web API for mobile or external clients.
* SQL Server database with audit logs and reports.
* Layered architecture suitable for future Azure deployment.

---

## 2. Architecture Explanation

Open the architecture diagram and explain the system layers.

The project follows a layered architecture:

```text
Browser / Client
        ↓
ASP.NET Core MVC Portal
        ↓
Controllers
        ↓
Service Layer
        ↓
Repository / Unit of Work
        ↓
EF Core / ADO.NET
        ↓
MS SQL Server Database
```

Also explain the API flow:

```text
Swagger / Postman / Mobile Client
        ↓
Healthcare.API
        ↓
JWT Authentication
        ↓
API Controllers
        ↓
DTO Validation
        ↓
Service Layer
        ↓
Repository / EF Core
        ↓
SQL Server
```

Mention the main projects:

```text
Healthcare.Core
Healthcare.Infrastructure
Healthcare.MVC
Healthcare.API
Healthcare.Tests
```

Explain that:

* `Healthcare.Core` contains entities, DTOs, interfaces, and business contracts.
* `Healthcare.Infrastructure` contains DbContext, repositories, services, JWT service, and password hashing.
* `Healthcare.MVC` contains Razor UI, controllers, ViewModels, and user workflows.
* `Healthcare.API` exposes REST endpoints protected using JWT.
* `Healthcare.Tests` is used for unit testing and business rule validation.

---

## 3. Database and ER Diagram Explanation

Open the ER diagram and explain the main database design.

Main tables:

```text
Departments
Doctors
Patients
Appointments
MedicalRecords
AuditLogs
AppUsers
AppRoles
```

Important relationships:

```text
One Department has many Doctors.
One Doctor can have many Appointments.
One Patient can book many Appointments.
One Patient can have many MedicalRecords.
One Appointment can have many AuditLogs.
One AppRole can be assigned to many AppUsers.
One AppUser may be linked to one Patient profile.
```

Explain the purpose of the database:

* Store normalized healthcare records.
* Avoid data duplication.
* Maintain referential integrity using foreign keys.
* Support dashboard reports using SQL views and stored procedures.
* Track appointment changes using SQL triggers and the `AuditLogs` table.

---

## 4. Admin Flow

Open the MVC application in the browser.

### Step 1 — Login as Admin

Go to the login page and enter:

```text
Email: admin@healthcare.local
Password: Admin@123
```

After login, show the Admin Dashboard.

Explain:

The admin dashboard displays appointment and healthcare analytics such as total patients, total doctors, total appointments, upcoming appointments, and status-based summaries.

---

### Step 2 — Show Patient Management

Open the **Patients** page.

Explain:

The patient management page displays patient records stored in SQL Server. Admin can create and manage patient profiles.

Create a sample patient:

```text
Full Name: Demo Patient
Email: demopatient@example.com
Phone Number: #######
Date of Birth: 2002-05-10
Gender: Male
Address: #######
Blood Group: O+
```

After submitting, show that the patient appears in the patient list.

Verify in SQL Server:

```sql
USE HealthcareCareAnalyticsDB;

SELECT TOP 10 *
FROM Patients
ORDER BY Id DESC;
```

Explain:

This proves the data flow from MVC UI to SQL Server database.

---

### Step 3 — Show Appointment Management

Open the **Appointments** page.

Create a new appointment by selecting:

```text
Patient: Demo Patient
Doctor: Any available doctor
Appointment Date/Time: Future date and time
Duration: 30 minutes
Reason: General health checkup
```

After submitting, show the appointment list.

Verify in SQL Server:

```sql
USE HealthcareCareAnalyticsDB;

SELECT TOP 10 *
FROM Appointments
ORDER BY Id DESC;
```

Explain:

The appointment is saved through the MVC Controller, Service Layer, Repository, EF Core, and SQL Server.

---

### Step 4 — Show Audit Log

Open SSMS and run:

```sql
USE HealthcareCareAnalyticsDB;

SELECT TOP 10 *
FROM AuditLogs
ORDER BY Id DESC;
```

Explain:

The `AuditLogs` table is updated through SQL triggers when appointment operations happen. This supports traceability and auditability.

---

## 5. Patient/User Portal Flow

### Step 1 — Logout from Admin

Logout from the admin account.

### Step 2 — Register New User

Open the registration page and create a new user:

```text
Full Name: Demo User
Email: demouser@example.com
Password: User@12345
Confirm Password: User@12345
```

Explain:

New users are assigned the `User` role by default. They cannot access admin pages.

Verify in SQL Server:

```sql
USE HealthcareCareAnalyticsDB;

SELECT 
    u.Id,
    u.FullName,
    u.Email,
    u.PatientId,
    r.Name AS RoleName
FROM AppUsers u
INNER JOIN AppRoles r ON u.RoleId = r.Id
ORDER BY u.Id DESC;
```

At this stage, `PatientId` may be `NULL` because the user has registered but has not created a patient profile yet.

---

### Step 3 — Login as User

Login using:

```text
Email: demouser@example.com
Password: User@12345
```

Show the Patient Portal Dashboard.

Explain:

Normal users are redirected to the patient portal, not the admin dashboard.

---

### Step 4 — Create Patient Profile

In the patient portal, click **Create Patient Profile**.

Enter:

```text
Full Name: Demo User
Email: demouser@example.com
Phone Number: #######
Date of Birth: 2002-05-10
Gender: Male
Address: #######
Blood Group: B+
```

After saving, verify the user-patient link:

```sql
USE HealthcareCareAnalyticsDB;

SELECT 
    u.Id AS UserId,
    u.FullName AS UserName,
    u.Email AS UserEmail,
    u.PatientId,
    p.FullName AS PatientName,
    p.Email AS PatientEmail
FROM AppUsers u
LEFT JOIN Patients p ON u.PatientId = p.Id
WHERE u.Email = 'demouser@example.com';
```

Explain:

The `AppUsers.PatientId` field links the logged-in user account to the created patient profile.

---

### Step 5 — Book Appointment as User

In the patient portal, click **Book Appointment**.

Select:

```text
Doctor: Any available doctor
Appointment Date/Time: Future date and time
Duration: 30 minutes
Reason: Fever and general consultation
```

Submit the form.

Open **My Appointments**.

Explain:

The user can view only their own appointments. This demonstrates role-based and user-specific access control.

Verify in SQL Server:

```sql
USE HealthcareCareAnalyticsDB;

SELECT TOP 10
    a.Id,
    a.PatientId,
    p.FullName AS PatientName,
    a.DoctorId,
    d.FullName AS DoctorName,
    a.AppointmentDateTime,
    a.DurationMinutes,
    a.Status,
    a.Reason
FROM Appointments a
INNER JOIN Patients p ON a.PatientId = p.Id
INNER JOIN Doctors d ON a.DoctorId = d.Id
ORDER BY a.Id DESC;
```

---

## 6. API and JWT Security Flow

Open Swagger:

```text
https://localhost:#######/swagger
```

### Step 1 — Login API

Open:

```text
POST /api/Auth/login
```

Use:

```json
{
  "email": "admin@healthcare.local",
  "password": "Admin@123"
}
```

Execute the request.

Explain:

The API validates the credentials and returns a JWT token.

---

### Step 2 — Authorize Swagger

Copy the JWT token.

Click **Authorize** in Swagger and enter:

```text
Bearer <JWT token>
```

Explain:

Protected API endpoints require this Bearer token in the Authorization header.

---

### Step 3 — Test Patients API

Call:

```text
GET /api/Patients
```

Expected output:

```text
200 OK
```

Explain:

The API returns patient data from SQL Server only after token validation.

---

### Step 4 — Test Doctors API

Call:

```text
GET /api/Doctors
```

Expected output:

```text
200 OK
```

Explain:

The Doctors API provides doctor IDs and specialization details required for appointment booking.

---

### Step 5 — Test Appointments API

Call:

```text
GET /api/Appointments
```

Expected output:

```text
200 OK
```

Then create an appointment:

```text
POST /api/Appointments
```

Sample request:

```json
{
  "patientId": 1,
  "doctorId": 1,
  "appointmentDateTime": "2026-06-10T11:00:00",
  "durationMinutes": 30,
  "reason": "General health checkup"
}
```

Expected output:

```text
201 Created
```

Verify the result in SQL Server:

```sql
USE HealthcareCareAnalyticsDB;

SELECT TOP 10 *
FROM Appointments
ORDER BY Id DESC;
```

---

### Step 6 — Test Unauthorized Access

In Swagger, click **Authorize**, then **Logout**.

Now call:

```text
GET /api/Appointments
```

Expected output:

```text
401 Unauthorized
```

Explain:

This proves that protected API endpoints cannot be accessed without a valid JWT token.

---

## 7. Reports and Analytics Flow

Login again as Admin in the MVC portal.

Open:

```text
Reports
```

or

```text
Analytics
```

Explain:

Reports are generated using SQL views, stored procedures, and ADO.NET service classes.

Report generation flow:

```text
Reports Page
        ↓
ReportsController
        ↓
SqlAnalyticsReportService
        ↓
ADO.NET
        ↓
Stored Procedure / SQL View
        ↓
SQL Server
        ↓
Razor View output
```

Verify SQL views:

```sql
USE HealthcareCareAnalyticsDB;

SELECT * FROM vw_AppointmentDetails;
```

or:

```sql
SELECT * FROM vw_DoctorWorkload;
```

Explain:

This demonstrates reporting from the database layer to the MVC UI.

---

## 8. Security Explanation

Explain the major security controls used in the project.

### SQL Injection Protection

The application uses EF Core LINQ queries and parameterized ADO.NET commands. User input is not directly concatenated into SQL queries.

### CSRF Protection

MVC forms use anti-forgery token validation. POST actions are protected using anti-forgery filters and tokens.

### XSS Protection

Razor views automatically encode output. User-entered values are displayed as text, not executable JavaScript.

### Authentication

MVC uses cookie authentication. API uses JWT Bearer authentication.

### Authorization

Admin and User access is separated using role-based authorization policies.

### Password Protection

Passwords are stored using PBKDF2 hashing with salt. Plain text passwords are not stored.

### Audit Logging

Appointment insert/update operations are recorded using SQL triggers and the `AuditLogs` table.

---

## 9. DevOps and AI Modernization

Show the DevOps assets if included:

```text
Dockerfile
docker-compose.yml
.github/workflows/ci.yml
copilot_prompts.txt
```

Explain:

The project is designed for future cloud deployment. MVC and API can be hosted separately on Azure App Service. SQL Server can be migrated to Azure SQL Database. GitHub Actions can automate restore, build, test, and deployment.

AI modernization can be added in the future through a conceptual AI/MCP endpoint for:

* Appointment summary
* Patient visit summary
* Doctor preparation note
* Care follow-up recommendation

---

## 10. Q&A Readiness

Be prepared to answer these questions:

### How is SQL injection prevented?

Through EF Core LINQ queries, parameterized ADO.NET commands, and avoiding raw string-concatenated SQL.

### How is CSRF handled?

MVC forms use anti-forgery tokens and server-side validation filters.

### How is XSS prevented?

Razor automatically HTML-encodes output, and the project avoids unsafe raw HTML rendering.

### How does JWT authentication work?

The user logs in through the API, receives a signed JWT token, and sends it in the Authorization header as a Bearer token.

### What is the Repository Pattern?

It separates database access logic from controllers and services.

### What is Unit of Work?

It coordinates multiple repository operations and commits changes using one `SaveChanges` call.

### How are appointments validated?

The appointment service checks required fields, patient existence, doctor existence, appointment time, and duplicate doctor time slots.

### How does audit logging work?

SQL triggers insert records into `AuditLogs` when appointment operations occur.

### Why use layered architecture?

It improves maintainability, testability, separation of concerns, and scalability.

### How can this project be deployed?

MVC and API can be deployed to Azure App Service, the database to Azure SQL, and CI/CD can be handled using GitHub Actions.

---

## 11. Closing Statement

This project demonstrates a secure, layered, and database-driven healthcare appointment platform. It includes MVC user workflows, JWT-secured APIs, SQL Server database design, reporting, audit logging, validation, and role-based access control.

The system is suitable for a healthcare appointment management scenario and is designed with future enhancements such as Azure deployment, AI-assisted summaries, and advanced analytics.
