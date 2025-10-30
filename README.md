<img width="1444" height="800" alt="image" src="https://github.com/user-attachments/assets/3d9d1878-2aeb-43ca-b6f0-4908730c77ed" />


# Tenant Management System (TMS) REST API

A RESTful API built with Clean Architecture principles for managing tenants and their lease agreements. This project demonstrates modern .NET development practices including JWT authentication, Entity Framework Core, and comprehensive testing.

## Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Features](#features)
- [Technologies](#technologies)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [API Endpoints](#api-endpoints)
- [Authentication](#authentication)
- [Database Configuration](#database-configuration)
- [Running Tests](#running-tests)
- [API Documentation](#api-documentation)
- [Contributing](#contributing)
- [License](#license)

## Overview

The Tenant Management System is a production-ready REST API designed to manage tenant information and lease agreements. It follows Clean Architecture principles to ensure separation of concerns, maintainability, and testability.

### Key Capabilities

- Complete tenant lifecycle management (Create, Read, Update, Delete)
- Lease agreement tracking and management
- Role-based access control (Admin, Manager, Viewer)
- JWT-based authentication and authorization
- Comprehensive data validation
- Pagination support for large datasets
- Automated database migrations

## Architecture

This project implements Clean Architecture with four distinct layers:

### Domain Layer
- Contains core business entities (Tenant, Lease, User)
- No external dependencies
- Pure C# domain models

### Application Layer
- Defines Data Transfer Objects (DTOs)
- Contains repository interfaces
- Implements AutoMapper profiles for object mapping
- Houses business logic contracts

### Infrastructure Layer
- Implements data access using Entity Framework Core
- Handles JWT token generation
- Manages password hashing
- Provides concrete implementations of repository interfaces

### API Layer
- ASP.NET Core Web API controllers
- JWT authentication middleware
- Swagger/OpenAPI documentation
- Dependency injection configuration

## Features

### Tenant Management
- Create new tenant records
- Retrieve tenant information with pagination
- Update existing tenant details
- Delete tenant records (Admin only)
- Email uniqueness validation

### Lease Management
- Create lease agreements linked to tenants
- Retrieve leases by tenant or lease ID
- Update lease terms and conditions
- Delete lease records (Admin only)
- Track property addresses, dates, and financial terms

### Security
- JWT Bearer token authentication
- Role-based authorization (Admin, Manager, Viewer)
- Secure password hashing using SHA256
- CORS support for frontend integration

### Data Management
- Entity Framework Core with SQL Server
- Code-first database migrations
- Cascade delete for related records
- Unique constraints on email and username

## Technologies

| Category | Technology |
|----------|-----------|
| Language | C# 9 / .NET 6+ |
| Architecture | Clean Architecture |
| Database | SQL Server / SQL Server Express |
| ORM | Entity Framework Core 8.0 |
| Object Mapping | AutoMapper 15.1.0 |
| Authentication | JWT (JSON Web Tokens) |
| API Documentation | Swagger / OpenAPI (Swashbuckle) |
| Testing | xUnit, Moq, FluentAssertions |
| Validation | FluentValidation |

## Getting Started

### Prerequisites

- .NET 6 SDK or later
- SQL Server or SQL Server Express
- Visual Studio 2022 / Visual Studio Code / Rider
- Git

### Installation

1. Clone the repository
```bash
git clone https://github.com/yourusername/tenant-management-system.git
cd tenant-management-system
```

2. Restore NuGet packages
```bash
dotnet restore
```

3. Update the connection string in `src/Tms.Api/appsettings.json`
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=TmsDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

4. Run database migrations
```bash
dotnet ef database update -p src/Tms.Infrastructure -s src/Tms.Api
```

5. Run the application
```bash
cd src/Tms.Api
dotnet run
```

6. Access Swagger UI
```
https://localhost:5001/swagger
```

### Default Credentials

The application automatically seeds an admin user on first run:

- **Username**: admin
- **Password**: Admin@123

## Project Structure

```
TenantManagementSystem/
├── src/
│   ├── Tms.Domain/              # Domain entities
│   │   └── Entities/
│   │       ├── Tenant.cs
│   │       ├── Lease.cs
│   │       └── User.cs
│   │
│   ├── Tms.Application/         # Application logic
│   │   ├── DTOs/
│   │   ├── Interfaces/
│   │   └── Mappings/
│   │
│   ├── Tms.Infrastructure/      # Data access & external services
│   │   ├── Persistence/
│   │   ├── Repositories/
│   │   └── Auth/
│   │
│   └── Tms.Api/                 # Web API
│       ├── Controllers/
│       ├── Program.cs
│       └── appsettings.json
│
└── tests/
    └── Tms.Tests/               # Unit & integration tests
```

## API Endpoints

### Authentication

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | /api/auth/login | Authenticate user and receive JWT token | No |
| POST | /api/auth/register | Register new user account | No |

### Tenants

| Method | Endpoint | Description | Auth Required | Roles |
|--------|----------|-------------|---------------|-------|
| GET | /api/tenants | Get paginated list of tenants | Yes | All |
| GET | /api/tenants/{id} | Get specific tenant by ID | Yes | All |
| POST | /api/tenants | Create new tenant | Yes | Admin, Manager |
| PUT | /api/tenants/{id} | Update existing tenant | Yes | Admin, Manager |
| DELETE | /api/tenants/{id} | Delete tenant | Yes | Admin |

### Leases

| Method | Endpoint | Description | Auth Required | Roles |
|--------|----------|-------------|---------------|-------|
| GET | /api/tenants/{tenantId}/leases | Get all leases for a tenant | Yes | All |
| GET | /api/leases/{id} | Get specific lease by ID | Yes | All |
| POST | /api/tenants/{tenantId}/leases | Create new lease | Yes | Admin, Manager |
| PUT | /api/leases/{id} | Update existing lease | Yes | Admin, Manager |
| DELETE | /api/leases/{id} | Delete lease | Yes | Admin |

## Authentication

This API uses JWT Bearer token authentication. To access protected endpoints:

1. Obtain a token via `/api/auth/login`
2. Include the token in the Authorization header:
```
Authorization: Bearer {your_token_here}
```

### Example Login Request

```json
POST /api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "Admin@123"
}
```

### Example Response

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "admin",
  "role": "Admin"
}
```

## Database Configuration

### Connection Strings

**SQL Server Express (Default)**
```json
"Server=.\\SQLEXPRESS;Database=TmsDb;Trusted_Connection=True;TrustServerCertificate=True"
```

**SQL Server LocalDB**
```json
"Server=(localdb)\\mssqllocaldb;Database=TmsDb;Trusted_Connection=True;TrustServerCertificate=True"
```

**SQL Server with Authentication**
```json
"Server=localhost;Database=TmsDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True"
```

### Migrations

Create a new migration:
```bash
dotnet ef migrations add MigrationName -p src/Tms.Infrastructure -s src/Tms.Api
```

Apply migrations:
```bash
dotnet ef database update -p src/Tms.Infrastructure -s src/Tms.Api
```

Remove last migration:
```bash
dotnet ef migrations remove -p src/Tms.Infrastructure -s src/Tms.Api
```

## Running Tests

Execute all tests:
```bash
dotnet test
```

Run tests with coverage:
```bash
dotnet test /p:CollectCoverage=true
```

Run specific test project:
```bash
dotnet test tests/Tms.Tests/Tms.Tests.csproj
```

## API Documentation

Interactive API documentation is available via Swagger UI when running in Development mode:

```
https://localhost:5001/swagger
```

The Swagger interface provides:
- Complete API endpoint documentation
- Request/response schemas
- Interactive testing capability
- JWT authentication integration

## Sample Requests

### Create Tenant

```json
POST /api/tenants
Authorization: Bearer {token}
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "phone": "0821234567"
}
```

### Create Lease

```json
POST /api/tenants/{tenantId}/leases
Authorization: Bearer {token}
Content-Type: application/json

{
  "propertyAddress": "123 Main Street, Cape Town",
  "startDate": "2025-01-01",
  "endDate": "2025-12-31",
  "monthlyRent": 15000.00,
  "deposit": 15000.00
}
```

## Configuration

### JWT Settings

Configure JWT authentication in `appsettings.json`:

```json
{
  "JwtSettings": {
    "Secret": "YourSecretKeyMustBeAtLeast32CharactersLong!",
    "Issuer": "TmsApi",
    "Audience": "TmsApiUsers",
    "ExpiryMinutes": 60
  }
}
```

Important: Always use environment variables or Azure Key Vault for production secrets.

## Security Considerations

- JWT secret should be stored in environment variables or secure vaults
- Always use HTTPS in production
- Implement rate limiting for authentication endpoints
- Regular security audits and dependency updates
- Consider upgrading password hashing from SHA256 to BCrypt or Argon2

## Future Enhancements

- [ ] Role-based tenant access restrictions
- [ ] Property management module
- [ ] Rent payment tracking and invoicing
- [ ] Email notifications for lease expiry
- [ ] Document upload and management
- [ ] Reporting and analytics dashboard
- [ ] Multi-tenancy support
- [ ] Audit logging
- [ ] Real-time notifications with SignalR
- [ ] Mobile application API support

## Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

Please ensure:
- All tests pass
- Code follows existing style conventions
- New features include appropriate tests
- Documentation is updated


## Contact

Project Maintainer - Mbuso Nkosi
- Email: mbusonkosi26@gmail.com


## Acknowledgments

- Clean Architecture principles by Robert C. Martin
- Microsoft ASP.NET Core documentation
- AutoMapper library contributors
- Entity Framework Core team
- xUnit and testing framework contributors
