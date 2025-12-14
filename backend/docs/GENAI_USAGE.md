# GenAI Tool Usage Documentation

## Prompt Engineering Approach

This project was developed using GenAI tools (Cursor AI) with structured prompts following best practices.

## Initial Prompt Strategy

### Context Setting
The initial prompt provided clear context about the project requirements:

```
Develop a Task Management System API using .NET 10, Clean Architecture, and TDD.
Key constraints:
- No Entity Framework, Dapper, or MediatR
- Manual JWT authentication implementation
- ADO.NET for data access
- PostgreSQL database
```

### Incremental Development Prompts

#### 1. Domain Layer
```
Create Domain entities (Task, User) following TDD.
Write tests first that validate:
- Business rules (title length, due date validation)
- Entity creation with valid/invalid data
Then implement entities to pass tests.
```

**AI Output Validation:**
- Reviewed generated test cases for completeness
- Ensured domain exceptions were properly typed
- Verified no framework dependencies in Domain layer

#### 2. Repository Interfaces
```
Define repository interfaces in Domain layer:
- ITaskRepository with CRUD operations
- IUserRepository with email lookup
Keep interfaces simple, return Task<T> for async operations.
```

**Critical Review:**
- Simplified overly complex suggested interfaces
- Ensured async/await patterns were correctly applied
- Removed unnecessary abstraction layers

#### 3. Application Layer Use Cases
```
Create use cases for:
- CreateTask, UpdateTask, DeleteTask, GetTasks
- RegisterUser, LoginUser
Follow TDD: write tests using Moq for repository mocks first.
Use DTOs, never expose domain entities.
```

**Edge Cases Handled:**
- Empty title validation
- Password minimum length (8 chars)
- Email uniqueness check
- Past due date prevention
- User authorization (can only access own tasks)

**AI Suggestions Improved:**
- Added proper error handling with domain exceptions
- Implemented null checks for not found scenarios
- Enhanced validation logic

#### 4. Infrastructure Layer
```
Implement repositories using ADO.NET with raw SQL.
No ORMs. Use Npgsql for PostgreSQL.
Implement PasswordHasher with BCrypt.
Implement JwtTokenGenerator manually without ASP.NET Identity.
```

**Validation Process:**
- Tested SQL injection prevention with parameterized queries
- Verified BCrypt salt generation
- Checked JWT token structure and claims
- Ensured proper connection disposal with using statements

**Corrections Made:**
- Fixed reader mapping to handle null values correctly
- Added reflection for private constructors (needed for entity hydration)
- Improved error handling in database operations

#### 5. API Layer
```
Create controllers with proper HTTP verbs and status codes.
Implement JWT authentication middleware.
Add global exception handling middleware.
Configure Swagger with Bearer authentication.
```

**Authentication Implementation:**
- Manual JWT configuration without Identity framework
- Custom claims extraction (NameIdentifier, Email)
- Proper authorization attributes on protected endpoints

**Improvements:**
- Added CORS policy for frontend integration
- Enhanced error responses with consistent format
- Implemented proper async/await in controllers

## Code Quality Validation

### Tests Review
- **Domain Tests**: Validated all business rules with edge cases
- **Application Tests**: Mocked dependencies correctly, tested failure paths
- **Infrastructure Tests**: Verified password hashing and JWT generation
- **Integration Tests**: End-to-end API testing with authentication

### Clean Architecture Validation
- Domain has zero external dependencies
- Application depends only on Domain
- Infrastructure implements interfaces from inner layers
- API depends on all layers but follows dependency inversion

### Clean Code Principles Applied
- Small, focused functions (Single Responsibility)
- Meaningful names (CreateTaskUseCase, not TaskService)
- No code duplication
- Self-documenting code with minimal comments
- Avoided premature optimization

## Edge Cases and Validations

### Authentication
- Password must be at least 8 characters
- Email uniqueness enforced at database and application layer
- Invalid credentials return 401 Unauthorized
- Token expiration properly configured

### Task Management
- Users can only access their own tasks (checked via UserId)
- Title cannot exceed 200 characters
- Due date cannot be in the past
- Status transitions validated (Pending → InProgress → Completed)

### Database Operations
- Parameterized queries prevent SQL injection
- Foreign key constraints ensure referential integrity
- Proper null handling for optional fields (Description, UpdatedAt)
- Connection disposal handled with using statements

## AI-Generated Code That Was Rejected

1. **Over-engineered Result<T> pattern**: AI suggested complex Result monad pattern. Rejected for simpler exception-based error handling given project scope.

2. **Repository per Entity per Operation**: AI suggested ICreateTaskRepository, IUpdateTaskRepository. Rejected for single ITaskRepository with all CRUD operations (simpler, more maintainable).

3. **Complex validation framework**: AI suggested FluentValidation library. Rejected for simple inline validations (meets requirements, fewer dependencies).

## GenAI Best Practices Demonstrated

1. **Iterative prompts**: Built layer by layer, not all at once
2. **Test-first prompts**: Always asked for tests before implementation
3. **Constraint specification**: Clearly stated "no EF, no Dapper"
4. **Critical review**: Never accepted AI output without validation
5. **Edge case explicit**: Asked specifically about validations and error handling
6. **Simplicity preference**: Rejected over-engineered suggestions

## Lessons Learned

### What Worked Well
- Breaking down into small, focused prompts
- TDD approach kept code quality high
- Explicitly stating constraints prevented wrong libraries
- Iterative refinement improved initial suggestions

### What Required Human Oversight
- Architecture decisions (when to add abstraction)
- Security considerations (SQL injection, password storage)
- Testing strategy (integration vs unit test balance)
- Code simplicity (avoiding over-engineering)

## Conclusion

GenAI tools are powerful when used with:
- Clear, specific prompts
- Critical evaluation of suggestions
- Understanding of underlying principles
- Willingness to reject and refine output

The key is treating AI as a pair programmer, not an autonomous developer.

