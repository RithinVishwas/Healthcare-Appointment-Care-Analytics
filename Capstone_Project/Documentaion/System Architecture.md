# System Architecture — Healthcare Appointment & Care Analytics

Capstone project — **ASP.NET Core MVC + Web API + EF Core + ADO.NET + MS SQL Server + JWT**.

**Author:** Rithin Vishwas

---

## 1. Overview

A layered ASP.NET Core solution for a **Healthcare Appointment & Care Analytics Platform**. The system manages patients, doctors, appointments, registered users, roles, reports, and audit logs.

The solution provides:

* A server-rendered **ASP.NET Core MVC portal** for Admin and Patient/User workflows.
* A secure **ASP.NET Core Web API** for Swagger, Postman, and future mobile clients.
* A normalized **MS SQL Server database** for healthcare records and analytics.
* **JWT authentication** for API access.
* **Cookie authentication** for MVC portal access.
* **Role-based authorization** for Admin and User access control.
* **SQL views, stored procedures, and triggers** for reporting and auditability.
* Conceptual DevOps support using Docker, GitHub Actions, and Azure hosting.

---

## 2. Business Objective

The objective of the project is to improve healthcare appointment management by providing a secure platform where:

* Patients can register, create a profile, and book appointments.
* Admins can manage patients, doctors, appointments, and reports.
* API clients can securely access healthcare data through JWT-protected endpoints.
* SQL Server maintains structured patient, appointment, user, role, and audit records.
* Reports and analytics help monitor appointments, doctor workload, and care delivery.

---

## 3. High-Level Architecture Diagram

```text
                 ┌─────────────────────────────────────────────┐
                 │              Browser / Client               │
                 │     Razor Views, MVC Forms, AJAX-ready UI    │
                 └──────────────────┬──────────────────────────┘
                                    │ HTTPS
                                    ▼
                 ┌─────────────────────────────────────────────┐
                 │              Healthcare.MVC                 │
                 │ ASP.NET Core MVC, Controllers, Razor Views   │
                 │ ViewModels, Cookie Auth, Anti-Forgery Tokens │
                 └──────────────────┬──────────────────────────┘
                                    │
                                    │ Controller → Service → Repository
                                    │
                                    ▼
┌──────────────────────────────────────────────────────────────────────────┐
│                    ASP.NET Core Middleware Pipeline                      │
│ UseExceptionHandler · UseStaticFiles · UseRouting · Authentication       │
│ Authorization · Security Headers · Model Validation                      │
└──────────────────────────────────┬───────────────────────────────────────┘
                                   │
              ┌────────────────────┴────────────────────┐
              │                                         │
              ▼                                         ▼
┌──────────────────────────────┐          ┌────────────────────────────────┐
│        Healthcare.API         │          │       Application Layer         │
│ ASP.NET Core Web API          │          │ Services and Business Rules     │
│ Controllers + DTOs            │          │ PatientService                  │
│ Swagger / OpenAPI             │          │ AppointmentService              │
│ JWT Bearer Authentication     │          │ TokenService                    │
│ Role-Based Authorization      │          │ ReportService                   │
└──────────────┬───────────────┘          └──────────────┬─────────────────┘
               │                                          │
               │ REST + JWT                               │
               ▼                                          ▼
┌──────────────────────────────┐          ┌────────────────────────────────┐
│ Swagger / Postman / Mobile   │          │          Domain Layer           │
│ API Clients                  │          │ Patient, Doctor, Appointment    │
│ Authorization: Bearer Token  │          │ MedicalRecord, AppUser, AppRole │
└──────────────────────────────┘          │ Business Rules and Validation   │
                                          └──────────────┬─────────────────┘
                                                         │
                                  ┌──────────────────────┴──────────────────────┐
                                  │                                             │
                                  ▼                                             ▼
                    ┌──────────────────────────────┐              ┌──────────────────────────────┐
                    │        EF Core Access         │              │        ADO.NET / SQL          │
                    │ HealthcareDbContext           │              │ Stored Procedures             │
                    │ Migrations                    │              │ SQL Views                     │
                    │ Repository Pattern            │              │ Reporting Queries             │
                    │ Unit of Work                  │              │ Joins and Analytics           │
                    └──────────────┬───────────────┘              └──────────────┬───────────────┘
                                   │                                             │
                                   └──────────────────────┬──────────────────────┘
                                                          ▼
                    ┌────────────────────────────────────────────────────────────┐
                    │                 MS SQL Server Database                     │
                    │ HealthcareCareAnalyticsDB                                  │
                    │ Patients · Doctors · Departments · Appointments            │
                    │ MedicalRecords · AuditLogs · AppUsers · AppRoles           │
                    │ Views · Stored Procedures · Triggers · Indexes             │
                    └────────────────────────────────────────────────────────────┘

                    ┌────────────────────────────────────────────────────────────┐
                    │                DevOps / Deployment Concept                 │
                    │ GitHub Actions · Docker · Docker Compose · Azure App       │
                    │ Service · Azure SQL Database · Azure Container Registry    │
                    └────────────────────────────────────────────────────────────┘
```

