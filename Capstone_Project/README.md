# Healthcare Appointment & Care Analytics

Capstone project — **ASP.NET Core MVC + Web API + EF Core + ADO.NET + MS SQL Server + JWT Authentication**.

A secure, layered healthcare platform for managing **patients, doctors, appointments, user registration, patient portal access, analytics reports, audit logging, and JWT-secured API testing**.

---

## Project Overview

**Healthcare Appointment & Care Analytics** is a full-stack .NET healthcare management platform designed to improve appointment handling and care delivery.

The application provides two main interfaces:

* **MVC Web Portal** — for Admin and Patient/User workflows.
* **Web API** — for Swagger/Postman/mobile clients using JWT authentication.

The system supports:

* Admin dashboard
* User registration and login
* Patient profile creation
* Appointment booking
* Doctor listing
* Appointment status tracking
* Reports and analytics
* JWT-secured REST APIs
* SQL Server audit logging using triggers

---

## Business Objective

The objective of this project is to improve healthcare appointment management by providing a secure and structured platform where:

* Patients can register, create profiles, and book appointments.
* Admins can manage patients, doctors, appointments, and reports.
* APIs can be consumed by Swagger, Postman, or future mobile applications.
* SQL Server stores normalized healthcare data with audit tracking.
* Reports help monitor appointment and doctor workload analytics.

---

## Solution Projects

The solution follows a clean layered architecture.

```text
Healthcare Appointment & Care Analytics
├── src
│   ├── Healthcare.Core
│   ├── Healthcare.Infrastructure
│   ├── Healthcare.API
│   ├── Healthcare.MVC
│   └── Healthcare.Tests
│
├── database
├── docs
├── devops
└── README.md
```

### Project Responsibilities

| Project                     | Purpose                                                                      |
| --------------------------- | ---------------------------------------------------------------------------- |
| `Healthcare.Core`           | Entities, DTOs, interfaces, enums, domain contracts                          |
| `Healthcare.Infrastructure` | EF Core DbContext, repositories, services, password hashing, JWT service     |
| `Healthcare.API`            | REST API, Swagger, JWT authentication, API controllers                       |
| `Healthcare.MVC`            | MVC portal, Razor views, controllers, ViewModels, user interface             |
| `Healthcare.Tests`          | Unit tests and business rule validation tests                                |
| `database`                  | SQL scripts for schema, seed data, views, triggers, stored procedures        |
| `devops`                    | Docker, Docker Compose, GitHub Actions CI/CD assets                          |
| `docs`                      | Architecture diagrams, DFD, ER diagram, database design, final documentation |

---

## Key Features

### Admin Features

* Login using seeded admin account
* View dashboard analytics
* Manage patients
* View doctors
* Manage appointments
* Update appointment status
* View reports and analytics
* Verify data stored in SQL Server

### Patient/User Features

* Register as a new user
* Login to patient portal
* Create patient profile
* Book appointment
* View own appointments
* Access only personal appointment records

### API Features

* JWT-secured login
* Swagger/OpenAPI documentation
* Patients API
* Doctors API
* Appointments API
* Analytics API
* Bearer token authorization
* Protected endpoint testing

---

## Technology Stack

| Layer                | Technology                                                            |
| -------------------- | --------------------------------------------------------------------- |
| Frontend             | ASP.NET Core MVC, Razor Views, HTML, CSS, AJAX-ready flow             |
| Backend API          | ASP.NET Core Web API                                                  |
| Programming Language | C#                                                                    |
| Database             | Microsoft SQL Server                                                  |
| ORM                  | Entity Framework Core                                                 |
| Raw SQL Reporting    | ADO.NET, Stored Procedures, SQL Views                                 |
| Authentication       | Cookie Authentication for MVC, JWT Bearer Authentication for API      |
| Authorization        | Role-based Authorization                                              |
| Security             | CSRF protection, input validation, password hashing, security headers |
| API Documentation    | Swagger / OpenAPI                                                     |
| Testing              | xUnit / unit testing structure                                        |
| DevOps               | Docker, Docker Compose, GitHub Actions concept                        |
| Cloud Concept        | Azure App Service, Azure SQL Database, Azure Container Registry       |

