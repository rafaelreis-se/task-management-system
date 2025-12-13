using TaskManagement.Application.DTOs;
using TaskManagement.Application.UseCases.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Repositories;

namespace TaskManagement.Integration.Tests.UseCases;

[Collection("Database")]
public class CreateTaskUseCaseIntegrationTests : IClassFixture<TestDatabaseFixture>
{
    private readonly TaskRepository _taskRepository;
    private readonly UserRepository _userRepository;
    private readonly CreateTaskUseCase _useCase;

    public CreateTaskUseCaseIntegrationTests(TestDatabaseFixture fixture)
    {
        _taskRepository = new TaskRepository(fixture.ConnectionFactory);
        _userRepository = new UserRepository(fixture.ConnectionFactory);
        _useCase = new CreateTaskUseCase(_taskRepository);
    }

    private async Task<User> CreateTestUserAsync()
    {
        var email = $"test{Guid.NewGuid()}@example.com";
        var user = new User("Test User", email, "hashedPassword123");
        return await _userRepository.CreateAsync(user);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCreateTaskInDatabase()
    {
        // Arrange
        var user = await CreateTestUserAsync();
        var request = new CreateTaskRequest(
            "Integration Test Task",
            "This task should be created in real database",
            DateTime.UtcNow.AddDays(7)
        );

        // Act
        var result = await _useCase.ExecuteAsync(user.Id, request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Title, result.Title);
        Assert.Equal(request.Description, result.Description);
        
        // Verify it's actually in the database
        var taskInDb = await _taskRepository.GetByIdAsync(result.Id);
        Assert.NotNull(taskInDb);
        Assert.Equal(request.Title, taskInDb.Title);
    }

    [Fact]
    public async Task ExecuteAsync_WithNullDescription_ShouldCreateTask()
    {
        // Arrange
        var user = await CreateTestUserAsync();
        var request = new CreateTaskRequest(
            "Task without description",
            null,
            DateTime.UtcNow.AddDays(5)
        );

        // Act
        var result = await _useCase.ExecuteAsync(user.Id, request);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Description);
        
        var taskInDb = await _taskRepository.GetByIdAsync(result.Id);
        Assert.NotNull(taskInDb);
        Assert.Null(taskInDb!.Description);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldSetPendingStatus()
    {
        // Arrange
        var user = await CreateTestUserAsync();
        var request = new CreateTaskRequest(
            "New Task",
            "Description",
            DateTime.UtcNow.AddDays(3)
        );

        // Act
        var result = await _useCase.ExecuteAsync(user.Id, request);

        // Assert
        Assert.Equal(Domain.ValueObjects.TaskStatus.Pending, result!.Status);
    }
}

