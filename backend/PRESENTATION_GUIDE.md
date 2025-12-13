# Interview Presentation Guide

## Before the Interview

### Install Just (if not installed)
```bash
brew install just
```

### Test Everything Works
```bash
just test  # Should show 72/72 passing
```

## Opening Statement (1 minute)

"Hello! Today I'll present a Task Management System API I built following Clean Architecture and TDD principles. I used .NET 10, PostgreSQL, and manual implementations without Entity Framework or other ORMs as required. The project includes a justfile for easy command running - you can see all commands with `just --list`."

## Demo Flow (20 minutes)

### 1. Project Overview (2 min)

Show `README.md` and explain:
- User story
- Tech stack
- Architecture approach

**Key Command:**
```bash
just info
```

### 2. Architecture Walkthrough (5 min)

Open in IDE and show structure:

**Domain Layer** (`src/TaskManagement.Domain/`)
- Show `Entities/TaskEntity.cs` - Business rules
- Show `Entities/User.cs` - Email validation
- Highlight: ZERO external dependencies

**Application Layer** (`src/TaskManagement.Application/`)
- Show `UseCases/Tasks/CreateTaskUseCase.cs`
- Show DTOs
- Explain: Depends only on Domain

**Infrastructure Layer** (`src/TaskManagement.Infrastructure/`)
- Show `Repositories/TaskRepository.cs` - ADO.NET with raw SQL
- Show parameterized queries (SQL injection prevention)
- Show `Auth/PasswordHasher.cs` - BCrypt
- Show `Auth/JwtTokenGenerator.cs` - Manual JWT

**API Layer** (`src/TaskManagement.API/`)
- Show `Controllers/TasksController.cs` - [Authorize] attribute
- Show `Middleware/ExceptionMiddleware.cs`
- Show `Program.cs` - Clean setup with extension methods

### 3. TDD Demonstration (5 min)

Show test-first approach:

**Open:** `tests/TaskManagement.Domain.Tests/Entities/TaskEntityTests.cs`

Explain:
- "I wrote these tests FIRST, before implementation"
- Show test: `CreateTask_WithValidData_ShouldSucceed`
- Show test: `CreateTask_WithEmptyTitle_ShouldThrowException`

**Then show:** `src/TaskManagement.Domain/Entities/TaskEntity.cs`

Explain:
- "Then implemented minimum code to pass tests"
- Point to validation methods

**Run tests live:**
```bash
just test  # Watch 72 tests pass with isolated test database
```

### 4. Manual Implementations (4 min)

**ADO.NET Repository:**
Open `src/TaskManagement.Infrastructure/Repositories/TaskRepository.cs`

Point out:
- Raw SQL queries
- NpgsqlCommand usage
- Parameterized queries
- No Entity Framework magic

**JWT Authentication:**
Open `src/TaskManagement.Infrastructure/Auth/JwtTokenGenerator.cs`

Point out:
- Manual token generation
- Claims configuration
- Symmetric key signing
- No ASP.NET Identity

**Password Hashing:**
Open `src/TaskManagement.Infrastructure/Auth/PasswordHasher.cs`

Point out:
- BCrypt with salt
- Secure hashing

### 5. Live API Demo (4 min)

```bash
# Start database
just db-up

# Setup database
just db-setup

# Start API
just run
```

Open browser: http://localhost:5000/swagger

**Live demonstration:**
1. Use `/api/auth/login` with test credentials
   - Email: `john@example.com`
   - Password: `TestPassword123`
2. Copy the token from response
3. Click "Authorize" button, enter token
4. Test `/api/tasks` GET endpoint
5. Test `/api/tasks` POST endpoint to create a task
6. Show that without token, it returns 401

## Q&A Preparation

### Expected Questions & Answers

**Q: Why didn't you use Entity Framework?**
A: "The requirements explicitly prohibited it, and it gave me an opportunity to demonstrate raw SQL skills and understanding of ADO.NET. I can show you the parameterized queries that prevent SQL injection."

**Q: How did you approach TDD?**
A: "I followed Red-Green-Refactor strictly. I can show you test files where tests were written before implementation. For example, [open test file], these tests defined the requirements, then I implemented the minimum code to pass them."

**Q: What would you change for production?**
A: "I'd add structured logging with Serilog, implement health checks, add request/response logging middleware, implement refresh tokens for JWT, add rate limiting, and use a proper secrets manager instead of appsettings."

