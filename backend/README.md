# Task Management System - Backend

RESTful API for task management built with .NET 10, Clean Architecture, and Test-Driven Development.

## Tech Stack

- .NET 10 / ASP.NET Core Web API
- PostgreSQL with Npgsql (ADO.NET)
- JWT authentication (manual implementation)
- BCrypt for password hashing
- xUnit for testing
- Docker for local development

## Project Structure

Clean Architecture with four layers:

```
src/
├── TaskManagement.Domain/          # Core business entities and interfaces
├── TaskManagement.Application/     # Use cases and business logic
├── TaskManagement.Infrastructure/  # Database access, JWT, BCrypt
└── TaskManagement.API/             # HTTP endpoints and controllers

tests/
├── TaskManagement.Domain.Tests/
├── TaskManagement.Application.Tests/
├── TaskManagement.Infrastructure.Tests/
└── TaskManagement.Integration.Tests/
```

## Getting Started

From the **project root** (not backend folder):

```bash
just setup    # Install dependencies
just dev      # Start database + API + frontend
just test     # Run all tests
```

See main [README.md](../README.md) for complete setup instructions.

## API Endpoints

### Authentication (Public)
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login and get JWT token

### Tasks (Protected - requires Bearer token)
- `GET /api/tasks` - Get all tasks for current user
- `GET /api/tasks/{id}` - Get task by ID
- `POST /api/tasks` - Create new task
- `PUT /api/tasks/{id}` - Update task
- `DELETE /api/tasks/{id}` - Delete task

## Test Credentials

All test users have password: **TestPassword123**

- john@example.com (3 tasks)
- jane@example.com (2 tasks)
- bob@example.com (2 tasks)

## Key Implementation Details

### No ORMs or Frameworks
- **No Entity Framework or Dapper**: Raw SQL with ADO.NET/Npgsql
- **No MediatR**: Direct use case invocation
- **No ASP.NET Identity**: Manual JWT and BCrypt

### Clean Architecture
- **Domain**: Zero external dependencies, business rules
- **Application**: Use cases, DTOs, interfaces
- **Infrastructure**: Repositories, auth services
- **API**: Controllers, middleware

### Test-Driven Development
- Tests written before implementation
- Unit tests for all layers
- Integration tests with real PostgreSQL (isolated on port 5433)

## Documentation

- [Architecture](./docs/ARCHITECTURE.md)
- [Database Schema](./docs/DATABASE.md)
- [API Endpoints](./docs/API.md)
- [TDD Approach](./docs/TDD.md)
- [GenAI Usage](./docs/GENAI_USAGE.md)
- [Docker Setup](./docs/DOCKER.md)