---

## Database

Database name used:

```text
HealthcareCareAnalyticsDB
```

### Main Tables

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

### SQL Features Used

* Primary keys
* Foreign keys
* Unique constraints
* Check constraints
* Indexes
* Views
* Triggers
* Stored procedures
* Seed data
* User portal update script

### SQL Script Execution Order

Run the SQL scripts in this order if you are setting up the database manually:

```text
1. 01_ddl_schema.sql
2. 02_dml_seed_data.sql
3. 03_views.sql
4. 04_triggers.sql
5. 05_stored_procedures.sql
6. 06_queries.sql
7. 07_user_portal_update.sql
```

---

## Default Login Credentials

### Admin Login

```text
Email: admin@healthcare.local
Password: Admin@123
```

### New User Login

A normal user can be created from the MVC login page using the **Register** option.

Example:

```text
Full Name: Test User
Email: testuser@gmail.com
Password: User@12345
```

After registration, the user can log in, create a patient profile, and book appointments.

---

## Connection String

Update the connection string in both files:

```text
src/Healthcare.MVC/appsettings.json
src/Healthcare.API/appsettings.json
```

Example for SQL Server Express:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server= ***********************************************************************************************************;"
  }
}
```

If your SQL Server instance name is different, replace:

```text
#####################################
```

with your actual SQL Server instance name.

---

## Run Locally

### Prerequisites

Install:

* .NET SDK
* SQL Server / SQL Server Express
* SQL Server Management Studio
* Visual Studio or VS Code

---

### 1. Restore Packages

From the project root:

```bash
dotnet restore
```

---

### 2. Build the Solution

```bash
dotnet build
```

Expected output:

```text
Build succeeded.
```

---

### 3. Run MVC Application

```bash
dotnet run --project src/Healthcare.MVC
```

Open the localhost URL shown in terminal.

Example:

```text
https://localhost:your-mvc-port
```

---

### 4. Run Web API

Open another terminal and run:

```bash
dotnet run --project src/Healthcare.API
```

Open Swagger:

```text
https://localhost:your-api-port/swagger
```

Example:

```text
https://localhost:58295/swagger
```

---

## Demo Flow

### MVC Demo Flow

```text
1. Run Healthcare.MVC
2. Open login page
3. Login as Admin
4. Show Admin Dashboard
5. View Patients
6. Create Patient
7. View Appointments
8. Book Appointment
9. Verify Patients and Appointments in SQL Server
10. Logout
11. Register as New User
12. Login as User
13. Create Patient Profile
14. Book Appointment
15. View My Appointments
16. Verify AppUsers, Patients, Appointments, and AuditLogs in SQL Server
```

---

### API Demo Flow

```text
1. Run Healthcare.API
2. Open Swagger
3. Test POST /api/Auth/login
4. Copy JWT token
5. Click Authorize
6. Paste Bearer token
7. Test GET /api/Patients
8. Test GET /api/Doctors
9. Test GET /api/Appointments
10. Test POST /api/Appointments
11. Test GET /api/Appointments/upcoming
12. Test PATCH /api/Appointments/{id}/status/{status}
13. Verify Appointments table in SQL Server
14. Verify AuditLogs table in SQL Server
15. Logout Swagger token and confirm protected endpoint returns 401 Unauthorized
```

---

## API Endpoints

### Authentication

| Method | Endpoint          | Auth | Description                  |
| ------ | ----------------- | ---- | ---------------------------- |
| POST   | `/api/Auth/login` | No   | Login and generate JWT token |

### Patients

| Method | Endpoint             | Auth         | Description       |
| ------ | -------------------- | ------------ | ----------------- |
| GET    | `/api/Patients`      | JWT Required | Get all patients  |
| POST   | `/api/Patients`      | JWT Required | Create patient    |
| GET    | `/api/Patients/{id}` | JWT Required | Get patient by ID |
| PUT    | `/api/Patients/{id}` | JWT Required | Update patient    |

### Doctors

| Method | Endpoint            | Auth         | Description      |
| ------ | ------------------- | ------------ | ---------------- |
| GET    | `/api/Doctors`      | JWT Required | Get all doctors  |
| GET    | `/api/Doctors/{id}` | JWT Required | Get doctor by ID |

### Appointments

| Method | Endpoint                                 | Auth         | Description               |
| ------ | ---------------------------------------- | ------------ | ------------------------- |
| GET    | `/api/Appointments`                      | JWT Required | Get all appointments      |
| POST   | `/api/Appointments`                      | JWT Required | Create appointment        |
| GET    | `/api/Appointments/upcoming`             | JWT Required | Get upcoming appointments |
| GET    | `/api/Appointments/{id}`                 | JWT Required | Get appointment by ID     |
| PATCH  | `/api/Appointments/{id}/status/{status}` | JWT Required | Update appointment status |

### Analytics

| Method | Endpoint                         | Auth         | Description                   |
| ------ | -------------------------------- | ------------ | ----------------------------- |
| GET    | `/api/Analytics/summary`         | JWT Required | Get dashboard summary         |
| GET    | `/api/Analytics/doctor-workload` | JWT Required | Get doctor workload analytics |

---

## Sample API Login Request

Endpoint:

```text
POST /api/Auth/login
```

Request body:

```json
{
  "email": "admin@healthcare.local",
  "password": "Admin@123"
}
```

Response:

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6...",
  "email": "admin@healthcare.local",
  "role": "Admin"
}
```

