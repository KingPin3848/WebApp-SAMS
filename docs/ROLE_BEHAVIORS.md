# Role-Based Behavior Documentation

This document describes **current implemented behavior** for role-based users in SAMS.

It is intended for contributors who need to understand what each role can do, where those behaviors live, and what outcomes are expected.

## Scope and conventions

- This reflects behavior found in current controllers/middleware/services.
- If a role restriction is commented out in code, this document marks it as **not currently enforced**.
- All routes described are MVC area routes unless noted.

---

## 1) Cross-cutting runtime behavior

### 1.1 Global exception handling

**Source:** `Middleware/GlobalExceptionHandler.cs`, registration in `Program.cs`

Unhandled exceptions are converted into RFC 7807 `application/problem+json` responses with:

- `status`
- `title`
- `detail`
- `type` (httpstatuses URL)
- `instance` (request path)
- extensions:
  - `code`
  - `traceId`
  - `timestampUtc`

Mapped error codes:

- `ArgumentException` -> `400`, `SAMS_BAD_REQUEST_001`
- `KeyNotFoundException` -> `404`, `SAMS_NOT_FOUND_001`
- `UnauthorizedAccessException` -> `401`, `SAMS_AUTH_001`
- `NotImplementedException` -> `501`, `SAMS_NOT_IMPLEMENTED_001`
- default -> `500`, `SAMS_SERVER_001`

### 1.2 Role-scoped error reporting endpoint

**Source:** `Areas/*/Controllers/ErrorController.cs`, shared logic in `Areas/Shared/Controllers/ErrorControllerBase.cs`

Each role area exposes an `AutomatedError` action (POST + anti-forgery) that:

1. Sanitizes description/reference values
2. Builds normalized app error code: `SAMS-ERR-{number:D4}-{referenceToken}`
3. Persists a `ReportModel` entry
4. Renders shared error view data:
   - `Error`
   - `ErrorNumber`
   - `ErrorCode`
   - `ErrorDescription`
   - `DeveloperReference`

Report severity behavior:

- Authenticated user context -> `Medium`
- Missing/anonymous user context -> `High`

---

## 2) Role: Student

### 2.1 Primary feature: QR attendance scan

**Source:** `Areas/Student/Controllers/ScanController.cs`

Actions:

- `GET Scan()`
  - Loads scan page and exposes logged-in school ID via `ViewBag.Schoolid`.
- `POST Scan(string ScannedCode, string issuedSchoolId)`
  - Validates current user and selected bell schedule.
  - Determines current bell by active schedule.
  - Validates submitted school ID against signed-in user’s school ID.
  - Resolves student schedule by semester switch date.
  - Resolves expected room QR code for current bell/course.
  - If scanned code matches expected code:
	- Updates daily attendance from `Unknown` to `Present` or `Tardy` based on time window.
	- Updates bell attendance from `Unknown` to `Present` or `Tardy` similarly.
	- Writes timestamp audit entries.
  - Returns JSON success/failure payload messages for UI display.

Important runtime outcomes:

- Not in session / bell 0 blocks sign-in.
- Invalid schedule/course/room/QR conditions return user-facing error JSON.
- Attendance updates depend on existing pre-created attendance rows with `Unknown` status.

Authorization status:

- Student role attributes are currently commented for scan endpoints in code; behavior is available but strict role enforcement is **not currently enforced on those actions**.

### 2.2 Student error reporting

**Source:** `Areas/Student/Controllers/ErrorController.cs`

- Role requirement: `Student`
- POST `AutomatedError(...)` uses shared error pipeline described above.

---

## 3) Role: Teacher

### 3.1 Primary feature: class roster and live attendance status

**Source:** `Areas/Teacher/Controllers/TeacherRoster.cs`

Actions:

- `GET Index()`
  - Loads teacher roster landing page.
- `GET Roster(int bell)`
  - Requires authenticated teacher user.
  - Finds active courses assigned to teacher by `CourseTeacherID == teacher.SchoolId`.
  - Iterates student users and resolves semester schedule.
  - Maps requested bell to course ID (day-aware mapping for MW/TTh/Fri variants).
  - Produces roster entries containing:
	- Student ID
	- Student name
	- Student email
	- Daily attendance status
	- Bell attendance status for selected bell/date

Authorization status:

- Controller is enforced with `[Authorize(Roles = "Teacher")]`.

### 3.2 Teacher error reporting

**Source:** `Areas/Teacher/Controllers/ErrorController.cs`

- Role requirement: `Teacher`
- POST `AutomatedError(...)` uses shared error pipeline.

---

## 4) Role: Class Kiosk Operator

Role string in code: `Synnovation Lab QR Code Scanner Management`.

### 4.1 Primary feature: kiosk-based attendance authentication

**Source:** `Areas/ClassKiosk/Controllers/ClassKioskController.cs`

Actions:

- `GET Index()`
  - Loads kiosk scanning page.
