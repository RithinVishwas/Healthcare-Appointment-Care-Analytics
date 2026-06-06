# API Documentation — Healthcare.API

Base URL (development): `https://localhost:#######`

All requests and responses use `application/json`.

Protected endpoints require a JWT Bearer token:

```text
Authorization: Bearer <JWT>
```

Swagger/OpenAPI documentation is available at:

```text
https://localhost:#######/swagger
```

The OpenAPI JSON file can be exported from Swagger UI at:

```text
/swagger/v1/swagger.json
```

---

## 1. Overview

`Healthcare.API` is the REST API layer of the **Healthcare Appointment & Care Analytics** platform.

It provides secure endpoints for:

* Authentication
* Patient management
* Doctor listing
* Appointment booking
* Appointment status tracking
* Healthcare analytics and reports

The API is protected using **JWT Bearer Authentication** and **Role-Based Authorization**.

---

## 2. Authentication

### POST `/api/Auth/login`

Authenticates an application user and returns a JWT token.

This endpoint is used before accessing protected API endpoints such as Patients, Doctors, Appointments, and Analytics.

### Request

```json
{
  "email": "admin@healthcare.local",
  "password": "Admin@123"
}
```

### Response — `200 OK`

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6...",
  "email": "admin@healthcare.local",
  "role": "Admin",
  "expiresAt": "2026-06-05T16:30:00Z"
}
```

### Error Responses

| Status Code        | Meaning                                    |
| ------------------ | ------------------------------------------ |
| `400 Bad Request`  | Invalid request body or validation failure |
| `401 Unauthorized` | Invalid email or password                  |

---

## 3. JWT Authorization in Swagger

After calling the login API, copy the JWT token from the response.

Click **Authorize** in Swagger and enter:

```text
Bearer <your_token_here>
```

Example:

```text
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6...
```

After authorization, protected endpoints can be tested.

---

## 4. Patients API

Protected endpoints. JWT token is required.

### GET `/api/Patients`

Returns all patient records.

### Response — `200 OK`

```json
[
  {
    "id": 1,
    "fullName": "Sample Patient",
    "email": "#######",
    "phoneNumber": "#######",
    "dateOfBirth": "2002-05-10",
    "gender": "Male",
    "address": "#######",
    "bloodGroup": "O+"
  }
]
```

### Possible Errors

| Status Code        | Meaning                          |
| ------------------ | -------------------------------- |
| `401 Unauthorized` | Missing or invalid JWT token     |
| `403 Forbidden`    | User does not have required role |

---

### GET `/api/Patients/{id}`

Returns a single patient by patient ID.

### Example

```text
GET /api/Patients/1
```

### Response — `200 OK`

```json
{
  "id": 1,
  "fullName": "Sample Patient",
  "email": "#######",
  "phoneNumber": "#######",
  "dateOfBirth": "2002-05-10",
  "gender": "Male",
  "address": "#######",
  "bloodGroup": "O+"
}
```

### Error Responses

| Status Code        | Meaning                          |
| ------------------ | -------------------------------- |
| `401 Unauthorized` | Missing or invalid JWT           |
| `403 Forbidden`    | Authenticated but not authorized |
| `404 Not Found`    | Patient ID does not exist        |

---

### POST `/api/Patients`

Creates a new patient record.

Admin or authorized clinical staff access may be required depending on the policy configured in the project.

### Request

```json
{
  "fullName": "API Test Patient",
  "email": "apitestpatient@example.com",
  "phoneNumber": "#######",
  "dateOfBirth": "2002-05-10",
  "gender": "Male",
  "address": "#######",
  "bloodGroup": "O+"
}
```

### Response — `201 Created`

```json
{
  "id": 8,
  "fullName": "API Test Patient",
  "email": "apitestpatient@example.com",
  "phoneNumber": "#######",
  "dateOfBirth": "2002-05-10",
  "gender": "Male",
  "address": "#######",
  "bloodGroup": "O+"
}
```

### Error Responses

| Status Code        | Meaning                                   |
| ------------------ | ----------------------------------------- |
| `400 Bad Request`  | Validation failed or email already exists |
| `401 Unauthorized` | JWT token missing or invalid              |
| `403 Forbidden`    | User does not have permission             |

---

### PUT `/api/Patients/{id}`

Updates an existing patient record.

### Example

```text
PUT /api/Patients/1
```

### Request

```json
{
  "id": 1,
  "fullName": "Updated Patient",
  "email": "updatedpatient@example.com",
  "phoneNumber": "#######",
  "dateOfBirth": "2002-05-10",
  "gender": "Male",
  "address": "#######",
  "bloodGroup": "B+"
}
```

### Response — `204 No Content`

Patient updated successfully.

### Error Responses

| Status Code        | Meaning                          |
| ------------------ | -------------------------------- |
| `400 Bad Request`  | Invalid input or duplicate email |
| `401 Unauthorized` | Missing or invalid JWT           |
| `403 Forbidden`    | User does not have required role |
| `404 Not Found`    | Patient does not exist           |

---

## 5. Doctors API

Protected endpoints. JWT token is required.

### GET `/api/Doctors`

Returns all active doctors.

This endpoint is useful before creating an appointment because the client needs a valid `doctorId`.

### Response — `200 OK`

```json
[
  {
    "id": 1,
    "fullName": "Dr. Ananya Rao",
    "email": "ananya.rao@healthcare.local",
    "phoneNumber": "#######",
    "specialization": "General Physician",
    "departmentId": 1,
    "isActive": true
  },
  {
    "id": 2,
    "fullName": "Dr. Karthik Menon",
    "email": "karthik.menon@healthcare.local",
    "phoneNumber": "#######",
    "specialization": "Cardiologist",
    "departmentId": 2,
    "isActive": true
  }
]
```

### Error Responses

| Status Code        | Meaning                          |
| ------------------ | -------------------------------- |
| `401 Unauthorized` | Missing or invalid JWT           |
| `403 Forbidden`    | Authenticated but not authorized |

---

### GET `/api/Doctors/{id}`

Returns one doctor by doctor ID.

### Example

```text
GET /api/Doctors/1
```

### Response — `200 OK`

```json
{
  "id": 1,
  "fullName": "Dr. Ananya Rao",
  "email": "ananya.rao@healthcare.local",
  "phoneNumber": "#######",
  "specialization": "General Physician",
  "departmentId": 1,
  "isActive": true
}
```

### Error Responses

| Status Code        | Meaning                          |
| ------------------ | -------------------------------- |
| `401 Unauthorized` | Missing or invalid JWT           |
| `403 Forbidden`    | Authenticated but not authorized |
| `404 Not Found`    | Doctor ID does not exist         |

---

## 6. Appointments API

Protected endpoints. JWT token is required.

### GET `/api/Appointments`

Returns all appointment records available to the authorized user or clinical/admin role.

### Response — `200 OK`

```json
[
  {
    "id": 1,
    "patientId": 1,
    "doctorId": 1,
    "appointmentDateTime": "2026-06-10T11:00:00",
    "durationMinutes": 30,
    "reason": "General health checkup",
    "status": 0
  }
]
```

### Appointment Status Values

```text
0 = Scheduled
1 = Completed
2 = Cancelled
3 = NoShow
```

Status values may differ slightly depending on the enum names configured in the project, but the API uses the `AppointmentStatus` enum internally.

### Error Responses

| Status Code                 | Meaning                          |
| --------------------------- | -------------------------------- |
| `401 Unauthorized`          | Missing or invalid JWT           |
| `403 Forbidden`             | User does not have required role |
| `500 Internal Server Error` | Unexpected server-side error     |

---

### GET `/api/Appointments/{id}`

Returns a single appointment by appointment ID.

### Example

```text
GET /api/Appointments/1
```

### Response — `200 OK`

```json
{
  "id": 1,
  "patientId": 1,
  "doctorId": 1,
  "appointmentDateTime": "2026-06-10T11:00:00",
  "durationMinutes": 30,
  "reason": "General health checkup",
  "status": 0
}
```

### Error Responses

| Status Code        | Meaning                          |
| ------------------ | -------------------------------- |
| `401 Unauthorized` | Missing or invalid JWT           |
| `403 Forbidden`    | Authenticated but not authorized |
| `404 Not Found`    | Appointment ID does not exist    |

---

### GET `/api/Appointments/upcoming`

Returns upcoming appointments based on appointment date and status.

### Response — `200 OK`

```json
[
  {
    "id": 5,
    "patientId": 3,
    "doctorId": 2,
    "appointmentDateTime": "2026-06-12T10:30:00",
    "durationMinutes": 30,
    "reason": "Follow-up consultation",
    "status": 0
  }
]
```

If no upcoming appointments exist, the response may be:

```json
[]
```

---

### POST `/api/Appointments`

Creates a new appointment.

The API validates:

* Patient ID must exist.
* Doctor ID must exist.
* Appointment date/time must be valid.
* Duration must be valid.
* Duplicate doctor time slot should not be allowed.
* Reason should not exceed allowed length.

### Request

```json
{
  "patientId": 1,
  "doctorId": 1,
  "appointmentDateTime": "2026-06-10T11:00:00",
  "durationMinutes": 30,
  "reason": "General health checkup"
}
```

### Response — `201 Created`

```json
{
  "id": 10,
  "patientId": 1,
  "doctorId": 1,
  "appointmentDateTime": "2026-06-10T11:00:00",
  "durationMinutes": 30,
  "reason": "General health checkup",
  "status": 0
}
```

### Error Responses

| Status Code        | Meaning                                          |
| ------------------ | ------------------------------------------------ |
| `400 Bad Request`  | Validation failure or duplicate appointment slot |
| `401 Unauthorized` | Missing or invalid JWT                           |
| `403 Forbidden`    | User does not have required role                 |
| `404 Not Found`    | Linked patient or doctor does not exist          |

---

### PATCH `/api/Appointments/{id}/status/{status}`

Updates appointment status.

### Example

```text
PATCH /api/Appointments/1/status/1
```

### Response — `204 No Content`

Appointment status updated successfully.

### Error Responses

| Status Code        | Meaning                          |
| ------------------ | -------------------------------- |
| `400 Bad Request`  | Invalid status value             |
| `401 Unauthorized` | Missing or invalid JWT           |
| `403 Forbidden`    | Authenticated but not authorized |
| `404 Not Found`    | Appointment ID does not exist    |

---

## 7. Analytics API

Protected endpoints. JWT token is required.

### GET `/api/Analytics/summary`

Returns dashboard summary data.

### Response — `200 OK`

```json
{
  "totalPatients": 7,
  "totalDoctors": 3,
  "totalAppointments": 10,
  "scheduledAppointments": 5,
  "completedAppointments": 3,
  "cancelledAppointments": 1,
  "noShowAppointments": 1
}
```

### Error Responses

| Status Code        | Meaning                          |
| ------------------ | -------------------------------- |
| `401 Unauthorized` | Missing or invalid JWT           |
| `403 Forbidden`    | User does not have required role |

---

### GET `/api/Analytics/doctor-workload`

Returns doctor-wise appointment workload analytics.

### Response — `200 OK`

```json
[
  {
    "doctorId": 1,
    "doctorName": "Dr. Ananya Rao",
    "specialization": "General Physician",
    "totalAppointments": 5
  },
  {
    "doctorId": 2,
    "doctorName": "Dr. Karthik Menon",
    "specialization": "Cardiologist",
    "totalAppointments": 3
  }
]
```

### Error Responses

| Status Code        | Meaning                          |
| ------------------ | -------------------------------- |
| `401 Unauthorized` | Missing or invalid JWT           |
| `403 Forbidden`    | Authenticated but not authorized |

---

## 8. Conceptual AI / MCP Endpoint

This endpoint is conceptual and reserved for future enhancement.

### POST `/api/ai/summarize-appointment`

Generates a summary for an appointment, patient concern, or care note.

This is not required for the current working submission but can be included as a future AI modernization concept.

### Request

```json
{
  "appointmentId": 2
}
```

### Response

```json
{
  "summary": "Patient booked a follow-up consultation for general health review.",
  "suggestedAction": "Doctor may review patient profile, previous appointment notes, and medical record before consultation."
}
```

### Future Use Cases

* Appointment summary
* Doctor preparation note
* Patient visit summary
* Medical record summarization
* Care follow-up recommendation

---

## 9. Error Response Format

The API returns structured error responses for validation, authentication, authorization, and server-side errors.

### Example — Validation Error

```json
{
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Email": [
      "The Email field is required."
    ],
    "FullName": [
      "The FullName field is required."
    ]
  }
}
```

### Example — Unauthorized

```json
{
  "status": 401,
  "message": "Unauthorized"
}
```

### Example — Not Found

```json
{
  "message": "Appointment not found."
}
```

### Example — Unexpected Server Error

```json
{
  "message": "Unexpected server error."
}
```

---

## 10. Status Codes Summary

| Code                        | Meaning                                             |
| --------------------------- | --------------------------------------------------- |
| `200 OK`                    | Request completed successfully                      |
| `201 Created`               | New resource created successfully                   |
| `204 No Content`            | Update completed successfully with no response body |
| `400 Bad Request`           | Validation failure or invalid request               |
| `401 Unauthorized`          | JWT token missing, invalid, or expired              |
| `403 Forbidden`             | User is authenticated but does not have permission  |
| `404 Not Found`             | Requested resource does not exist                   |
| `409 Conflict`              | Duplicate or conflicting business rule condition    |
| `500 Internal Server Error` | Unexpected server-side error                        |

---

## 11. API Testing Flow in Swagger

Recommended Swagger testing order:

```text
1. Open Swagger at /swagger.
2. Expand POST /api/Auth/login.
3. Login using admin credentials.
4. Copy JWT token from response.
5. Click Authorize.
6. Enter Bearer <token>.
7. Test GET /api/Patients.
8. Test GET /api/Doctors.
9. Test GET /api/Appointments.
10. Test POST /api/Appointments.
11. Test GET /api/Appointments/upcoming.
12. Test PATCH /api/Appointments/{id}/status/{status}.
13. Test GET /api/Analytics/summary.
14. Remove token and confirm protected API returns 401 Unauthorized.
```

---

## 12. SQL Verification After API Testing

After creating an appointment through the API, verify in SQL Server:

```sql
USE HealthcareCareAnalyticsDB;

SELECT TOP 10 *
FROM Appointments
ORDER BY Id DESC;
```

Verify audit log trigger:

```sql
USE HealthcareCareAnalyticsDB;

SELECT TOP 10 *
FROM AuditLogs
ORDER BY Id DESC;
```

Expected result:

* Appointment appears in the `Appointments` table.
* Trigger inserts an audit entry in the `AuditLogs` table.

---

## 13. Security Notes

The API is protected using:

* JWT Bearer Authentication
* Role-Based Authorization
* DTO validation
* Server-side ModelState validation
* EF Core parameterized queries
* Security headers middleware
* Password hashing
* Protected analytics and appointment endpoints
* Audit logging through SQL triggers

---

## 14. Summary

`Healthcare.API` provides a secure REST API layer for the Healthcare Appointment & Care Analytics platform.

It supports authentication, patient management, doctor lookup, appointment booking, appointment status update, analytics, and Swagger-based testing.

The API is designed to support current web usage and future mobile or external healthcare integration.
