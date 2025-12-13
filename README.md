# Task Management System

Full-stack web application with .NET Core backend and React frontend, built following Clean Architecture principles and Test-Driven Development (TDD).

## Project Overview

This is a technical interview exercise demonstrating:
- Clean Architecture (backend)
- Test-Driven Development (TDD)
- RESTful API design
- JWT authentication
- Modern frontend with React & TypeScript
- Full CRUD operations
- Responsive UI design

## Architecture

### Backend (.NET Core 8)
- **Clean Architecture** with separation of concerns
- **Layers**: API -> Application -> Domain -> Infrastructure
- **No Entity Framework or Dapper** - Custom data access using Npgsql
- **JWT Authentication** with secure password hashing
- **Unit Tests** with xUnit (72 comprehensive tests)
- **PostgreSQL** database

### Frontend (React + TypeScript)
- **Component-based architecture**
- **Material-UI** for consistent design
- **React Router** for navigation
- **Axios** for API communication
- **React Hook Form** for form handling
- **Responsive design** for all devices

## Quick Start

### Prerequisites
- .NET 8 SDK
- Node.js 18+
- PostgreSQL 14+
- Docker (optional, for containerized setup)

### Option 1: Manual Setup

#### Backend
```bash
cd backend
./setup.sh  # Initialize database and seed data
dotnet run --project src/TaskManagement.API
```
Backend will be available at: http://localhost:5000

#### Frontend
```bash
cd frontend
npm install
npm run dev
```
Frontend will be available at: http://localhost:5173

### Option 2: Docker Setup (Recommended)

```bash
cd backend
docker-compose up
```

Then start the frontend:
```bash
cd frontend
npm install
npm run dev
```

## Demo Credentials

```
Email: john@example.com
Password: TestPassword123
```

Additional test users are seeded in the database. See `backend/src/TaskManagement.Infrastructure/Data/seed.sql`

## Features

### User Authentication
- Register new account with validation
- Login with JWT token
- Secure password hashing (BCrypt)
- Protected API endpoints
- Automatic logout on token expiration

### Task Management
- Create tasks with title, description, and due date
- View all tasks for authenticated user
- Update task details and status
- Delete tasks
- Filter by status (Pending, In Progress, Completed)
- Visual indicators for overdue tasks
- Task counts by status

### User Experience
- Clean, modern UI with Material Design
- Responsive layout for mobile and desktop
- Real-time form validation
- Loading states and error handling
- Success/error notifications
- Confirmation dialogs for destructive actions

## Project Structure

```
.
├── backend/                 # .NET Core API
│   ├── src/
│   │   ├── TaskManagement.API/          # Web API layer
│   │   ├── TaskManagement.Application/  # Use cases & DTOs
│   │   ├── TaskManagement.Domain/       # Entities & business logic
│   │   └── TaskManagement.Infrastructure/ # Data access & auth
│   ├── tests/              # Unit tests
│   ├── docs/               # Documentation
│   └── docker-compose.yml  # Docker setup
│
└── frontend/               # React application
    ├── src/
    │   ├── components/     # Reusable UI components
    │   ├── pages/         # Route pages
    │   ├── services/      # API integration
    │   ├── context/       # State management
    │   └── types/         # TypeScript definitions
    └── docs/              # Frontend documentation
```

## Testing

### Backend Tests
```bash
just test              # Runs all tests with isolated test database
```

The test database runs on port 5433 (isolated from dev on 5432) and is automatically managed.

## Documentation

### Backend
- [Architecture](./backend/docs/ARCHITECTURE.md)
- [API Endpoints](./backend/docs/API.md)
- [Database Schema](./backend/docs/DATABASE.md)
- [Docker Setup](./backend/docs/DOCKER.md)
- [TDD Approach](./backend/docs/TDD.md)
- [GenAI Usage](./backend/docs/GENAI_USAGE.md)

