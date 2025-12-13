# Architecture Overview

This project follows Clean Architecture principles with clear separation of concerns.

## Layers

### Domain Layer
The core of the application. Contains business entities and rules.

- Entities: Task, User
- Interfaces: ITaskRepository, IUserRepository
- Value Objects: TaskStatus
- No dependencies on other layers

### Application Layer
Contains the business logic and use cases.

- Use Cases: CreateTask, UpdateTask, DeleteTask, GetTasks, RegisterUser, LoginUser
- DTOs for data transfer
- Validation logic
- Service interfaces
- Depends only on Domain

### Infrastructure Layer
Implements interfaces defined in Domain and Application.

- Repository implementations using ADO.NET
- Database connection management
- JWT token generation
- Password hashing
- Depends on Domain and Application

### API Layer
The entry point for HTTP requests.

- Controllers
- Request/Response DTOs
- Middleware (error handling, authentication)
- Swagger configuration
- Depends on Application and Infrastructure

## Data Flow

Request → Controller → Use Case → Repository → Database
Response ← Controller ← Use Case ← Repository ← Database

## Key Principles

- Dependency Inversion: Inner layers define interfaces, outer layers implement them
- Single Responsibility: Each class has one reason to change
- Separation of Concerns: Each layer has distinct responsibilities
- Testability: Dependencies are injected, making unit testing straightforward

## Database Access

All database operations use ADO.NET with raw SQL queries.
No ORM is used in this project as per technical requirements.

Connection management is centralized in the Infrastructure layer.