Use the token in Swagger Authorize:

```text
Bearer your_token_here
```

---

## Sample Appointment API Request

Endpoint:

```text
POST /api/Appointments
```

Request body:

```json
{
  "patientId": 1,
  "doctorId": 1,
  "appointmentDateTime": "2026-06-10T11:00:00",
  "durationMinutes": 30,
  "reason": "General health checkup"
}
```

Expected response:

```text
201 Created
```

---

## Security Implementation

The application includes multiple security controls.

| Security Concern  | Implementation                                                       |
| ----------------- | -------------------------------------------------------------------- |
| SQL Injection     | EF Core LINQ queries and parameterized ADO.NET commands              |
| CSRF              | MVC anti-forgery tokens and `[ValidateAntiForgeryToken]`             |
| XSS               | Razor automatic HTML encoding and avoiding unsafe raw HTML rendering |
| Authentication    | Cookie authentication for MVC, JWT Bearer authentication for API     |
| Authorization     | Role-based authorization policies                                    |
| Password Security | PBKDF2 password hashing with salt                                    |
| API Protection    | JWT token validation for issuer, audience, lifetime, and signing key |
| Secure Headers    | Security headers middleware                                          |
| Input Validation  | ViewModels, DTOs, DataAnnotations, and ModelState validation         |
| Audit Trail       | SQL triggers and AuditLogs table                                     |

---

## SOLID Principles Followed

### Single Responsibility Principle

Controllers handle HTTP requests, services handle business logic, repositories handle data access, and entities represent domain data.

### Open/Closed Principle

New services, repositories, controllers, or report modules can be added without changing existing core logic.

### Liskov Substitution Principle

Interfaces allow repository and service implementations to be replaced with mock or test versions.

### Interface Segregation Principle

Separate interfaces are used for repositories, services, token generation, password hashing, and unit of work.

### Dependency Inversion Principle

Controllers depend on interfaces and services through dependency injection instead of directly creating infrastructure classes.

---

## Design Patterns Used