---

## 4. Layered Project Structure

```text
Healthcare Appointment & Care Analytics
├── src/
│   ├── Healthcare.Core/                  # Domain layer
│   │   ├── DTOs/                         # API request/response models
│   │   ├── Entities/                     # Patient, Doctor, Appointment, etc.
│   │   ├── Enums/                        # AppointmentStatus, roles, etc.
│   │   ├── Exceptions/                   # Custom domain exceptions
│   │   └── Interfaces/                   # Service/repository contracts
│   │
│   ├── Healthcare.Infrastructure/        # Data access and implementation layer
│   │   ├── Data/                         # HealthcareDbContext, DbInitializer
│   │   ├── Repositories/                 # Repository implementations
│   │   ├── Security/                     # JWT service, password hasher
│   │   └── Services/                     # Report and domain services
│   │
│   ├── Healthcare.API/                   # REST API layer
│   │   ├── Controllers/                  # Auth, Patients, Doctors, Appointments, Analytics
│   │   ├── Middleware/                   # Security headers, exception handling
│   │   ├── Program.cs                    # API configuration
│   │   └── appsettings.json              # API configuration
│   │
│   ├── Healthcare.MVC/                   # MVC web portal layer
│   │   ├── Controllers/                  # Account, Dashboard, Patients, Appointments, Reports
│   │   ├── Views/                        # Razor pages/views
│   │   ├── ViewModels/                   # MVC form models
│   │   ├── Middleware/                   # Security headers
│   │   ├── wwwroot/                      # CSS, JS, static files
│   │   ├── Program.cs                    # MVC configuration
│   │   └── appsettings.json              # MVC configuration
│   │
│   └── Healthcare.Tests/                 # Unit testing project
│       └── Tests/                        # Service and business rule tests
│
├── database/                             # SQL Server scripts
│   ├── 01_ddl_schema.sql                 # Tables and constraints
│   ├── 02_dml_seed_data.sql              # Initial seed data
│   ├── 03_views.sql                      # SQL views
│   ├── 04_triggers.sql                   # Audit triggers
│   ├── 05_stored_procedures.sql          # Reporting stored procedures
│   ├── 06_queries.sql                    # Demo/report queries
│   └── 07_user_portal_update.sql         # User portal database updates
│
├── docs/                                 # Documentation and diagrams
├── devops/                               # Docker and GitHub Actions assets
└── README.md
```

---

## 5. Main Modules

| Module              | Description                                                               |
| ------------------- | ------------------------------------------------------------------------- |
| Authentication      | Admin/User login, registration, cookie auth, JWT auth                     |
| Admin Dashboard     | Displays patient, doctor, appointment, and report summary                 |
| Patient Management  | Admin can create, view, and update patient records                        |
| Patient Portal      | Registered users can create profile and book appointments                 |
| Doctor Module       | Stores doctors, departments, specializations, and availability references |
| Appointment Booking | Creates appointments and validates doctor/patient information             |
| Appointment Status  | Allows appointment status tracking and update                             |
| Reports & Analytics | Uses SQL views/stored procedures for dashboard/report data                |
| API Layer           | REST endpoints tested through Swagger/Postman                             |
| Audit Logging       | SQL triggers store appointment operation audit entries                    |

---

## 6. Data Model / ER Overview

