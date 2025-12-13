# Quick Command Reference

## Setup

```bash
# Quick setup (all in one)
./setup.sh

# Or manual setup
docker-compose up -d
docker exec -i taskmanagement-db psql -U postgres -d taskmanagement < src/TaskManagement.Infrastructure/Data/Scripts/001_create_tables.sql
docker exec -i taskmanagement-db psql -U postgres -d taskmanagement < src/TaskManagement.Infrastructure/Data/Scripts/002_seed_data.sql
```

## Docker Commands

```bash
# Start all services
docker-compose up -d

# Start only database
docker-compose up -d postgres

# Stop services
docker-compose down

# Stop and remove volumes (clean slate)
docker-compose down -v

# View logs
docker-compose logs -f

# View specific service logs
docker-compose logs -f api
docker-compose logs -f postgres

# Restart services
docker-compose restart

# Rebuild and start
docker-compose up -d --build
```

## Database Commands

```bash
# Connect to database
docker exec -it taskmanagement-db psql -U postgres -d taskmanagement

# Run SQL file
docker exec -i taskmanagement-db psql -U postgres -d taskmanagement < file.sql

# Backup database
docker exec taskmanagement-db pg_dump -U postgres taskmanagement > backup.sql

# Restore database
docker exec -i taskmanagement-db psql -U postgres -d taskmanagement < backup.sql
```

## .NET Commands

```bash
# Restore packages
dotnet restore

# Build solution
dotnet build

# Build specific project
dotnet build src/TaskManagement.API

# Run API locally
cd src/TaskManagement.API
dotnet run

# Run with specific environment
dotnet run --environment Development

# Watch mode (auto-reload on changes)
dotnet watch run
```

## Testing Commands

```bash
# Run all tests (automatically manages test database on port 5433)
dotnet test

# Run tests with output
dotnet test -v normal

# Run specific test project
dotnet test tests/TaskManagement.Domain.Tests

# Run specific test
dotnet test --filter "FullyQualifiedName~CreateTask_WithValidData_ShouldSucceed"

# Watch tests (auto-run on changes)
dotnet watch test
```

## API Testing with curl

```bash
# Register user
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"name":"John Doe","email":"john@example.com","password":"TestPassword123"}'

# Login
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"john@example.com","password":"TestPassword123"}'

# Get tasks (replace TOKEN)
curl -X GET http://localhost:5000/api/tasks \
  -H "Authorization: Bearer TOKEN"

# Create task (replace TOKEN)
curl -X POST http://localhost:5000/api/tasks \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer TOKEN" \
  -d '{"title":"New Task","description":"Description","dueDate":"2024-12-31T23:59:59Z"}'

# Update task (replace TOKEN and ID)
curl -X PUT http://localhost:5000/api/tasks/TASK_ID \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer TOKEN" \
  -d '{"title":"Updated","description":"Updated desc","dueDate":"2024-12-31T23:59:59Z","status":"InProgress"}'

# Delete task (replace TOKEN and ID)
curl -X DELETE http://localhost:5000/api/tasks/TASK_ID \
  -H "Authorization: Bearer TOKEN"
```

## Useful SQL Queries

```sql
-- List all users
SELECT id, name, email, created_at FROM users;

-- List all tasks with user info
SELECT t.*, u.name as user_name, u.email 
FROM tasks t 
JOIN users u ON t.user_id = u.id;

-- Count tasks by status
SELECT status, COUNT(*) 
FROM tasks 
GROUP BY status;

-- Find tasks by user email
SELECT t.* 
FROM tasks t 
JOIN users u ON t.user_id = u.id 
WHERE u.email = 'john@example.com';

-- Tasks due soon
SELECT * FROM tasks 
WHERE due_date < NOW() + INTERVAL '7 days' 
AND status != 'Completed';

-- Delete all data (careful!)
TRUNCATE tasks, users CASCADE;
```

## Development Workflow

```bash
# 1. Start database
docker-compose up -d postgres

# 2. Run API locally
cd src/TaskManagement.API
dotnet watch run

# 3. In another terminal, run tests
dotnet test --watch

# 4. Access Swagger
open http://localhost:5000/swagger
```

## Cleanup Commands

```bash
# Remove all containers and volumes
docker-compose down -v

# Remove build artifacts
dotnet clean

# Remove bin and obj folders
find . -name "bin" -type d -exec rm -rf {} +
find . -name "obj" -type d -exec rm -rf {} +

# Full cleanup and rebuild
docker-compose down -v
dotnet clean
dotnet build
docker-compose up -d
```

## Troubleshooting

```bash
# Check if port 5432 is in use
lsof -i :5432

# Check if port 5000 is in use
lsof -i :5000

# View Docker container details
docker inspect taskmanagement-db
docker inspect taskmanagement-api

# Check Docker logs for errors
docker-compose logs --tail=50

# Verify database connection
docker exec taskmanagement-db pg_isready -U postgres

# Test database connection string
docker exec taskmanagement-db psql -U postgres -d taskmanagement -c "SELECT 1;"
```

## Git Commands (if using git)

```bash
# Initialize repo
git init
git add .
git commit -m "Initial commit: Task Management System"

# Create .gitignore (already included)
# Commit after each feature
git add .
git commit -m "Add Domain layer with tests"
git commit -m "Add Application layer use cases"
git commit -m "Add Infrastructure with ADO.NET"
git commit -m "Add API layer with JWT auth"
```

## Performance Testing

```bash
# Install apache bench (if not installed)
# macOS: brew install httpd
# Linux: apt-get install apache2-utils

# Test login endpoint
ab -n 100 -c 10 -T 'application/json' \
  -p login.json \
  http://localhost:5000/api/auth/login

# Where login.json contains:
# {"email":"john@example.com","password":"TestPassword123"}
```

## Quick Links

- Swagger UI: http://localhost:5000/swagger
- API Base URL: http://localhost:5000/api
- PostgreSQL: localhost:5432 (user: postgres, pass: postgres, db: taskmanagement)

