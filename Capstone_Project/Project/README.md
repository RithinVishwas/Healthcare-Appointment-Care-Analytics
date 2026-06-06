# Healthcare Appointment & Care Analytics

A layered ASP.NET Core capstone solution for a healthcare provider to manage patient appointments, expose JWT-secured mobile APIs, generate care analytics, and demonstrate cloud/container readiness.

## Capstone coverage

| Requirement | Where it is implemented |
|---|---|
| Clean layered C# solution | `src/Healthcare.Core`, `src/Healthcare.Infrastructure`, `src/Healthcare.API`, `src/Healthcare.MVC` |
| ASP.NET Core MVC + Razor UI | `src/Healthcare.MVC` |
| ASP.NET Core Web API | `src/Healthcare.API` |
| SQL Server DDL/DML/Views/Triggers/SPs | `database/` |
| EF Core migrations | `src/Healthcare.Infrastructure/Migrations/` |
| ADO.NET reporting | `SqlAnalyticsReportService.cs` |
| JWT authentication | `JwtTokenService.cs`, `AuthController.cs`, `Program.cs` |
| CSRF protection | MVC forms use antiforgery tokens and controllers validate POST requests |
| SQL injection protection | EF Core LINQ and parameterized ADO.NET commands only |
| Input validation | Data annotations, DTO validation, model state checks, DB constraints |
| SOLID principles | Interfaces, services, repositories, unit of work, dependency injection |
| Unit tests | `src/Healthcare.Tests` |
| Docker/GitHub Actions/Azure readiness | `devops/` and `docs/Azure_Hosting_Model.md` |
| Diagrams and final documentation | `docs/` |
| 18-slide final presentation | `presentation/Healthcare_Capstone_Final_Presentation.pptx` |

## Recommended execution order

### 1. Create database
Open SQL Server Management Studio and run these files in order:

```text
01_ddl_schema.sql
02_dml_seed_data.sql
03_views.sql
04_triggers.sql
05_stored_procedures.sql
06_queries.sql
```

### 2. Update connection strings
Edit both files if your SQL Server name differs:

```text
src/Healthcare.API/appsettings.json
src/Healthcare.MVC/appsettings.json
```

Default connection string:

```json
"Server=localhost;Database=HealthcareCareAnalyticsDB;Trusted_Connection=True;TrustServerCertificate=True;"
```

For SQL Express:

```json
"Server=.\\SQLEXPRESS;Database=HealthcareCareAnalyticsDB;Trusted_Connection=True;TrustServerCertificate=True;"
```

### 3. Restore and build

```bash
dotnet restore
dotnet build
dotnet test
```

### 4. Run API and MVC together

```bash
dotnet run --project src/Healthcare.API
dotnet run --project src/Healthcare.MVC
```

Or on Windows:

```powershell
./run-both-projects.ps1
```

### 5. Default demo login

```text
Email: admin@healthcare.local
Password: Admin@123
Role: Admin
```

## Main application flow

1. Admin logs in through MVC.
2. Admin registers/updates patient details.
3. Admin books an appointment with validation.
4. Business service checks duplicate appointment and doctor availability.
5. Repository saves data through EF Core.
6. SQL Server trigger writes audit information.
7. Dashboard and reports show appointment counts, status, and department-wise analytics.
8. Mobile clients can access REST endpoints using JWT.

## Security notes

- MVC POST methods use `[ValidateAntiForgeryToken]`.
- JWT validates issuer, audience, signing key, and token lifetime.
- ADO.NET reports use `SqlParameter`; no raw input is concatenated into SQL.
- Model validation prevents invalid date, email, phone, gender, and appointment data.
- Razor encodes output by default to reduce XSS risk.
- Security headers middleware adds `X-Content-Type-Options`, `X-Frame-Options`, `Referrer-Policy`, and a base Content Security Policy.