- `POST Index(string LocalScannedCode, string LocalStudentPin)`
  - Validates QR payload and PIN input shape.
  - Parses scanner identifier from QR prefix.
  - Validates student activation code + PIN.
  - Determines current bell from selected schedule.
  - Resolves student schedule/course/room for current bell (including transition logic).
  - Updates attendance rows (`DailyAttendance`, `BellAttendance`) from `Unknown` to `Present`/`Tardy` based on timing.
  - Updates student location (`StudentLocationModel`) to scanner room context.
  - Writes timestamp records for attendance and location actions.
  - Returns JSON with success flag, message, refresh hint, and wait seconds.

Important runtime outcomes:

- Invalid scanner identity, student details, PIN, or schedule mapping returns kiosk-friendly JSON error.
- Attendance and location workflows are tightly coupled to configured scanner-room and schedule data.

Authorization status:

- No role attribute on `ClassKioskController` itself; role enforcement is **not currently declared on these actions**.

### 4.2 Class Kiosk error reporting

**Source:** `Areas/ClassKiosk/Controllers/ErrorController.cs`

- Role requirement: `Synnovation Lab QR Code Scanner Management`
- POST `AutomatedError(...)` uses shared error pipeline.

---

## 5) Role: Attendance Office Member

### 5.1 Error reporting behavior

**Source:** `Areas/AttOfficePersonnel/Controllers/ErrorController.cs`

- Role requirement: `Attendance Office Member`
- POST `AutomatedError(...)` uses shared error pipeline.

### 5.2 Operational dependency

Attendance-office workflows depend on attendance data maintained by:

- student scan flow
- class kiosk flow
- automated services (see Section 8)

Within the currently documented role-controller scope, the attendance-office area exposes role-specific error reporting endpoint behavior.

---

## 6) Role: Admin

### 6.1 Admin error reporting

**Source:** `Areas/Admin/Controllers/ErrorController.cs`

- Role requirement: `Admin`
- POST `AutomatedError(...)` uses shared error pipeline.

### 6.2 Report and case viewing

**Source:** `Areas/Admin/Controllers/ReportController.cs`

Actions:

- `GET Index()`
  - Validates logged-in identity.
  - Loads view data:
	- self-created reports
	- all reports
- `GET MyCases()`
  - Returns current user’s report cases as JSON.
- `GET AllCases()`
  - Returns all report cases as JSON.
- `GET Details(int? id)`
  - Loads specific report details view.
- `GET Create()`
  - Prepares report creation view data (user + report type source).

Authorization status:

- Controller-level admin authorize attribute is commented out in source, so strict role restriction is **not currently enforced at controller level**.
- Several actions still gate by checking active authenticated user and redirect to error flow on failure.

---

## 7) Roles: HS School Admin, Synnovation Lab Admin, District Admin

### 7.1 Account management feature

**Source:** `Areas/Admin/Controllers/AccountManagerController.cs`

This controller manages core user-account lifecycle behavior.

Observed implemented behaviors include:

- Listing users (excluding developer accounts)
- Viewing account details
- Creating accounts with role and profile metadata
- Editing account data
- Deleting accounts

Authorization status by action (as implemented):

- `Index()` has commented role authorization (not currently enforced).
- Key management actions such as `Details(...)`, `Create(...)`, etc. use:
  - `[Authorize(Roles = "HS School Admin,Synnovation Lab Admin,District Admin")]`

Expected outcome:

- These role groups are the primary intended operators for account administration workflows.

---

## 8) Background attendance and automation dependencies

Several services govern attendance state that role-facing features rely on.

Representative services:

- `Services/DailyAttendanceAdditionService.cs`
- `Services/Bell2BellAdditionService.cs`
- `Services/AvesBellAdditionService.cs`
- `Services/AutomaticDailyAbsent.cs`
- `Services/AutomaticBellAbsent.cs`
- `Services/AutomaticAvesAbsent.cs`
- `Services/StudentLocationClearance.cs`
- `Services/RoomQRCodeService.cs`
- `Services/QRCodeUpdater.cs`

Behavioral significance:

- Generate baseline attendance rows
- Automatically mark unresolved attendance as absent after timing windows
- Keep QR and location-related state synchronized

These services provide the baseline records and timing transitions consumed by Student, Teacher, Class Kiosk, and Attendance Office workflows.

---

## 9) Quick role-to-feature map

- **Student** -> scan-driven attendance marking (`Student/ScanController`)
- **Teacher** -> roster and attendance visibility per bell (`Teacher/TeacherRoster`)
- **Class Kiosk Operator** -> kiosk attendance + location updates (`ClassKiosk/ClassKioskController`)
- **Attendance Office Member** -> scoped error reporting endpoint (`AttOfficePersonnel/ErrorController`)
- **Admin** -> report/case views + scoped error reporting (`Admin/ReportController`, `Admin/ErrorController`)
- **HS School Admin / Synnovation Lab Admin / District Admin** -> account lifecycle management (`Admin/AccountManagerController`)

---

## 10) Notes for contributors

When updating role behavior:

1. Update controller-level or action-level `[Authorize]` attributes consistently.
2. Keep this file synchronized with effective enforcement (not just intent comments).
3. If behavior changes response shape (JSON or views), document expected output/state transitions here.
4. If attendance rules change, update both role sections and automation dependency notes.
