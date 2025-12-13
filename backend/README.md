# Task Management System - Backend

Technical interview project demonstrating Clean Architecture, TDD, and manual data access in .NET.

## Overview

A RESTful API for task management built with:
- **.NET 8** and Clean Architecture
- **Test-Driven Development** (TDD) methodology
- **ADO.NET** for data access (no ORMs)
- **JWT** authentication (manual implementation)
- **PostgreSQL** database
- **Docker** for local development

## User Story

"As a system user, I need to manage my daily tasks by creating, viewing, editing, and deleting them. Each task has a title, description, status, and due date. I also need to register and log in to access my tasks securely."

## Tech Stack

- .NET 8.0
- ASP.NET Core Web API
- ADO.NET (raw SQL)
- SQL Server / PostgreSQL
- JWT Authentication
- xUnit

## Project Structure

This project follows Clean Architecture with four layers:

- **Domain**: Core business entities and interfaces
- **Application**: Use cases and business logic
- **Infrastructure**: Database access and external services
- **API**: HTTP endpoints and controllers

## Getting Started

### Prerequisites
- .NET 10.0 SDK (or .NET 8.0 LTS)
- Docker and Docker Compose
- [just](https://github.com/casey/just) (optional, recommended)

### Quick Start with Just

```bash
# Install just (if not installed)
brew install just  # macOS
# or: cargo install just

# Complete setup
just setup

# Start API
just run

# Run tests with coverage
just test-coverage
```

### Running with Docker

Start PostgreSQL and API:
```bash
docker-compose up -d
# or: just docker-up
```

Access Swagger UI: http://localhost:5000/swagger

### Running Locally

Start only PostgreSQL:
```bash
docker-compose up -d postgres
# or: just db-up
```

Run API:
```bash
cd src/TaskManagement.API
dotnet run
# or: just run
```

### Running Tests
```bash
# Run all tests
dotnet test
# or: just test

# Run tests with coverage report in terminal
./show-coverage.sh
# or: just test-coverage
```

### Available Commands

See all available commands:
```bash
just --list
```

## API Endpoints

### Authentication (Public)
- POST /api/auth/register
- POST /api/auth/login

### Tasks (Protected)
- GET /api/tasks
- GET /api/tasks/{id}
- POST /api/tasks
- PUT /api/tasks/{id}
- DELETE /api/tasks/{id}

## Test Credentials

All test users have the password: **TestPassword123**

- john@example.com (John Doe) - Has 3 tasks
- jane@example.com (Jane Smith) - Has 2 tasks
- bob@example.com (Bob Wilson) - Has 2 tasks

## Database Setup

After starting Docker services, run the database scripts:

```bash
# Create tables
docker exec -i taskmanagement-db psql -U postgres -d taskmanagement < src/TaskManagement.Infrastructure/Data/Scripts/001_create_tables.sql

# Seed data
docker exec -i taskmanagement-db psql -U postgres -d taskmanagement < src/TaskManagement.Infrastructure/Data/Scripts/002_seed_data.sql
```

## Documentation

- **[SETUP.md](SETUP.md)** - Complete setup and usage guide
- **[docs/ARCHITECTURE.md](docs/ARCHITECTURE.md)** - Architecture overview and design decisions
- **[docs/DATABASE.md](docs/DATABASE.md)** - Database schema and relationships
- **[docs/API.md](docs/API.md)** - API endpoints documentation
- **[docs/TDD.md](docs/TDD.md)** - Testing strategy and approach
- **[docs/GENAI_USAGE.md](docs/GENAI_USAGE.md)** - How GenAI tools were used in development
- **[docs/DOCKER.md](docs/DOCKER.md)** - Docker setup and commands

## Project Highlights

### Clean Architecture
- **Domain Layer**: Business entities and rules with zero dependencies
- **Application Layer**: Use cases and business logic
- **Infrastructure Layer**: Data access with ADO.NET, JWT, BCrypt
- **API Layer**: Controllers, middleware, and authentication

### Test-Driven Development
- Tests written before implementation (Red-Green-Refactor)
- Unit tests for all layers with Moq for mocking
- Integration tests for API endpoints
- Focus on quality over quantity

### Manual Implementations
- **No Entity Framework or Dapper**: Raw SQL with ADO.NET
- **No MediatR**: Direct use case invocation
- **No ASP.NET Identity**: Manual JWT and BCrypt implementation

### Key Features
- User registration and JWT authentication
- CRUD operations for tasks
- Users can only access their own tasks
- Proper validation and error handling
- Global exception middleware
- Swagger documentation with Bearer auth

## Technical Stack

- .NET 8.0 / C# 12
- ASP.NET Core Web API
- PostgreSQL with Npgsql
- BCrypt.Net for password hashing
- System.IdentityModel.Tokens.Jwt
- xUnit, Moq for testing
- Docker & Docker Compose

## Contributing

This is a technical interview project. For information on how it was developed with GenAI tools, see [docs/GENAI_USAGE.md](docs/GENAI_USAGE.md).

