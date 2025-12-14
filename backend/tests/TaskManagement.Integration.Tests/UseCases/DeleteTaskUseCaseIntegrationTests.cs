using TaskManagement.Application.UseCases.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Repositories;

namespace TaskManagement.Integration.Tests.UseCases;

[Collection("Database")]
public class DeleteTaskUseCaseIntegrationTests : IClassFixture<TestDatabaseFixture>
{
    private readonly TaskRepository _taskRepository;
    private readonly UserRepository _userRepository;
    private readonly DeleteTaskUseCase _useCase;

    public DeleteTaskUseCaseIntegrationTests(TestDatabaseFixture fixture)
    {
        _taskRepository = new TaskRepository(fixture.ConnectionFactory);
        _userRepository = new UserRepository(fixture.ConnectionFactory);
        _useCase = new DeleteTaskUseCase(_taskRepository);
    }

    private async Task<User> CreateTestUserAsync()
    {
        var email = $"test{Guid.NewGuid()}@example.com";
        var user = new User("Test User", email, "hashedPassword123");
        return await _userRepository.CreateAsync(user);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldDeleteTaskFromDatabase()
    {
        // Arrange
        var user = await CreateTestUserAsync();
        var task = new TaskEntity(user.Id, "Task to Delete", "Description", DateTime.UtcNow.AddDays(5));
        await _taskRepository.CreateAsync(task);

        // Act
        var result = await _useCase.ExecuteAsync(task.Id, user.Id);

        // Assert
        Assert.True(result);
        
        // Verify it's deleted from database
        var taskInDb = await _taskRepository.GetByIdAsync(task.Id);
        Assert.Null(taskInDb);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnFalse_WhenUserDoesNotOwnTask()
    {
        // Arrange
        var owner = await CreateTestUserAsync();
        var otherUser = await CreateTestUserAsync();
        var task = new TaskEntity(owner.Id, "Owner Task", "Desc", DateTime.UtcNow.AddDays(5));
        await _taskRepository.CreateAsync(task);

        // Act
        var result = await _useCase.ExecuteAsync(task.Id, otherUser.Id);

        // Assert
        Assert.False(result);
        
        // Verify task still exists
        var taskInDb = await _taskRepository.GetByIdAsync(task.Id);
        Assert.NotNull(taskInDb);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnFalse_WhenTaskNotFound()
    {
        // Arrange
        var user = await CreateTestUserAsync();
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _useCase.ExecuteAsync(nonExistentId, user.Id);

        // Assert
        Assert.False(result);
    }
}