```text
┌────────────────────┐        1        *       ┌────────────────────┐
│    Departments     │────────────────────────►│      Doctors       │
├────────────────────┤                         ├────────────────────┤
│ PK Id              │                         │ PK Id              │
│ Name               │                         │ FK DepartmentId    │
│ Description        │                         │ FullName           │
│ IsActive           │                         │ Email              │
└────────────────────┘                         │ PhoneNumber        │
                                               │ Specialization     │
                                               │ IsActive           │
                                               └─────────┬──────────┘
                                                         │ 1
                                                         │
                                                         │ *
┌────────────────────┐        1        *       ┌─────────▼──────────┐
│      Patients      │────────────────────────►│    Appointments    │
├────────────────────┤                         ├────────────────────┤
│ PK Id              │                         │ PK Id              │
│ FullName           │                         │ FK PatientId       │
│ Email              │                         │ FK DoctorId        │
│ PhoneNumber        │                         │ AppointmentDateTime│
│ DateOfBirth        │                         │ DurationMinutes    │
│ Gender             │                         │ Reason             │
│ Address            │                         │ Status             │
│ BloodGroup         │                         └─────────┬──────────┘
└─────────┬──────────┘                                   │
          │                                               │
          │ 1                                             │ 1
          │                                               │
          │ 0/1                                           │ *
┌─────────▼──────────┐                         ┌─────────▼──────────┐
│      AppUsers      │                         │     AuditLogs      │
├────────────────────┤                         ├────────────────────┤
│ PK Id              │                         │ PK Id              │
│ FK RoleId          │                         │ FK AppointmentId   │
│ FK PatientId NULL  │                         │ Action             │
│ FullName           │                         │ OldStatus          │
│ Email              │                         │ NewStatus          │
│ PasswordHash       │                         │ ChangedBy          │
│ IsActive           │                         │ CreatedAtUtc       │
└─────────┬──────────┘                         └────────────────────┘
          │ *
          │
          │ 1
┌─────────▼──────────┐
│      AppRoles      │
├────────────────────┤
│ PK Id              │
│ Name               │
│ IsActive           │
└────────────────────┘

┌────────────────────┐
│   MedicalRecords   │
├────────────────────┤
│ PK Id              │
│ FK PatientId       │
│ FK AppointmentId   │
│ Diagnosis          │
│ Prescription       │
│ Notes              │
└────────────────────┘
```

---

## 7. Database Design

Database name:

```text
HealthcareCareAnalyticsDB
```

### Main Tables

| Table          | Purpose                                           |
| -------------- | ------------------------------------------------- |
| Departments    | Stores hospital departments                       |
| Doctors        | Stores doctor details and department relationship |
| Patients       | Stores patient profile data                       |
| Appointments   | Stores appointment booking details                |
| MedicalRecords | Stores diagnosis, prescription, and notes         |
| AuditLogs      | Stores audit trail for appointment operations     |
| AppUsers       | Stores application login users                    |
| AppRoles       | Stores roles such as Admin/User/Staff             |

### Database Features

* Normalized schema
* Primary keys and foreign keys
* Unique constraints for email fields
* Status enum/int for appointments
* Views for reports
* Stored procedures for analytics
* Triggers for audit logging
* Seed data for demo and testing

---

## 8. Data Flow Diagram — Level 0

```text
┌──────────────────────┐
│ Admin / Hospital     │
│ Staff                │
└──────────┬───────────┘
           │ manage patients, appointments, reports
           ▼
┌────────────────────────────────────────┐
│ Process 0                              │
│ Healthcare Appointment & Care          │
│ Analytics System                       │
└──────────┬─────────────────────────────┘
           │ dashboards, lists, analytics
           ▼
┌──────────────────────┐
│ Admin / Hospital     │
│ Staff                │
└──────────────────────┘

┌──────────────────────┐
│ Registered Patient   │
│ User                 │
└──────────┬───────────┘
           │ register, create profile, book appointment
           ▼
┌────────────────────────────────────────┐
│ Healthcare Appointment & Care          │
│ Analytics System                       │
└──────────┬─────────────────────────────┘
           │ profile status, appointment details
           ▼
┌──────────────────────┐
│ Registered Patient   │
│ User                 │
└──────────────────────┘

┌──────────────────────┐
│ Swagger / Postman /  │
│ Mobile Client        │
└──────────┬───────────┘
           │ login + REST API requests + JWT
           ▼
┌────────────────────────────────────────┐
│ Healthcare Appointment & Care          │
│ Analytics System                       │
└──────────┬─────────────────────────────┘
           │ JSON responses
           ▼
┌──────────────────────┐
│ Swagger / Postman /  │
│ Mobile Client        │
└──────────────────────┘

┌────────────────────────────────────────┐
│ MS SQL Server Database                 │
│ Patients, Doctors, Appointments,       │
│ Users, Roles, Reports, AuditLogs       │
└────────────────────────────────────────┘
```