**Q: How do you ensure security?**
A: "Multiple layers: parameterized queries prevent SQL injection, BCrypt with salt for passwords, JWT tokens with expiration, HTTPS in production, and input validation at both domain and API layers."

**Q: What is your testing strategy?**
A: "I wrote 72 comprehensive tests covering all layers - unit tests for business logic and validation, integration tests with real PostgreSQL database. I focused on quality over quantity, testing critical paths, edge cases, and error scenarios rather than chasing arbitrary coverage metrics."

**Q: How would you scale this?**
A: "Add caching for read-heavy operations, implement read replicas, add message queues for async operations, implement CQRS if needed, add API gateway for microservices, and containerize with Kubernetes."

**Q: Why use Just instead of Make?**
A: "Just is more modern, cross-platform, has simpler syntax without Make's tab issues, and is well-suited for .NET projects. It also makes it very easy for anyone to run the project - `just demo` and you're done."

## Files to Highlight

**Must Show:**
1. `justfile` - Professional automation
2. `IMPLEMENTATION_PLAN.md` - Strategic approach
3. `docs/GENAI_USAGE.md` - AI tool usage with critical thinking
4. `src/TaskManagement.Domain/Entities/TaskEntity.cs` - Business logic
5. `src/TaskManagement.Infrastructure/Repositories/TaskRepository.cs` - ADO.NET
6. `tests/TaskManagement.Domain.Tests/` - TDD examples

**Quick References:**
- `PROJECT_SUMMARY.md` - Project overview
- `TEST_RESULTS.md` - Test suite details
- `COMMANDS.md` - All useful commands

## Demo Script

### Option 1: Live Coding Demo
Show git history with TDD commits (if you committed incrementally)

### Option 2: Code Walkthrough
Walk through each layer showing separation of concerns

### Option 3: Live API Test
Start API and test all endpoints live with Swagger

## Time Management

- Overview: 2 min
- Architecture: 5 min
- TDD Demo: 5 min
- Manual Implementations: 4 min
- Live Demo: 4 min
- **Total: 20 min**
- Q&A: 10+ min

## Tips

1. **Be confident** - You built this with best practices
2. **Show, don't just tell** - Run commands, show code
3. **Explain trade-offs** - Why you chose X over Y
4. **Be honest** - If asked about improvements, suggest them
5. **Use just commands** - Shows professionalism

## Red Flags to Avoid

❌ Don't say "I don't know" - say "Let me show you in the code"
❌ Don't apologize for simplicity - it's a strength
❌ Don't criticize your own code - explain decisions instead
❌ Don't claim it's production-ready if it's not - be honest about scope

## Green Flags to Demonstrate

✅ Understanding of Clean Architecture principles
✅ TDD discipline (test-first approach)
✅ Security consciousness (parameterized queries, BCrypt, JWT)
✅ Code quality (SOLID principles, clean code)
✅ Documentation (comprehensive docs)
✅ Developer experience (justfile, automated test database, setup scripts)
✅ Critical thinking with GenAI (show what you rejected)

## Closing Statement

"Thank you for your time. The project demonstrates Clean Architecture, TDD methodology, and manual low-level implementations as required. All code is tested, documented, and ready for review. I'm happy to answer any questions or dive deeper into any part of the implementation."

## Post-Presentation

**If they ask to see specific code:**
- Use `just` commands to quickly demonstrate
- Open files confidently
- Explain your reasoning

**If they ask about GenAI usage:**
- Show `docs/GENAI_USAGE.md`
- Explain your prompting strategy
- Discuss what you accepted vs rejected
- Demonstrate critical thinking

**If they want to run it themselves:**
```bash
just demo
just run
# Open http://localhost:5000/swagger
```

## Backup Plan

If something goes wrong during demo:
1. Stay calm
2. Show the tests passing: `just test`
3. Walk through code instead of live demo
4. Explain what should happen

Remember: The code quality and architecture matter more than a perfect live demo.

## Final Checklist

Before presenting:
- [ ] `just test` - All tests passing
- [ ] `just test` - All 72 tests passing
- [ ] `just info` - Shows project info correctly
- [ ] Review IMPLEMENTATION_PLAN.md
- [ ] Review PROJECT_SUMMARY.md
- [ ] Review GENAI_USAGE.md
- [ ] Test credentials documented
- [ ] Swagger UI loads correctly
- [ ] Docker commands work

## Good Luck!

You've built a solid project with:
- Clean Architecture ✅
- TDD ✅
- Manual implementations ✅
- Professional tooling ✅
- Complete documentation ✅

You're ready! 🚀

