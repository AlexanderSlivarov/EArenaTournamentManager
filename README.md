<div align="center">

[![GitHub stars](https://img.shields.io/github/stars/AlexanderSlivarov/EArenaTournamentManager?style=for-the-badge)](https://github.com/AlexanderSlivarov/EArenaTournamentManager/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/AlexanderSlivarov/EArenaTournamentManager?style=for-the-badge)](https://github.com/AlexanderSlivarov/EArenaTournamentManager/network)
[![GitHub issues](https://img.shields.io/github/issues/AlexanderSlivarov/EArenaTournamentManager?style=for-the-badge)](https://github.com/AlexanderSlivarov/EArenaTournamentManager/issues)

**A full-stack esports tournament management platform built with ASP.NET Core and Clean Architecture.**

</div>

---

## 🛡️ Author

Александър Сливаров

---

## Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Tech Stack](#️-tech-stack)
- [Architecture](#architecture)
- [Getting Started with Docker](#-getting-started-with-docker)
- [Environment Variables](#environment-variables)
- [Running Without Docker](#️-running-without-docker)
- [Testing](#-testing)
- [Project Structure](#-project-structure)
- [Database Schema](#database-schema)
- [Contributing](#contributing)
- [Support & Contact](#-support--contact)

---

## 📖 Overview

EArena Tournament Manager is designed to serve esports communities by providing a centralized platform for organizing competitive play. It covers the full lifecycle from registering organizations and teams, to scheduling tournaments and tracking participants — all through a clean REST API and a server-rendered web front-end, containerized with Docker.

---

## ✨ Features

- 🏆 **Tournament Management:** Create and manage tournaments with format, region, map, schedule, rules, and prizes
- 🏢 **Organization Management:** Register organizations with logos and header images, manage staff with assigned roles
- 👥 **Team Management:** Build teams with captains and members, track roles and join dates
- 🎮 **Game Catalogue:** Catalogue esports titles by platform with cover images
- 👤 **User Accounts:** Role-based access control with avatar support
- 🔐 **JWT Authentication:** Secure token-based authentication with configurable expiry
- 📄 **API Documentation:** Swagger UI with a custom OpenAPI YAML spec served at `/swagger`
- 🐳 **Fully Containerized:** One-command startup with Docker Compose — database, API, and web app

---

## 🛠️ Tech Stack

**Primary:**

[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/.NET_9-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://docs.microsoft.com/en-us/aspnet/core)
[![SQL Server](https://img.shields.io/badge/SQL_Server_2022-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)](https://www.microsoft.com/en-us/sql-server)
[![EF Core](https://img.shields.io/badge/EF_Core_9-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://docs.microsoft.com/en-us/ef/core/)
[![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)](https://www.docker.com/)

**Development Tools:**

[![Visual Studio](https://img.shields.io/badge/Visual%20Studio-5C2D91?style=for-the-badge&logo=visual-studio&logoColor=white)](https://visualstudio.microsoft.com/)
[![NuGet](https://img.shields.io/badge/NuGet-004880?style=for-the-badge&logo=nuget&logoColor=white)](https://www.nuget.org/)

---

## Architecture

The solution follows **Clean Architecture** with a strict one-way dependency chain:

```
┌────────────────────────────────────────────────────┐
│                 Presentation Layer                  │
│   EArenaTournamentManager.API   (REST API)          │
│   EArenaTournamentManager.Web   (MVC Web App)       │
├────────────────────────────────────────────────────┤
│                 Application Layer                   │
│   EArenaTournamentManager.Application               │
│   DTOs · Interfaces · Services · Mappings           │
├────────────────────────────────────────────────────┤
│                Infrastructure Layer                 │
│   EArenaTournamentManager.Infrastructure            │
│   EF Core · Repositories · Migrations · Seeding    │
├────────────────────────────────────────────────────┤
│                   Domain Layer                      │
│   EArenaTournamentManager.Domain                    │
│   Entities · Enums · BaseEntity                     │
└────────────────────────────────────────────────────┘
```

`API / Web` → `Application` → `Domain`. Infrastructure implements Application interfaces and is only referenced by outer layers for DI wiring.

> The `EArenaTournamentManager.Tests` project sits outside this chain — it references the Application layer to test services and validators in isolation without touching the database or HTTP layer.

---

## 🚀 Getting Started with Docker

### Prerequisites

- [Docker Desktop](https://www.docker.com/get-started) (includes Docker Compose)
- Git

### 1. Clone the repository

```bash
git clone https://github.com/AlexanderSlivarov/EArenaTournamentManager.git
cd EArenaTournamentManager
```

### 2. Create your `.env` file

Create a `.env` file in the **root of the project** (same directory as `docker-compose.yml`):

```env
DB_PASSWORD=YourStrong@Passw0rd
JWT_KEY=your_super_secret_jwt_signing_key_min_32_chars
ADMIN_USERNAME=admin
ADMIN_EMAIL=admin@earena.com
ADMIN_PASSWORD=Admin@123!
```

> ⚠️ The `.env` file is listed in `.gitignore` — never commit it to source control.

### 3. Build and start all containers

```bash
docker compose up --build
```

Docker Compose starts three services in the correct dependency order:

| Container | Role | Exposed Port |
|---|---|---|
| `e-arena-sqlserver` | Microsoft SQL Server 2022 | `1433` |
| `e-arena-api` | ASP.NET Core REST API | `5001` |
| `e-arena-web` | ASP.NET Core MVC Web App | `5000` |

On first boot the API automatically applies all EF Core migrations and seeds the admin user. The SQL Server container runs a health check every 10 seconds — the API waits for a healthy database before starting.

### 4. Access the application

| Service | URL |
|---|---|
| Web Application | http://localhost:5000 |
| REST API | http://localhost:5001 |
| Swagger UI | http://localhost:5001/swagger |

### 5. Stopping the application

```bash
# Stop containers, keep the database volume
docker compose down

# Stop containers and delete the database volume (full reset)
docker compose down -v
```

### 6. Rebuilding after code changes

```bash
docker compose up --build --force-recreate
```

---

## Environment Variables

| Variable | Used By | Description |
|---|---|---|
| `DB_PASSWORD` | `sqlserver`, `api` | SQL Server SA account password |
| `JWT_KEY` | `api` | Secret key for signing JWT tokens (min. 32 characters) |
| `ADMIN_USERNAME` | `api` | Username for the seeded administrator account |
| `ADMIN_EMAIL` | `api` | Email for the seeded administrator account |
| `ADMIN_PASSWORD` | `api` | Password for the seeded administrator account |

Additional variables configured in `docker-compose.yml`:

| Variable | Default | Description |
|---|---|---|
| `Jwt__Issuer` | `EArenaTournamentManager` | JWT token issuer claim |
| `Jwt__Audience` | `EArenaTournamentManager` | JWT token audience claim |
| `Jwt__ExpiryHours` | `10` | Token expiry in hours |
| `ASPNETCORE_ENVIRONMENT` | `Development` | ASP.NET Core environment |

---

## Local Configuration

The project uses a local development configuration file:

```text
src/EArenaTournamentManager.API/appsettings.Development.json
```

This file is intentionally excluded from source control.

Create it by copying:

```text
src/EArenaTournamentManager.API/appsettings.Development.example.json
```

and then update the values to match your local environment.

---

## 🖥️ Running Without Docker

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- A running SQL Server instance
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository** (if not already done)
    ```bash
    git clone https://github.com/AlexanderSlivarov/EArenaTournamentManager.git
    cd EArenaTournamentManager
    ```

2. **Open in Visual Studio**
   Open `EArenaTournamentManager.sln`. Visual Studio will automatically restore NuGet packages.

   *(Alternatively, using the .NET CLI)*
   ```bash
   dotnet restore
   ```

3. Create your local configuration file:

```bash
cp src/EArenaTournamentManager.API/appsettings.Development.example.json \
   src/EArenaTournamentManager.API/appsettings.Development.json
```

4. Update the copied file with your local SQL Server connection string, JWT key, and administrator credentials.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=EArenaTournamentManagerDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "your_super_secret_jwt_key_min_32_chars",
    "Issuer": "EArenaTournamentManager",
    "Audience": "EArenaTournamentManager",
    "ExpiryHours": 10
  },
  "Seed": {
    "AdminUsername": "admin",
    "AdminEmail": "admin@earena.com",
    "AdminPassword": "Admin@123!"
  }
}
```

> The `appsettings.Development.json` file is ignored by Git and should remain local to your machine.

4. **Run the API** — migrations and seeding happen automatically on startup:
    ```bash
    dotnet run --project src/EArenaTournamentManager.API
    ```

5. **Run the web app** in a separate terminal:
    ```bash
    dotnet run --project src/EArenaTournamentManager.Web
    ```

### Available Commands

| Command | Description |
|---|---|
| `dotnet restore` | Restores NuGet packages for the solution |
| `dotnet build` | Compiles the solution |
| `dotnet run` | Builds and runs a project |
| `dotnet test` | Runs all unit tests |

### Managing Migrations (Package Manager Console)

```powershell
# Create a new migration after changing an entity
Add-Migration <MigrationName> -StartupProject EArenaTournamentManager.API

# Remove the last migration (if not yet applied)
Remove-Migration -StartupProject EArenaTournamentManager.API
```

> Always run `Add-Migration` immediately after changing a domain entity and commit the generated files.

---

## 🧪 Testing

Tests live in `tests/EArenaTournamentManager.Tests` and cover the **Application layer** — all services and validators are tested in isolation without touching the database or HTTP stack.

### What's covered

| Area | Test Files |
|---|---|
| **Services** | `AuthServiceTests`, `GameServiceTests`, `OrganizationServiceTests`, `OrganizationStaffServiceTests`, `TeamServiceTests`, `TeamMemberServiceTests`, `TournamentServiceTests`, `TournamentParticipantServiceTests`, `UserServiceTests` |
| **Validators** | `GameValidatorTests`, `OrganizationValidatorTests`, `OrganizationStaffValidatorTests`, `TeamValidatorTests`, `TeamMemberValidatorTests`, `TournamentValidatorTests`, `TournamentParticipantValidationTests`, `UserValidatorTests`, `Auth/` (auth-specific validator tests) |

### Run all tests

```bash
dotnet test
```

### Run only the test project

```bash
dotnet test tests/EArenaTournamentManager.Tests
```

### Run with detailed output

```bash
dotnet test --logger "console;verbosity=detailed"
```

### Run with coverage (requires `coverlet`)

```bash
dotnet test --collect:"XPlat Code Coverage"
```

---

## 📁 Project Structure

```
EArenaTournamentManager/
│
├── src/
│   │
│   ├── EArenaTournamentManager.API/               # REST API — entry point for the backend
│   │   ├── Controllers/                           # HTTP endpoints (Auth, Games, Teams, Orgs…)
│   │   ├── Extensions/                            # Service registration (AddApplicationServices)
│   │   ├── Middleware/                            # GlobalExceptionMiddleware
│   │   ├── wwwroot/
│   │   │   └── Docs/
│   │   │       └── earena-api-docs.yml            # OpenAPI YAML spec served at /openapi/
│   │   ├── Program.cs                             # App startup, middleware pipeline, seeding
│   │   ├── appsettings.json
│   │   └── Dockerfile
│   │
│   ├── EArenaTournamentManager.Web/               # MVC Web Application — browser-facing UI
│   │   ├── Controllers/                           # MVC controllers
│   │   ├── Views/                                 # Razor views (.cshtml)
│   │   │   ├── Shared/                            # _Layout, partial views
│   │   │   ├── Home/
│   │   │   ├── Games/
│   │   │   ├── Teams/
│   │   │   ├── Organizations/
│   │   │   └── Tournaments/
│   │   ├── wwwroot/                               # Static assets (CSS, JS, images)
│   │   ├── Program.cs
│   │   └── Dockerfile
│   │
│   ├── EArenaTournamentManager.Application/       # Use cases and business logic
│   │   ├── DTOs/                                  # Request and response models
│   │   │   ├── Auth/
│   │   │   ├── Game/
│   │   │   ├── Team/
│   │   │   ├── Organization/
│   │   │   ├── Tournament/
│   │   │   └── User/
│   │   ├── Interfaces/                            # IUnitOfWork, IRepository<T>, service contracts
│   │   ├── Services/                              # Application service implementations
│   │   └── Mappings/                              # AutoMapper profiles
│   │
│   ├── EArenaTournamentManager.Infrastructure/    # Data access and external concerns
│   │   ├── Persistence/
│   │   │   └── EArenaAppDbContext.cs              # EF Core DbContext with full model configuration
│   │   ├── Repositories/                          # Repository pattern implementations
│   │   │   └── Interfaces/                        # IUnitOfWork, IUserRepository, etc.
│   │   ├── Migrations/                            # EF Core migration files
│   │   ├── Security/                              # IPasswordHasher + implementation
│   │   └── Seed/
│   │       └── DbSeeder.cs                        # Admin user seeding on startup
│   │
│   └── EArenaTournamentManager.Domain/            # Core domain — zero external dependencies
│       ├── Entities/
│       │   ├── User.cs
│       │   ├── Game.cs
│       │   ├── Team.cs
│       │   ├── TeamMember.cs
│       │   ├── Organization.cs
│       │   ├── OrganizationStaff.cs
│       │   ├── Tournament.cs
│       │   └── TournamentParticipant.cs
│       ├── Enums/
│       │   ├── UserRole.cs
│       │   ├── OrganizationStaffRole.cs
│       │   ├── OrganizationType.cs
│       │   ├── GamePlatform.cs
│       │   ├── TournamentFormat.cs
│       │   └── TournamentStatus.cs
│       └── Common/
│           └── BaseEntity.cs                      # Id, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsActive
│
├── tests/
│   └── EArenaTournamentManager.Tests/             # Unit tests for the Application layer
│       ├── Services/                              # Service-level unit tests
│       │   ├── AuthServiceTests.cs
│       │   ├── GameServiceTests.cs
│       │   ├── OrganizationServiceTests.cs
│       │   ├── OrganizationStaffServiceTests.cs
│       │   ├── TeamServiceTests.cs
│       │   ├── TeamMemberServiceTests.cs
│       │   ├── TournamentServiceTests.cs
│       │   ├── TournamentParticipantServiceTests.cs
│       │   └── UserServiceTests.cs
│       └── Validators/                            # Validator unit tests
│           ├── Auth/                              # Auth-specific validator tests
│           ├── GameValidatorTests.cs
│           ├── OrganizationValidatorTests.cs
│           ├── OrganizationStaffValidatorTests.cs
│           ├── TeamValidatorTests.cs
│           ├── TeamMemberValidatorTests.cs
│           ├── TournamentValidatorTests.cs
│           ├── TournamentParticipantValidationTests.cs
│           └── UserValidatorTests.cs
│
├── .dockerignore
├── .gitignore
├── .gitattributes
├── docker-compose.yml                             # Orchestrates sqlserver + api + web
└── EArenaTournamentManager.sln
```

---

## Database Schema

All tables inherit `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, and `IsActive` from `BaseEntity`. Soft-delete is supported via the `IsActive` flag.

| Table | Description |
|---|---|
| `Users` | Platform accounts with hashed passwords, email, avatar, and role |
| `Games` | Esports titles with platform and cover image |
| `Organizations` | Orgs with logo, header image, and type |
| `OrganizationStaff` | Junction — users assigned to org staff with a role |
| `Teams` | Teams with a captain (FK → Users) and logo |
| `TeamMembers` | Junction — users as members of a team with an optional role |
| `Tournaments` | Tournaments linked to a game and org, with format, region, dates, and status |
| `TournamentParticipants` | Junction — teams registered to a tournament |

---

## Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/your-feature-name`
3. Commit your changes: `git commit -m "feat: describe your change"`
4. Push to the branch: `git push origin feature/your-feature-name`
5. Open a Pull Request against the `development` branch

> After any entity change, run `Add-Migration <Name> -StartupProject EArenaTournamentManager.API` before committing.

---

## 📞 Support & Contact

For any questions, feedback, or collaboration, feel free to reach out:

- **Email**: [stu2401321058@uni-plovdiv.bg](mailto:stu2401321058@uni-plovdiv.bg)

---

<div align="center">

**⭐ Star this repo if you find it helpful!**

</div>