---

## 9. Security Architecture

### Authentication

The project uses two authentication methods:

| Area       | Authentication Type       |
| ---------- | ------------------------- |
| MVC Portal | Cookie Authentication     |
| Web API    | JWT Bearer Authentication |

### Authorization

Role-based authorization is applied using policies and attributes.

Examples:

```text
AdminOnly
ClinicalStaff
User
```

### Password Security

Passwords are not stored as plain text. The project uses PBKDF2 password hashing with:

* Random salt
* Secure hash generation
* Fixed-time comparison

### JWT Flow

```text
Client submits email + password
        ↓
AuthController validates model
        ↓
DbContext loads AppUser + AppRole
        ↓
PasswordHasher verifies PBKDF2 hash
        ↓
JwtTokenService creates signed token
        ↓
Client sends Authorization: Bearer token
        ↓
JwtBearer middleware validates issuer, audience, lifetime, and key
        ↓
Protected API returns JSON response
```

---

## 10. Protection Against Common Attacks

| Attack / Risk           | Protection Used                                            |
| ----------------------- | ---------------------------------------------------------- |
| SQL Injection           | EF Core LINQ and parameterized ADO.NET commands            |
| CSRF                    | MVC anti-forgery tokens and validation filters             |
| XSS                     | Razor automatic HTML encoding and security headers         |
| Unauthorized API Access | JWT Bearer token authentication                            |
| Unauthorized MVC Access | Cookie authentication and role authorization               |
| Password Theft          | PBKDF2 password hashing with salt                          |
| Clickjacking            | X-Frame-Options / security headers                         |
| Data Tampering          | Server-side validation and business rules                  |
| Weak API Access         | JWT issuer, audience, lifetime, and signing key validation |
| Missing Audit Trail     | SQL triggers and AuditLogs table                           |

---

## 11. API Documentation

Swagger/OpenAPI is enabled for API testing.

Development Swagger URL:

```text
https://localhost:#######/swagger
```

### API Groups

```text
Auth
Patients
Doctors
Appointments
Analytics
```

### Main API Endpoints

| Method | Endpoint                                 | Auth         | Description                |
| ------ | ---------------------------------------- | ------------ | -------------------------- |
| POST   | `/api/Auth/login`                        | No           | Login and return JWT       |
| GET    | `/api/Patients`                          | JWT Required | Get all patients           |
| POST   | `/api/Patients`                          | JWT Required | Create patient             |
| GET    | `/api/Patients/{id}`                     | JWT Required | Get patient by ID          |
| PUT    | `/api/Patients/{id}`                     | JWT Required | Update patient             |
| GET    | `/api/Doctors`                           | JWT Required | Get all doctors            |
| GET    | `/api/Doctors/{id}`                      | JWT Required | Get doctor by ID           |
| GET    | `/api/Appointments`                      | JWT Required | Get all appointments       |
| POST   | `/api/Appointments`                      | JWT Required | Create appointment         |
| GET    | `/api/Appointments/upcoming`             | JWT Required | Get upcoming appointments  |
| GET    | `/api/Appointments/{id}`                 | JWT Required | Get appointment by ID      |
| PATCH  | `/api/Appointments/{id}/status/{status}` | JWT Required | Update appointment status  |
| GET    | `/api/Analytics/summary`                 | JWT Required | Get dashboard summary      |
| GET    | `/api/Analytics/doctor-workload`         | JWT Required | Get doctor workload report |

---

## 12. Report Generation Flow

```text
User opens Reports page
        ↓
ReportsController
        ↓
AnalyticsReportService
        ↓
SqlAnalyticsReportService / ADO.NET
        ↓
Stored Procedure: usp_GetDepartmentAppointmentReport
        ↓
SQL Views + Joins
        ↓
DTO Mapping
        ↓
Razor table / AJAX refresh
```

Reports are generated using SQL views, joins, stored procedures, and DTO mapping. The MVC reports page displays analytics output in tabular/dashboard form.

---

## 13. SOLID Principles Followed

| Principle             | Implementation                                                                               |
| --------------------- | -------------------------------------------------------------------------------------------- |
| Single Responsibility | Controllers handle HTTP; services handle business logic; repositories handle data access     |
| Open/Closed           | New repositories/services can be added without rewriting controllers                         |
| Liskov Substitution   | Repository/service interfaces can be replaced with test implementations                      |
| Interface Segregation | Separate interfaces are used for repositories, services, token service, and password hashing |
| Dependency Inversion  | Controllers depend on abstractions through dependency injection                              |

