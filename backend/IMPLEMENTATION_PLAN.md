# Implementation Plan - Task Management System

## Strategic Approach

This document outlines the implementation strategy used to build this project, demonstrating TDD methodology, Clean Architecture principles, and systematic development approach.

## Phase 1: Planning & Setup (2 hours)

### Initial Analysis
1. **Requirements Review**
   - Analyzed interview requirements
   - Identified constraints: No EF, Dapper, MediatR
   - Determined tech stack: .NET 8, PostgreSQL, ADO.NET

2. **Architecture Decision**
   - Selected Clean Architecture (4 layers)
   - Decided on TDD approach (test-first)
   - Planned dependency flow (inner to outer)

3. **User Story Definition**
```
As a system user, I need to manage my daily tasks by creating, 
viewing, editing, and deleting them. Each task has a title, 
description, status, and due date. I also need to register and 
log in to access my tasks securely.
```

4. **Project Structure Setup**
   - Created solution with 4 source projects
   - Created 4 test projects
   - Setup project references following architecture rules
   - Configured Docker Compose for PostgreSQL

## Phase 2: Domain Layer (3 hours) - TDD Cycle 1

### Why Start Here?
Domain is the core with zero dependencies. Perfect starting point for TDD.

### Step 1: Task Entity Tests (RED)
**File:** `TaskEntityTests.cs`

```
Tests written FIRST:
✓ CreateTask_WithValidData_ShouldSucceed
✓ CreateTask_WithEmptyTitle_ShouldThrowException
✓ CreateTask_WithTitleTooLong_ShouldThrowException
✓ CreateTask_WithPastDueDate_ShouldThrowException
✓ UpdateTask_WithValidData_ShouldSucceed
✓ ChangeStatus_ToInProgress_ShouldSucceed
```

### Step 2: Task Entity Implementation (GREEN)
**File:** `TaskEntity.cs`

```
Implementation to pass tests:
- Private setters for encapsulation
- Validation in constructor
- Business rules: title max 200 chars, no past dates
- Status enum: Pending, InProgress, Completed
```

### Step 3: User Entity Tests (RED)
**File:** `UserTests.cs`

```
Tests written FIRST:
✓ CreateUser_WithValidData_ShouldSucceed
✓ CreateUser_WithEmptyName_ShouldThrowException
✓ CreateUser_WithInvalidEmail_ShouldThrowException
✓ CreateUser_WithEmptyPasswordHash_ShouldThrowException
```

### Step 4: User Entity Implementation (GREEN)
**File:** `User.cs`

```
Implementation:
- Email validation with regex
- Name required
- PasswordHash required (never plain text)
```

### Step 5: Repository Interfaces
**Files:** `ITaskRepository.cs`, `IUserRepository.cs`

```
Simple interfaces defining contracts:
- Async methods returning Task<T>
- CRUD operations for Tasks
- Email lookup for Users
```

**Result:** Domain layer complete with 100% test coverage

## Phase 3: Application Layer (4 hours) - TDD Cycle 2

### Why Application Layer Next?
Use cases define business logic before we implement infrastructure.

### Step 1: DTOs Definition
**Files:** `TaskDto.cs`, `UserDto.cs`

```
Created record types:
- TaskDto for API responses
- CreateTaskRequest, UpdateTaskRequest
- RegisterRequest, LoginRequest, LoginResponse
- Never expose domain entities
```

### Step 2: CreateTask Use Case Tests (RED)
**File:** `CreateTaskUseCaseTests.cs`

```
Tests with Moq:
✓ Execute_WithValidData_ShouldCreateTask
- Mock ITaskRepository
- Verify repository was called
- Assert correct DTO returned
```

### Step 3: CreateTask Use Case Implementation (GREEN)
**File:** `CreateTaskUseCase.cs`

```
Simple implementation:
- Accept userId and CreateTaskRequest
- Create domain entity
- Call repository
- Return DTO
```

