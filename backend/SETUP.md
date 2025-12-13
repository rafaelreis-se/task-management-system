# Complete Setup Guide

## Prerequisites

- .NET 8.0 SDK
- Docker and Docker Compose
- Git

## Quick Start (5 minutes)

### 1. Start Services

```bash
# Start PostgreSQL and API
docker-compose up -d
```

Wait for services to be healthy (check with `docker-compose ps`)

### 2. Setup Database

```bash
# Create tables
docker exec -i taskmanagement-db psql -U postgres -d taskmanagement < src/TaskManagement.Infrastructure/Data/Scripts/001_create_tables.sql

# Seed test data
docker exec -i taskmanagement-db psql -U postgres -d taskmanagement < src/TaskManagement.Infrastructure/Data/Scripts/002_seed_data.sql
```

### 3. Access the Application

- **Swagger UI**: http://localhost:5000/swagger
- **API Base**: http://localhost:5000/api

### 4. Test with Swagger

1. Go to http://localhost:5000/swagger
2. Use `/api/auth/login` endpoint
3. Login credentials:
   - Email: `john@example.com`
   - Password: `TestPassword123`
4. Copy the token from response
5. Click "Authorize" button at top
6. Enter: `Bearer {your-token}`
7. Now you can test protected endpoints

## Development Setup

### Run Locally (without Docker)

1. **Start only PostgreSQL:**
```bash
docker-compose up -d postgres
```

2. **Run the API:**
```bash
cd src/TaskManagement.API
dotnet run
```

3. **Run tests:**
```bash
dotnet test
```

### Database Management

**Connect to database:**
```bash
docker exec -it taskmanagement-db psql -U postgres -d taskmanagement
```

**Common queries:**
```sql
-- List all users
SELECT * FROM users;

-- List all tasks
SELECT * FROM tasks;

-- Reset database (careful!)
DROP TABLE IF EXISTS tasks CASCADE;
DROP TABLE IF EXISTS users CASCADE;
```

## Project Structure

```
backend/
├── src/
│   ├── TaskManagement.Domain/          # Core business logic
│   ├── TaskManagement.Application/     # Use cases
│   ├── TaskManagement.Infrastructure/  # Data access, Auth
│   └── TaskManagement.API/            # Controllers, Middleware
├── tests/
│   ├── TaskManagement.Domain.Tests/
│   ├── TaskManagement.Application.Tests/
│   ├── TaskManagement.Infrastructure.Tests/
│   └── TaskManagement.API.Tests/
├── docs/                               # Documentation
├── docker-compose.yml
└── README.md
```

## Testing Strategy

### Unit Tests
```bash
# Test specific layer
dotnet test tests/TaskManagement.Domain.Tests
dotnet test tests/TaskManagement.Application.Tests
```

### Integration Tests
```bash
dotnet test tests/TaskManagement.API.Tests
```

### All Tests
```bash
dotnet test
```

## Common Issues

### Port already in use
```bash
# Change ports in docker-compose.yml
ports:
  - "5433:5432"  # PostgreSQL
  - "5001:8080"  # API
```

### Database connection failed
```bash
# Check if PostgreSQL is running
docker-compose ps

# Check logs
docker-compose logs postgres
```

### Can't connect to API
```bash
# Check API logs
docker-compose logs api

# Or if running locally
cd src/TaskManagement.API
dotnet run --environment Development
```

## API Endpoints

### Authentication (Public)
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login and get JWT token

### Tasks (Protected - requires Bearer token)
- `GET /api/tasks` - Get all tasks for current user
- `POST /api/tasks` - Create new task
- `PUT /api/tasks/{id}` - Update task
- `DELETE /api/tasks/{id}` - Delete task

## Example API Calls

### Register
```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "name": "New User",
    "email": "newuser@example.com",
    "password": "SecurePass123"
  }'
```

### Login
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john@example.com",
    "password": "TestPassword123"
  }'
```

### Create Task (with token)
```bash
curl -X POST http://localhost:5000/api/tasks \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "title": "New Task",
    "description": "Task description",
    "dueDate": "2024-12-31T23:59:59Z"
  }'
```

## Clean Architecture Layers

### Domain Layer
- Pure business logic
- No external dependencies
- Entities: Task, User
- Interfaces: ITaskRepository, IUserRepository

### Application Layer
- Use cases (CreateTask, LoginUser, etc.)
- DTOs for data transfer
- Depends only on Domain

### Infrastructure Layer
- Database access with ADO.NET
- JWT token generation
- Password hashing with BCrypt
- Implements Domain interfaces

### API Layer
- HTTP endpoints
- Authentication/Authorization
- Error handling middleware
- Swagger documentation

## Next Steps

1. Review code in `src/` folders
2. Run tests to understand TDD approach
3. Read `docs/GENAI_USAGE.md` for GenAI tool usage
4. Check `docs/ARCHITECTURE.md` for design decisions
5. Explore API with Swagger UI

