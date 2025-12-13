# Test-Driven Development Approach

This project is built using TDD methodology.

## Red-Green-Refactor Cycle

1. RED: Write a failing test
2. GREEN: Write minimal code to make it pass
3. REFACTOR: Improve code while keeping tests green

## Testing Strategy

### Unit Tests
Test individual components in isolation using mocks.

- Domain entities business logic
- Application use cases
- Repository implementations
- Service classes

### Integration Tests
Test how components work together.

- API endpoints with test database
- Repository operations with real database
- Authentication flow

## Test Organization

Tests are organized by layer:
- TaskManagement.Domain.Tests
- TaskManagement.Application.Tests
- TaskManagement.Infrastructure.Tests
- TaskManagement.API.Tests

## Example Test Structure

```csharp
public class CreateTaskUseCaseTests
{
    [Fact]
    public async Task CreateTask_WithValidData_ShouldSucceed()
    {
        // Arrange
        var mockRepository = new Mock<ITaskRepository>();
        var useCase = new CreateTaskUseCase(mockRepository.Object);
        var request = new CreateTaskRequest { /* valid data */ };
        
        // Act
        var result = await useCase.ExecuteAsync(request);
        
        // Assert
        Assert.NotNull(result);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<Task>()), Times.Once);
    }
    
    [Fact]
    public async Task CreateTask_WithEmptyTitle_ShouldFail()
    {
        // Test validation logic
    }
}
```

## Mocking Strategy

Use Moq library for creating mock objects.
Mock repository interfaces in Application layer tests.
Mock service interfaces when needed.

## Test Coverage Goal

Aim for at least 80% code coverage across all layers.
Focus on testing business logic and critical paths.