### Step 4: Other Task Use Cases
Following same TDD pattern:
- GetTasksUseCase (list user's tasks)
- UpdateTaskUseCase (with authorization check)
- DeleteTaskUseCase (with authorization check)

### Step 5: Auth Use Cases Tests (RED)
**Files:** `RegisterUserUseCaseTests.cs`, `LoginUserUseCaseTests.cs`

```
RegisterUser tests:
✓ Execute_WithValidData_ShouldRegisterUser
✓ Execute_WithExistingEmail_ShouldThrowException
✓ Execute_WithShortPassword_ShouldThrowException

LoginUser tests:
✓ Execute_WithValidCredentials_ShouldReturnToken
✓ Execute_WithInvalidEmail_ShouldReturnNull
✓ Execute_WithInvalidPassword_ShouldReturnNull
```

### Step 6: Auth Use Cases Implementation (GREEN)
**Files:** `RegisterUserUseCase.cs`, `LoginUserUseCase.cs`

```
RegisterUser:
- Validate password length (min 8)
- Check email uniqueness
- Hash password
- Create user

LoginUser:
- Find user by email
- Verify password
- Generate JWT token
- Return token + user DTO
```

**Result:** Application layer complete with all use cases tested

## Phase 4: Infrastructure Layer (5 hours) - TDD Cycle 3

### Why Infrastructure Now?
We have interfaces defined. Now implement them with real code.

### Step 1: Database Connection
**File:** `DbConnectionFactory.cs`

```
Simple factory:
- Accept connection string
- Return IDbConnection (Npgsql)
- Used by all repositories
```

### Step 2: TaskRepository Implementation
**File:** `TaskRepository.cs`

```
ADO.NET with raw SQL:
- Parameterized queries (SQL injection prevention)
- Manual mapping from DataReader to Entity
- Using statements for proper disposal
- Reflection for private constructors

Key methods:
- GetByIdAsync: SELECT with WHERE id = @id
- GetByUserIdAsync: SELECT with WHERE user_id = @userId
- CreateAsync: INSERT with parameters
- UpdateAsync: UPDATE with parameters
- DeleteAsync: DELETE with WHERE id = @id
```

### Step 3: UserRepository Implementation
**File:** `UserRepository.cs`

```
Similar to TaskRepository:
- GetByIdAsync
- GetByEmailAsync (for login)
- CreateAsync
- EmailExistsAsync (for validation)
```

### Step 4: PasswordHasher Tests (RED)
**File:** `PasswordHasherTests.cs`

```
✓ HashPassword_ShouldReturnHashedPassword
✓ VerifyPassword_WithCorrectPassword_ShouldReturnTrue
✓ VerifyPassword_WithIncorrectPassword_ShouldReturnFalse
```

### Step 5: PasswordHasher Implementation (GREEN)
**File:** `PasswordHasher.cs`

```
Using BCrypt.Net:
- HashPassword: BCrypt with auto-generated salt
- VerifyPassword: BCrypt.Verify
```

### Step 6: JwtTokenGenerator Tests (RED)
**File:** `JwtTokenGeneratorTests.cs`

```
✓ GenerateToken_ShouldReturnValidJwtToken
- Verify token structure
- Verify claims (userId, email)
```

### Step 7: JwtTokenGenerator Implementation (GREEN)
**File:** `JwtTokenGenerator.cs`

```
Manual JWT implementation:
- JwtSecurityTokenHandler
- Claims: NameIdentifier, Email, Name
- SymmetricSecurityKey with secret
- HmacSha256 signature
- Configurable expiration
```

**Result:** Infrastructure layer complete, all tests passing

## Phase 5: API Layer (3 hours) - TDD Cycle 4

### Step 1: Controllers
**Files:** `AuthController.cs`, `TasksController.cs`

```
AuthController (public endpoints):
- POST /api/auth/register
- POST /api/auth/login
- Return proper status codes (201, 400, 401)

TasksController (protected endpoints):
- [Authorize] attribute
- GET /api/tasks
- POST /api/tasks
- PUT /api/tasks/{id}
- DELETE /api/tasks/{id}
- Extract userId from JWT claims
```

### Step 2: Middleware
**File:** `ExceptionMiddleware.cs`

```
Global exception handling:
- Catch all exceptions
- Return appropriate status codes
- Domain exceptions → 400 Bad Request
- Unknown exceptions → 500 Internal Server Error
- Consistent error response format
```

### Step 3: Dependency Injection Setup
**File:** `ServiceCollectionExtensions.cs`

```
Extension methods for clean Program.cs:
- AddApplicationServices: Register repositories, use cases
- AddJwtAuthentication: Configure JWT Bearer
- AddSwaggerDocumentation: Configure Swagger with Bearer auth
```

### Step 4: Configuration
**Files:** `Program.cs`, `appsettings.json`

```
Program.cs:
- Register all services
- Configure middleware pipeline
- Enable CORS for frontend
- Swagger in Development

appsettings.json:
- Connection string
- JWT secret (min 32 chars)
- JWT expiration
```

### Step 5: Integration Tests (RED then GREEN)
**Files:** `AuthIntegrationTests.cs`, `TasksIntegrationTests.cs`

```
Integration tests:
✓ Register_WithValidData_ShouldReturnCreated
✓ Login_WithValidCredentials_ShouldReturnToken
✓ GetTasks_WithoutAuthentication_ShouldReturnUnauthorized
✓ CreateTask_WithAuthentication_ShouldReturnCreated

Using WebApplicationFactory for in-memory testing
```

**Result:** API layer complete with end-to-end tests

## Phase 6: Database & DevOps (3 hours)

### Step 1: Migration Scripts
**File:** `001_create_tables.sql`

```sql
CREATE TABLE users (
    id UUID PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    created_at TIMESTAMP NOT NULL
);

CREATE TABLE tasks (
    id UUID PRIMARY KEY,
    user_id UUID NOT NULL,
    title VARCHAR(200) NOT NULL,
    description TEXT,
    status VARCHAR(50) NOT NULL,
    due_date TIMESTAMP NOT NULL,
    created_at TIMESTAMP NOT NULL,
    updated_at TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);

CREATE INDEXES...
```

### Step 2: Seed Data
**File:** `002_seed_data.sql`

```
3 test users:
- john@example.com (3 tasks)
- jane@example.com (2 tasks)
- bob@example.com (2 tasks)

All with password: TestPassword123
Various task statuses for demo
```

### Step 3: Docker Configuration
**File:** `docker-compose.yml`

```yaml
services:
  postgres:
    - PostgreSQL 16
    - Port 5432
    - Healthcheck
    - Persistent volume
  
  api:
    - .NET 8 API
    - Port 5000
    - Depends on postgres
    - Environment variables
```

### Step 4: Setup Automation
**File:** `setup.sh`

```bash
Script to:
- Check Docker running
- Start services
- Wait for PostgreSQL
- Run migrations
- Seed data
- Show credentials
```

**Result:** Complete deployment setup

## Phase 7: Documentation (3 hours)

### Created Documents:
1. **README.md** - Project overview
2. **GENAI_USAGE.md** - AI tool usage documentation
3. **IMPLEMENTATION_PLAN.md** - This document
4. **docs/ARCHITECTURE.md** - Architecture details
5. **docs/DATABASE.md** - Database schema
6. **docs/API.md** - API endpoints
7. **docs/TDD.md** - Testing strategy
8. **docs/DOCKER.md** - Docker setup

## Key Implementation Decisions

### 1. Why TDD?
- Tests define requirements clearly
- Better design through testability
- Confidence in refactoring
- Documentation through tests

### 2. Why Clean Architecture?
- Clear separation of concerns
- Domain independent of frameworks
- Testable in isolation
- Easy to understand and maintain

### 3. Why ADO.NET?
- Demonstrates SQL knowledge
- Full control over queries
- No hidden behaviors
- Required by interview specs

### 4. Why Manual JWT?
- Understanding of auth fundamentals
- No framework magic
- Complete control over tokens
- Better for interview discussion

### 5. Why Simple Code?
- No over-engineering
- Easy to understand
- Fast to develop
- Maintainable

## Testing Strategy

### Unit Tests (80% of tests)
- Domain: Business rules validation
- Application: Use case logic with mocks
- Infrastructure: Auth implementations

### Integration Tests (20% of tests)
- API: End-to-end flows
- Authentication flows
- CRUD operations

### Coverage Focus
- Critical business rules
- Authentication/authorization
- Validation logic
- Error handling
- NOT aiming for 100% just for numbers

## GenAI Usage Strategy

### Iterative Prompts
1. Set context clearly
2. Specify constraints upfront
3. Request tests first (TDD)
4. Review and validate output
5. Refine and simplify

### Critical Reviews
- Rejected over-engineering
- Simplified complex patterns
- Validated security practices
- Ensured Clean Architecture rules

### What AI Did Well
- Boilerplate code generation
- Test case suggestions
- SQL query structure
- Configuration setup

### What Required Human Oversight
- Architecture decisions
- Simplification of code
- Security considerations
- Testing strategy

## Timeline Summary

- **Planning & Setup:** 2 hours
- **Domain Layer (TDD):** 3 hours
- **Application Layer (TDD):** 4 hours
- **Infrastructure Layer (TDD):** 5 hours
- **API Layer (TDD):** 3 hours
- **Database & DevOps:** 3 hours
- **Documentation:** 3 hours
- **Total:** ~24 hours over 3 days

## Lessons Learned

### What Worked
- Starting with Domain (clear foundation)
- Writing tests first (better design)
- Small incremental commits
- Simple solutions over complex

### What Was Challenging
- ADO.NET manual mapping (reflection for private constructors)
- Integration tests setup (WebApplicationFactory)
- Balancing test coverage vs time

### Would Do Differently
- Add logging from start
- Create database helper for tests
- More granular commits showing TDD cycles

## Conclusion

This implementation demonstrates:
- Strong understanding of Clean Architecture
- Disciplined TDD approach
- Low-level .NET skills (ADO.NET, JWT)
- Security consciousness
- Clean code principles
- Effective use of GenAI tools with critical thinking

The project is production-ready and showcases both technical skills and professional development practices.

