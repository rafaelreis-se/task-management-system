# Task Management System

Fullstack task management application built with Clean Architecture and TDD for technical interview.

## Tech Stack

**Backend:**
- .NET 10.0 / ASP.NET Core Web API
- PostgreSQL with Docker
- ADO.NET (raw SQL - no ORM)
- JWT Authentication (manual implementation)
- BCrypt password hashing
- Clean Architecture (4 layers)
- xUnit for testing

**Frontend:**
- Coming soon (React/Vue)

## Quick Start

### Prerequisites
- .NET 10.0 SDK
- Docker and Docker Compose
- [just](https://github.com/casey/just) (optional, recommended)

### Backend Setup

```bash
cd backend

# Complete setup (database + migrations + seed)
just setup

# Start API
just run

# Run tests
just test

# Run tests with coverage
just test-coverage
```

Access Swagger UI: http://localhost:5000/swagger

### Test Credentials

```
Email: john@example.com
Password: TestPassword123
```

## Project Structure

```
task-management-system/
├── backend/              # .NET API
│   ├── src/             # Source code (4 layers)
│   ├── tests/           # Unit tests
│   ├── docs/            # Technical documentation
│   ├── justfile         # Command runner
│   └── docker-compose.yml
├── frontend/            # (Coming soon)
└── docs/                # General documentation
```

## Backend Architecture

Clean Architecture with 4 layers:

1. **Domain Layer** - Business entities and rules
2. **Application Layer** - Use cases and DTOs
3. **Infrastructure Layer** - Data access (ADO.NET), Auth (JWT, BCrypt)
4. **API Layer** - Controllers, middleware, dependency injection

### Key Features

- Test-Driven Development (TDD)
- Manual implementations (no Entity Framework, Dapper, or MediatR)
- Comprehensive unit tests (33 tests, 92.4% coverage)
- Raw SQL with parameterized queries
- JWT token generation
- BCrypt password hashing
- Docker containerization
- Automated commands with justfile

## API Endpoints

### Authentication (Public)
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login and get JWT token

### Tasks (Protected)
- `GET /api/tasks` - Get user's tasks
- `GET /api/tasks/{id}` - Get specific task
- `POST /api/tasks` - Create new task
- `PUT /api/tasks/{id}` - Update task
- `DELETE /api/tasks/{id}` - Delete task

## Testing

```bash
cd backend

# Run all tests
just test

# Run with coverage report
just test-coverage

# Watch mode
just test-watch
```

**Test Results:**
- 33 tests passing (100%)
- 92.4% code coverage
- Domain: 11 tests
- Application: 18 tests
- Infrastructure: 4 tests

## Documentation

Detailed documentation available in `/backend/docs/`:
- [Architecture](backend/docs/ARCHITECTURE.md)
- [Database](backend/docs/DATABASE.md)
- [API Reference](backend/docs/API.md)
- [TDD Approach](backend/docs/TDD.md)
- [Docker Setup](backend/docs/DOCKER.md)
- [GenAI Usage](backend/docs/GENAI_USAGE.md)

## Development Commands

```bash
cd backend

# See all available commands
just

# Development
just run-watch      # API with auto-reload
just test-watch     # Tests with auto-reload
just format         # Format code

# Database
just db-up          # Start PostgreSQL
just db-setup       # Run migrations + seed
just db-connect     # Connect to DB CLI

# Docker
just docker-up      # Start all services
just docker-logs    # View logs
just docker-clean   # Clean everything
```

## Technical Highlights

### Clean Architecture
- Clear separation of concerns
- Domain-driven design
- Dependency inversion principle
- Infrastructure independent

### TDD Methodology
- Tests written before implementation
- Red-Green-Refactor cycle
- Meaningful test names
- Quality over quantity

### Security
- JWT authentication
- BCrypt password hashing
- Parameterized SQL queries (SQL injection prevention)
- Input validation at multiple layers

### Code Quality
- Clean Code principles
- SOLID principles
- No over-engineering
- YAGNI (You Aren't Gonna Need It)

## Project Timeline

This project was developed as part of a technical interview process, demonstrating:
- Backend development skills (.NET, C#)
- Clean Architecture implementation
- Test-Driven Development
- Manual low-level implementations
- Docker containerization
- API design
- Security best practices
- GenAI tool usage with critical thinking

## License

This is a technical interview project.

## Author

Rafael Reis
- GitHub: [@rafaelreis-se](https://github.com/rafaelreis-se)