### Frontend
- [Architecture](./frontend/docs/ARCHITECTURE.md)
- [API Integration](./frontend/docs/API_INTEGRATION.md)
- [Implementation Plan](./frontend/IMPLEMENTATION_PLAN.md)

### Setup Guides
- [Backend Setup](./backend/SETUP.md)
- [Frontend Setup](./frontend/README.md)

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login and get JWT token

### Tasks (Protected)
- `GET /api/tasks` - Get all tasks for user
- `GET /api/tasks/{id}` - Get task by ID
- `POST /api/tasks` - Create new task
- `PUT /api/tasks/{id}` - Update task
- `DELETE /api/tasks/{id}` - Delete task

## Technical Highlights

### Clean Architecture
- Clear separation of concerns
- Business logic in Domain layer
- Infrastructure independence
- Testable components

### Test-Driven Development
- Tests written before implementation
- 72 comprehensive tests covering all layers
- Unit tests for all layers
- Integration tests for API endpoints

### Security
- JWT token authentication
- BCrypt password hashing
- Protected API endpoints
- CORS configuration
- Input validation

### Modern Frontend
- TypeScript for type safety
- React Hooks for state management
- Material-UI components
- Responsive design
- Error boundaries

### Database Design
- Normalized schema
- Foreign key relationships
- Indexes for performance
- Migration scripts
- Seed data for testing

## Development Tools

### Backend
- **.NET 8** - Latest LTS framework
- **xUnit** - Unit testing
- **Npgsql** - PostgreSQL driver
- **BCrypt.Net** - Password hashing
- **JWT Bearer** - Authentication

### Frontend
- **Vite** - Fast build tool
- **React 18** - UI library
- **TypeScript** - Type safety
- **Material-UI** - Component library
- **Axios** - HTTP client

### Infrastructure
- **PostgreSQL** - Relational database
- **Docker** - Containerization
- **Docker Compose** - Multi-container setup

## GenAI Tool Usage

This project was developed with the assistance of AI coding tools, demonstrating:

### Prompting Strategy
- Clear, specific prompts with context
- Incremental development approach
- Validation of generated code
- Critical thinking about suggestions

### Code Validation
- Testing all AI-generated code
- Reviewing for best practices
- Ensuring Clean Architecture principles
- Verifying security implementations

### Edge Cases Handled
- Null reference checks
- Validation for all inputs
- Error handling and logging
- Concurrent request handling
- Date/time handling across timezones

See [GenAI Usage Documentation](./backend/docs/GENAI_USAGE.md) for detailed examples.

## Presentation Checklist

- [x] User story and requirements
- [x] Architecture overview
- [x] Live demo (login, CRUD operations)
- [x] Code walkthrough
- [x] Test suite review (72 tests)
- [x] Database schema explanation
- [x] Security implementation
- [x] GenAI tool usage examples
- [x] Responsive design showcase
- [x] Error handling demonstration

## Troubleshooting

### Backend Issues
- Check PostgreSQL is running
- Verify connection string in `appsettings.json`
- Run `./setup.sh` to initialize database
- Check port 5000 is available

### Frontend Issues
- Ensure backend is running on port 5000
- Check Node.js version (18+)
- Clear browser cache and localStorage
- Verify API URL in frontend constants

### Database Issues
- Check PostgreSQL service status
- Verify credentials in connection string
- Run migration scripts manually if needed
- Check database logs for errors

## Future Enhancements

Potential improvements for production:
- [ ] Refresh token mechanism
- [ ] Task categories and tags
- [ ] Task search functionality
- [ ] Email notifications
- [ ] Task comments and collaboration
- [ ] File attachments
- [ ] Advanced filtering and sorting
- [ ] Performance monitoring
- [ ] Automated frontend tests
- [ ] CI/CD pipeline

## Author

Rafael Reis

## License

This project is created for interview purposes.

---

**Tech Stack**: .NET 8 | React 18 | TypeScript | PostgreSQL | Material-UI | Docker

**Architecture**: Clean Architecture | TDD | RESTful API | JWT Auth | Responsive Design