---

## 14. Design Patterns Used

| Design Pattern        | Usage                                                               |
| --------------------- | ------------------------------------------------------------------- |
| MVC Pattern           | Used in the Healthcare.MVC project                                  |
| Repository Pattern    | Separates database operations from controllers                      |
| Unit of Work Pattern  | Coordinates repositories and SaveChanges                            |
| Service Layer Pattern | Keeps business rules outside controllers                            |
| DTO Pattern           | Used for API request and response models                            |
| Dependency Injection  | Injects services, repositories, DbContext, token service            |
| Middleware Pattern    | Handles security, exception handling, authentication, authorization |

---

## 15. Deployment Topology — Conceptual

```text
GitHub Repository
        │
        ▼
GitHub Actions
Restore → Build → Test → Publish → Docker Build
        │
        ▼
Container Registry / Docker Images
        │
        ▼
┌────────────────────────────────────────────┐
│ Azure App Service / Container App          │
│ ├─ Healthcare.MVC                          │
│ └─ Healthcare.API                          │
└────────────────────────────────────────────┘
        │
        ▼
Azure SQL Database
```

### Deployment Notes

* MVC and API can be deployed as separate Azure App Services.
* SQL Server can be moved to Azure SQL Database.
* JWT keys and connection strings should be stored in Azure App Settings or Key Vault.
* Docker images can be pushed to Azure Container Registry.
* GitHub Actions can automate restore, build, test, and deployment.

---

## 16. Cross-Cutting Concerns

| Concern          | Implementation                                             |
| ---------------- | ---------------------------------------------------------- |
| Logging          | Console/file logging concept through application pipeline  |
| Exceptions       | Global exception middleware and structured error responses |
| Validation       | DataAnnotations, ViewModels, DTOs, ModelState checks       |
| Authentication   | Cookie auth for MVC, JWT auth for API                      |
| Authorization    | Role-based policies and controller attributes              |
| Documentation    | Swagger/OpenAPI and project documentation                  |
| Security Headers | Middleware adds secure browser/API headers                 |
| Testing          | xUnit test project structure                               |
| Auditability     | SQL triggers and AuditLogs table                           |

---

## 17. Local Configuration Notes

Do not commit real personal connection strings, machine names, emails, phone numbers, or secrets.

Use placeholders in public repositories.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=#######;Database=HealthcareCareAnalyticsDB;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=True;MultipleActiveResultSets=True;"
  },
  "Jwt": {
    "Issuer": "HealthcareAppointmentCareAnalytics",
    "Audience": "HealthcareMobileClients",
    "Key": "#######",
    "ExpiresInMinutes": "60"
  }
}
```

---

## 18. Final Demo Flow

```text
1. Show architecture diagram.
2. Run Healthcare.MVC.
3. Login as Admin.
4. Show dashboard, patients, appointments, and reports.
5. Create a patient.
6. Book an appointment.
7. Verify Patients, Appointments, and AuditLogs in SQL Server.
8. Logout.
9. Register a new user.
10. Login as User.
11. Create patient profile.
12. Book appointment from patient portal.
13. View My Appointments.
14. Run Healthcare.API.
15. Open Swagger.
16. Login and generate JWT.
17. Authorize using Bearer token.
18. Test Patients, Doctors, Appointments, and Analytics APIs.
19. Remove token and verify 401 Unauthorized.
20. Explain security, SOLID principles, design patterns, and database design.
```

---

## 19. Future Enhancements

* Doctor availability calendar
* Appointment rescheduling and cancellation
* Email/SMS appointment reminders
* Patient medical history dashboard
* Online payment integration
* Prescription upload/download
* Power BI analytics dashboard
* Multi-hospital support
* Azure deployment
* AI-based appointment summary or care assistant
* MCP-style AI endpoint for appointment or medical record summarisation

---

## 20. Summary

Healthcare Appointment & Care Analytics demonstrates a secure and layered healthcare management platform using ASP.NET Core MVC, Web API, EF Core, ADO.NET, SQL Server, JWT authentication, role-based authorization, reporting, and audit logging.

The project is designed to support final capstone evaluation areas, such as:

* Backend and MVC development
* Database and data access
* Web API and security
* Testing and code quality
* Cloud, DevOps, and AI modernization concepts
* Documentation and final presentation readiness
