# User Portal Update Steps

This update adds normal user registration, user login redirection, patient profile creation, and user appointment booking.

## Files added

- `src/Healthcare.MVC/ViewModels/RegisterViewModel.cs`
- `src/Healthcare.MVC/ViewModels/UserPatientProfileViewModel.cs`
- `src/Healthcare.MVC/ViewModels/UserBookAppointmentViewModel.cs`
- `src/Healthcare.Core/Interfaces/IUserPortalService.cs`
- `src/Healthcare.Infrastructure/Services/UserPortalService.cs`
- `src/Healthcare.MVC/Controllers/PatientPortalController.cs`
- `src/Healthcare.MVC/Views/Account/Register.cshtml`
- `src/Healthcare.MVC/Views/PatientPortal/Dashboard.cshtml`
- `src/Healthcare.MVC/Views/PatientPortal/Profile.cshtml`
- `src/Healthcare.MVC/Views/PatientPortal/BookAppointment.cshtml`
- `src/Healthcare.MVC/Views/PatientPortal/MyAppointments.cshtml`
- `database/07_user_portal_update.sql`

## Files modified

- `src/Healthcare.Core/Entities/AppUser.cs`
- `src/Healthcare.Infrastructure/Data/HealthcareDbContext.cs`
- `src/Healthcare.Infrastructure/DependencyInjection.cs`
- `src/Healthcare.MVC/Controllers/AccountController.cs`
- `src/Healthcare.MVC/Views/Account/Login.cshtml`
- `src/Healthcare.MVC/Views/Shared/_Layout.cshtml`
- Admin controllers were restricted using `AdminOnly` policy.

## Execution order

1. Run `database/07_user_portal_update.sql` in SSMS.
2. Run `dotnet clean`.
3. Run `dotnet build`.
4. Run `dotnet run --project src/Healthcare.MVC`.
5. Open Login page.
6. Click Register here.
7. Register a new user.
8. Login using the new user.
9. Create patient profile.
10. Book appointment.
