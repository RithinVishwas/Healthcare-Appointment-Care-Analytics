# Core Business Rules

Scope: **Healthcare Appointment & Care Analytics** platform.

These rules are enforced through the MVC controllers, API controllers, service layer, repository layer, Entity Framework Core, SQL Server constraints, triggers, and validation logic.

---

## 1. User Accounts and Authentication

* **BR-1.1** Email must be unique for application users. Duplicate user accounts with the same email are not allowed.

* **BR-1.2** Passwords must not be stored as plain text. Passwords are stored only as secure hashed values using PBKDF2 password hashing with salt.

* **BR-1.3** The default role for newly registered users is `User`. Admin accounts are created only through seed data or database-level setup.

* **BR-1.4** Admin users are redirected to the Admin Dashboard after login. Normal registered users are redirected to the Patient Portal.

* **BR-1.5** MVC portal authentication uses cookie-based login. Web API authentication uses JWT Bearer token authentication.

* **BR-1.6** User role must be checked from the authenticated identity or validated JWT token. Roles must never be trusted from the request body.

---

## 2. Role-Based Access Rules

* **BR-2.1** Admin users can access admin dashboard, patient management, appointment management, reports, and analytics pages.

* **BR-2.2** Normal users can access only the Patient Portal.

* **BR-2.3** Normal users cannot access admin pages such as Dashboard, Patients, Reports, or full Appointment Management.

* **BR-2.4** API endpoints for Patients, Doctors, Appointments, and Analytics require a valid JWT token.

* **BR-2.5** Admin-only operations must be protected using role-based authorization policies such as `AdminOnly`.

* **BR-2.6** Clinical or staff-level API operations must be protected using authorization policies such as `ClinicalStaff`, where applicable.

---

## 3. Patient Profile Rules

* **BR-3.1** A registered user must create a patient profile before booking an appointment from the Patient Portal.

* **BR-3.2** One registered user can be linked to one patient profile through `AppUsers.PatientId`.

* **BR-3.3** Patient email must be unique to avoid duplicate patient records.

* **BR-3.4** Patient profile data must pass server-side validation before being saved.

* **BR-3.5** Required patient fields include name, email, phone number, gender, date of birth, address, and blood group, depending on the form model.

* **BR-3.6** A normal user must only view or manage their own patient profile and appointments.

---

## 4. Doctor Rules

* **BR-4.1** Every doctor must belong to a valid department using `DepartmentId`.

* **BR-4.2** Doctor email must be unique.

* **BR-4.3** Only active doctors should be used for appointment booking.

* **BR-4.4** Doctor details are used to select the doctor during appointment creation.

* **BR-4.5** The API can expose doctor details such as doctor ID, name, email, phone number, specialization, department ID, and active status.

---

## 5. Appointment Booking Rules

* **BR-5.1** Every appointment must be linked to a valid patient.

* **BR-5.2** Every appointment must be linked to a valid doctor.

* **BR-5.3** Appointment date and time must be provided before booking.

* **BR-5.4** Appointment duration must be within the accepted range configured in the ViewModel or DTO validation.

* **BR-5.5** Appointment reason must be provided and must not exceed the allowed length.

* **BR-5.6** A doctor cannot be double-booked for the same date and time slot.

* **BR-5.7** New appointments are created with the default status `Scheduled`.

* **BR-5.8** Appointment status can be updated through admin or authorized API flow.

* **BR-5.9** Appointment records must be saved to the `Appointments` table using EF Core repository logic.

* **BR-5.10** When a user books an appointment through the Patient Portal, the system must use the logged-in user’s linked `PatientId`. The browser must not be trusted to submit another patient’s ID.

---

## 6. Appointment Status Rules

* **BR-6.1** Appointment status must follow the defined appointment status enum.

* **BR-6.2** Common appointment statuses include `Scheduled`, `Completed`, `Cancelled`, and `NoShow`, depending on the enum used in the project.

* **BR-6.3** Only authorized users should update appointment status.

* **BR-6.4** Status updates must be saved through the service/repository layer.

* **BR-6.5** Appointment status changes must be traceable through audit logging.

---

## 7. Audit Logging Rules

* **BR-7.1** Important appointment operations must be tracked in the `AuditLogs` table.

* **BR-7.2** SQL triggers must automatically create audit entries when appointment records are inserted or updated.

* **BR-7.3** Audit logs should store details such as appointment ID, action, old status, new status, changed by, and timestamp where applicable.

* **BR-7.4** Audit records should not be deleted when appointment records are modified.

* **BR-7.5** Audit logs are used for traceability, accountability, and security review.

---

## 8. Reports and Analytics Rules

* **BR-8.1** Admin users can view appointment analytics and reports.

* **BR-8.2** Reports must be generated from SQL views, stored procedures, or reporting services.

* **BR-8.3** Dashboard analytics can include total patients, total doctors, total appointments, upcoming appointments, completed appointments, and doctor workload.

* **BR-8.4** Report generation should not directly expose database tables to the UI. Data should pass through the controller, service, and DTO/ViewModel flow.

* **BR-8.5** Report queries should be optimized using SQL views, joins, indexes, or stored procedures.

---

## 9. API Business Rules

* **BR-9.1** The API login endpoint must validate credentials and return a JWT token only for valid users.

* **BR-9.2** Protected API endpoints must reject requests without a valid JWT token.

* **BR-9.3** Swagger testing must use the `Authorization: Bearer <token>` format for protected endpoints.

* **BR-9.4** API controllers should return DTOs instead of directly exposing full EF Core entity objects.

* **BR-9.5** API responses should use appropriate status codes:

  * `200 OK` for successful GET requests.
  * `201 Created` for successful create operations.
  * `204 No Content` for successful update operations with no response body.
  * `400 Bad Request` for validation errors.
  * `