| Pattern               | Usage                                                                            |
| --------------------- | -------------------------------------------------------------------------------- |
| MVC Pattern           | Used in the Healthcare.MVC project                                               |
| Repository Pattern    | Encapsulates database operations                                                 |
| Unit of Work Pattern  | Coordinates repository operations and SaveChanges                                |
| Dependency Injection  | Injects DbContext, services, repositories, token service                         |
| DTO Pattern           | Used in API request and response models                                          |
| Middleware Pattern    | Used for authentication, authorization, exception handling, and security headers |
| Service Layer Pattern | Keeps business rules outside controllers                                         |

---

## Business Rules

Important rules implemented in the project:

```text
1. Email must be unique for users and patients.
2. Only registered users can create patient profiles.
3. A normal user can view only their own appointments.
4. Admin can view and manage all patients and appointments.
5. Appointment must be linked to a valid patient and doctor.
6. Duplicate doctor time slots are prevented.
7. New appointments are created with Scheduled status.
8. Appointment status can be updated through admin/API flow.
9. All important appointment operations are audited.
10. API endpoints require JWT authentication.
```

---

## Testing Checklist

Use this checklist before submission:

```text
[ ] Project builds successfully
[ ] MVC project runs
[ ] API project runs
[ ] Swagger opens
[ ] Admin login works
[ ] User registration works
[ ] Patient profile creation works
[ ] Appointment booking works
[ ] My Appointments page shows only logged-in user's appointments
[ ] Patients API returns 200 OK
[ ] Doctors API returns 200 OK
[ ] Appointments API returns 200 OK
[ ] JWT authorization works in Swagger
[ ] Protected API returns 401 without token
[ ] SQL Server stores patient and appointment data
[ ] AuditLogs table receives trigger entries
[ ] Reports page loads analytics data
```

---

## Docker Run

If Docker files are included, run:

```bash
docker compose up --build
```

Expected services:

```text
Healthcare.MVC
Healthcare.API
SQL Server
```

Use Docker Compose for container-based local testing and deployment preparation.

---

## GitHub Actions CI/CD

The project includes a conceptual GitHub Actions workflow for:

```text
Restore
Build
Test
Publish
Docker image build
Deployment preparation
```

This supports DevOps readiness for cloud deployment.

---

## Azure Hosting Concept

Recommended cloud deployment:

```text
Healthcare.MVC → Azure App Service
Healthcare.API → Azure App Service
Database → Azure SQL Database
Secrets → Azure Key Vault / App Settings
Container Images → Azure Container Registry
CI/CD → GitHub Actions
```

### Azure Configuration

Set these in Azure App Settings:

```text
ConnectionStrings__DefaultConnection
Jwt__Issuer
Jwt__Audience
Jwt__Key
Jwt__ExpiresInMinutes
```

---

## Documentation and Diagrams

The project documentation includes:

```text
System Architecture Diagram
DFD Level 0
DFD Level 1
ER Diagram
Database Design Diagram
JWT Authentication Flow
Report Generation Flow
Frontend to Backend Connectivity
Final Capstone Report
Final Presentation PPT
```

---

## Final Demo Explanation

During final demo, explain:

```text
This project follows a layered architecture with separate MVC, API, Core, and Infrastructure projects. The MVC application supports admin and patient portal workflows. The API is secured using JWT authentication and tested using Swagger. Data is stored in SQL Server using EF Core and ADO.NET for reports. Security features include SQL injection protection, CSRF protection, XSS protection, password hashing, role-based authorization, and audit logging. The system supports future deployment using Docker, GitHub Actions, and Azure.
```

---

## Future Enhancements

Possible future improvements:

```text
Online payment integration
Doctor availability calendar
Email/SMS appointment reminders
Prescription upload and download
Patient medical history dashboard
Advanced analytics using Power BI
AI-based appointment summary
MCP/AI agent for ticket or appointment summarization
Multi-hospital support
Cloud deployment on Azure
```

---

## Author

```text
Rithin Vishwas
Capstone Project: Healthcare Appointment & Care Analytics
Domain: Healthcare Analytics Platform
Technology: ASP.NET Core MVC, Web API, EF Core, SQL Server, JWT
```

---

## License

This project is created for academic capstone submission and learning purposes.
