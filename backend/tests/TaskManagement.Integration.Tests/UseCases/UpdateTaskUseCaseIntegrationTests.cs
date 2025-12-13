using TaskManagement.Application.DTOs;
using TaskManagement.Application.UseCases.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Repositories;

namespace TaskManagement.Integration.Tests.UseCases;

[Collection("Database")]
public class UpdateTaskUseCaseIntegrationTests : IClassFixture<TestDatabaseFixture>
{
    private readonly TaskRepository _taskRepository;
    private readonly UserRepository _userRepository;
    private readonly UpdateTaskUseCase _useCase;

    public UpdateTaskUseCaseIntegrationTests(TestDatabaseFixture fixture)
    {
        _taskRepository = new TaskRepository(fixture.ConnectionFactory);
        _userRepository = new UserRepository(fixture.ConnectionFactory);
        _useCase = new UpdateTaskUseCase(_taskRepository);
    }

    private async Task<User> CreateTestUserAsync()
    {
        var email = $"test{Guid.NewGuid()}@example.com";
        var user = new User("Test User", email, "hashedPassword123");
        return await _userRepository.CreateAsync(user);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldUpdateTaskInDatabase()
    {
        // Arrange
        var user = await CreateTestUserAsync();
        var task = new TaskEntity(user.Id, "Original Title", "Original Desc", DateTime.UtcNow.AddDays(5));
        await _taskRepository.CreateAsync(task);

        var request = new UpdateTaskRequest(
            "Updated Title",
            "Updated Description",
            DateTime.UtcNow.AddDays(10),
            TaskStatus.InProgress
        );

        // Act
        var result = await _useCase.ExecuteAsync(task.Id, user.Id, request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Title", result!.Title);
        Assert.Equal("Updated Description", result!.Description);
        Assert.Equal(TaskStatus.InProgress, result!.Status);
        
        // Verify in database
        var taskInDb = await _taskRepository.GetByIdAsync(task.Id);
        Assert.Equal("Updated Title", taskInDb.Title);
        Assert.Equal(TaskStatus.InProgress, taskInDb.Status);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNull_WhenUserDoesNotOwnTask()
    {
        // Arrange
        var owner = await CreateTestUserAsync();
        var otherUser = await CreateTestUserAsync();
        var task = new TaskEntity(owner.Id, "Owner Task", "Desc", DateTime.UtcNow.AddDays(5));
        await _taskRepository.CreateAsync(task);

        var request = new UpdateTaskRequest(
            "Hacked Title",
            "Hacked Desc",
            DateTime.UtcNow.AddDays(10),
            TaskStatus.Completed
        );

        // Act
        var result = await _useCase.ExecuteAsync(task.Id, otherUser.Id, request);

        // Assert
        Assert.Null(result);
        
        // Verify task was NOT updated
        var taskInDb = await _taskRepository.GetByIdAsync(task.Id);
        Assert.NotNull(taskInDb);
        Assert.Equal("Owner Task", taskInDb!.Title);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNull_WhenTaskNotFound()
    {
        // Arrange
        var user = await CreateTestUserAsync();
        var nonExistentId = Guid.NewGuid();
        
        var request = new UpdateTaskRequest(
            "Title",
            "Desc",
            DateTime.UtcNow.AddDays(5),
            TaskStatus.Pending
        );

        // Act
        var result = await _useCase.ExecuteAsync(nonExistentId, user.Id, request);

        // Assert
        Assert.Null(result);
    }
}

