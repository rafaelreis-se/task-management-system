# Task Management System

Full-stack task management application built with .NET 10 and React, following Clean Architecture principles and Test-Driven Development (TDD).

## User Story

> **As a** busy professional,  
> **I need** a simple way to manage my daily tasks with titles, descriptions, and due dates,  
> **So that** I can stay organized, track my progress, and ensure nothing important is forgotten.

See [USER_STORY.md](./USER_STORY.md) for complete acceptance criteria and business rules.

## Prerequisites

Before running this project, ensure you have the following installed:

| Tool | Version | Installation |
|------|---------|--------------|
| Docker | Latest | [docker.com](https://www.docker.com/get-started) |
| .NET SDK | 10.0+ | [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/10.0) |
| Node.js | 18+ | [nodejs.org](https://nodejs.org/) |
| just | Latest | [github.com/casey/just](https://github.com/casey/just#installation) |

### Verify Installation

```bash
docker --version    # Docker version 24.x or higher
dotnet --version    # 10.0.x
node --version      # v18.x or higher
just --version      # just 1.x
```

## Quick Start

### 1. Clone and Setup

```bash
git clone <repository-url>
cd ballastlane
just setup
```

This installs all dependencies (.NET packages, npm modules).

### 2. Start the Application

```bash
just dev
```

This command will:
- Start PostgreSQL database (via Docker)
- Initialize database schema
- Seed demo data
- Start backend API on http://localhost:5001
- Start frontend on http://localhost:5173

### 3. Open the Application

Open your browser at **http://localhost:5173**

## Demo Credentials

The application comes with seeded test data for demo purposes:

| User | Email | Password |
|------|-------|----------|
| John Doe | john@example.com | TestPassword123 |
| Jane Smith | jane@example.com | TestPassword123 |
| Bob Wilson | bob@example.com | TestPassword123 |

Each user has pre-created tasks to demonstrate the application.

## Available Commands

```bash
just setup      # First-time setup (install dependencies)
just dev        # Start all services for development/demo
just stop       # Stop all running services
just test       # Run all tests (unit + integration)
just e2e        # Run end-to-end tests
just build      # Build all projects
```

## Project Structure

```
ballastlane/
├── backend/                    # .NET 10 API
│   ├── src/
│   │   ├── TaskManagement.API/           # REST API controllers
│   │   ├── TaskManagement.Application/   # Use cases and DTOs
│   │   ├── TaskManagement.Domain/        # Entities and business rules
│   │   └── TaskManagement.Infrastructure/# Data access and auth
│   └── tests/                  # Unit and integration tests
├── frontend/                   # React + TypeScript
│   └── src/
│       ├── components/         # Reusable UI components
│       ├── pages/              # Route pages
│       ├── services/           # API integration
│       └── context/            # State management
├── e2e/                        # Cypress E2E tests
└── justfile                    # Task runner commands
```

## Technology Stack

### Backend
- .NET 10 / ASP.NET Core Web API
- PostgreSQL 16 (via Docker)
- Custom data access with Npgsql (no EF/Dapper)
- JWT authentication with BCrypt password hashing
- xUnit for testing

### Frontend
- React 18 + TypeScript
- Material-UI (MUI) component library
- React Router for navigation
- Axios for API communication
- React Hook Form for form handling

## Architecture

This project follows **Clean Architecture** principles:

- **Domain Layer**: Core business entities and rules
- **Application Layer**: Use cases, DTOs, and interfaces
- **Infrastructure Layer**: Database access, external services
- **API Layer**: Controllers and HTTP handling

Key design decisions:
- No Entity Framework, Dapper, or MediatR (as per requirements)
- Business logic isolated in Domain layer
- DTOs for API contracts (entities never exposed)
- Repository pattern for data access

## Testing

### Unit and Integration Tests

```bash
just test
```

Runs all .NET tests (unit + integration) with an isolated test database:

- **Domain Tests**: Entity validation and business rules
- **Application Tests**: Use case logic
- **Infrastructure Tests**: Auth services (JWT, password hashing)
- **Integration Tests**: Repository operations with real database

### E2E Tests (Cypress)

```bash
just e2e
```

Cypress is a JavaScript-based end-to-end testing framework that runs tests directly in the browser. I chose it to validate that the frontend and backend work correctly together, simulating real user interactions like login, creating tasks, and navigating through the application.

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login (returns JWT token)

### Tasks (Protected - requires JWT)
- `GET /api/tasks` - Get all tasks for current user
- `GET /api/tasks/{id}` - Get task by ID
- `POST /api/tasks` - Create new task
- `PUT /api/tasks/{id}` - Update task
- `DELETE /api/tasks/{id}` - Delete task

## Features

- User registration and authentication
- Create, read, update, delete tasks
- Task status management (Pending, In Progress, Completed)
- Due date tracking with overdue indicators
- Filter tasks by status
- Responsive design for mobile and desktop

## Troubleshooting

### Port already in use

```bash
just stop        # Stop all services
just dev         # Start again
```

### Database connection issues

```bash
docker ps        # Check if PostgreSQL container is running
just stop        # Stop everything
just dev         # Restart
```

### Clean start

```bash
just stop
cd backend && docker-compose down -v   # Remove database volume
just dev                                # Fresh start with new seed data
```

## Documentation

Additional documentation is available in the project:

- [Backend Architecture](./backend/docs/ARCHITECTURE.md)
- [API Documentation](./backend/docs/API.md)
- [Database Schema](./backend/docs/DATABASE.md)
- [TDD Approach](./backend/docs/TDD.md)
- [GenAI Usage](./backend/docs/GENAI_USAGE.md)

## GenAI Development Approach

This project was developed with AI assistance (Cursor IDE). To provide context and maintain consistency, I created configuration files that describe the project requirements, constraints, and coding standards:

### Cursor Rules (`.cursorrules`)

These files act as "pre-prompts" that give the AI context about the project before each interaction. Each folder has its own rules file with specific context:

| File | Purpose |
|------|---------|
| `backend/.cursorrules` | Backend-specific context: Clean Architecture layers, .NET conventions, TDD approach, business rules for tasks and users, authentication requirements, code standards |
| `frontend/.cursorrules` | Frontend-specific context: React patterns, component structure, state management rules, API integration guidelines, code standards |

When working in a specific folder, Cursor automatically loads the corresponding rules file, ensuring the AI follows the right conventions for that part of the codebase.

### Backend Documentation (`backend/docs/`)

Technical reference documents that I used to maintain consistency:

| Document | Description |
|----------|-------------|
| `ARCHITECTURE.md` | Detailed explanation of Clean Architecture implementation and layer responsibilities |
| `API.md` | API endpoint specifications, request/response formats, and authentication flow |
| `DATABASE.md` | PostgreSQL schema design, table relationships, and SQL scripts |
| `TDD.md` | Test-Driven Development methodology and testing strategy |
| `GENAI_USAGE.md` | Examples of prompts used and how AI suggestions were validated |

These documents served as a knowledge base that I could reference during development and share with the AI to ensure generated code followed the established patterns.

### Implementation Plans

Before starting development, I asked Cursor to generate detailed implementation plans for both backend and frontend. These plans were reviewed and refined until they accurately reflected the architecture, phases, and approach I wanted to follow:

| Plan | Description |
|------|-------------|
| [backend/IMPLEMENTATION_PLAN.md](./backend/IMPLEMENTATION_PLAN.md) | Detailed TDD phases, layer-by-layer development approach, testing strategy, and technical decisions for the .NET API |
| [frontend/IMPLEMENTATION_PLAN.md](./frontend/IMPLEMENTATION_PLAN.md) | Component structure, API integration approach, and development phases for the React frontend |

These plans served as a roadmap during development, ensuring a systematic approach to building the application.

## Author

Rafael Reis

---

**Stack**: .NET 10 | React 18 | TypeScript | PostgreSQL | Material-UI | Docker
