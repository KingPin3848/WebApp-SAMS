# SAMS

Student Attendance Management System (ASP.NET Core MVC + Identity + EF Core).

## 1) Prerequisites

Install these before running the project:

- **Visual Studio 2026 Community** (or newer) with:
  - ASP.NET and web development workload
  - .NET desktop development workload (recommended)
- **.NET SDK 10.0.x**
- **SQL Server LocalDB** (usually installed with Visual Studio)
- Optional CLI tools:
  - `dotnet-ef` for migration commands

Check SDK:

- `dotnet --version`

## 2) Clone the repository

- `git clone https://github.com/KingPin3848/WebApp-SAMS.git`
- `cd WebApp-SAMS`

If you contribute as an external contributor, fork first, then clone your fork.

## 3) Open solution

- Open `SAMS.sln` in Visual Studio.
- Ensure startup project is `SAMS`.

## 4) Restore dependencies

Use either Visual Studio restore or CLI:

- `dotnet restore SAMS.sln`

## 5) Configure local settings (important)

The app reads:

- `ConnectionStrings:DefaultConnection`
- `Authentication:Google:ClientId`
- `Authentication:Google:ClientSecret`

Recommended: use **User Secrets** for local development.

From repository root:

- `dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\\mssqllocaldb;Database=aspnet-SAMS-local;Trusted_Connection=True;MultipleActiveResultSets=true" --project "SAMS.csproj"`
- `dotnet user-secrets set "Authentication:Google:ClientId" "<your-google-client-id>" --project "SAMS.csproj"`
- `dotnet user-secrets set "Authentication:Google:ClientSecret" "<your-google-client-secret>" --project "SAMS.csproj"`

If you do not need Google sign-in locally, keep placeholders and avoid using production credentials.

## 6) Apply database migrations

This repository contains EF Core migrations under `Data/Migrations`.

Run:

- `dotnet ef database update --project "SAMS.csproj"`

If `dotnet ef` is missing:

- `dotnet tool install --global dotnet-ef`

Then run `dotnet ef database update` again.

## 7) Run the app

In Visual Studio:

- Press **F5** (or **Ctrl+F5**)

Using CLI:

- `dotnet run --project "SAMS.csproj"`

Browse to the HTTPS URL shown in output.

## 8) Default development flow

1. Create a branch:
   - `git checkout -b feature/<short-name>`
2. Make changes
3. Build:
   - `dotnet build SAMS.sln -c Release`
4. Commit:
   - `git add .`
   - `git commit -m "<clear message>"`
5. Push branch:
   - `git push origin feature/<short-name>`
6. Open a Pull Request to `master`

## 9) Troubleshooting

### NETSDK1004: `project.assets.json` not found

Run restore:

- `dotnet restore SAMS.sln`

### LocalDB connection issues

- Confirm LocalDB is installed.
- Confirm your `DefaultConnection` points to `(localdb)\\mssqllocaldb`.
- Re-run migrations.

### Google authentication fails

- Verify User Secrets values are set correctly.
- Verify callback URI configuration in Google Cloud Console matches local URL.

## 10) Project structure (high-level)

- `Program.cs` - app startup, DI, middleware, auth
- `Data/` - DbContext and EF migrations
- `Areas/` - area-specific controllers/views
- `Services/` - background and domain services
- `wwwroot/` - static assets

## 11) Role-based behavior documentation

For detailed behavior by user role (Student, Teacher, Admin, Class Kiosk, Attendance Office, and account-management roles), see:

- [docs/ROLE_BEHAVIORS.md](docs/ROLE_BEHAVIORS.md)

---

If you are new to this codebase, follow Sections **1 -> 7** in order and the app should run locally.