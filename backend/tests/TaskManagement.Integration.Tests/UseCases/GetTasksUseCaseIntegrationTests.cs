using TaskManagement.Application.UseCases.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Repositories;

namespace TaskManagement.Integration.Tests.UseCases;

[Collection("Database")]
public class GetTasksUseCaseIntegrationTests : IClassFixture<TestDatabaseFixture>
{
    private readonly TaskRepository _taskRepository;
    private readonly UserRepository _userRepository;
    private readonly GetTasksUseCase _useCase;

    public GetTasksUseCaseIntegrationTests(TestDatabaseFixture fixture)
    {
        _taskRepository = new TaskRepository(fixture.ConnectionFactory);
        _userRepository = new UserRepository(fixture.ConnectionFactory);
        _useCase = new GetTasksUseCase(_taskRepository);
    }

    private async Task<User> CreateTestUserAsync()
    {
        var email = $"test{Guid.NewGuid()}@example.com";
        var user = new User("Test User", email, "hashedPassword123");
        return await _userRepository.CreateAsync(user);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnUserTasksFromDatabase()
    {
        // Arrange
        var user = await CreateTestUserAsync();
        var task1 = new TaskEntity(user.Id, "Task 1", "Description 1", DateTime.UtcNow.AddDays(1));
        var task2 = new TaskEntity(user.Id, "Task 2", "Description 2", DateTime.UtcNow.AddDays(2));
        
        await _taskRepository.CreateAsync(task1);
        await _taskRepository.CreateAsync(task2);

        // Act
        var result = await _useCase.ExecuteAsync(user.Id);

        // Assert
        var tasks = result.ToList();
        Assert.True(tasks.Count >= 2);
        Assert.Contains(tasks, t => t.Title == "Task 1");
        Assert.Contains(tasks, t => t.Title == "Task 2");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnEmptyForUserWithNoTasks()
    {
        // Arrange
        var user = await CreateTestUserAsync();

        // Act
        var result = await _useCase.ExecuteAsync(user.Id);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldNotReturnOtherUsersTasks()
    {
        // Arrange
        var user1 = await CreateTestUserAsync();
        var user2 = await CreateTestUserAsync();
        
        var task1 = new TaskEntity(user1.Id, "User1 Task", "Desc", DateTime.UtcNow.AddDays(1));
        var task2 = new TaskEntity(user2.Id, "User2 Task", "Desc", DateTime.UtcNow.AddDays(2));
        
        await _taskRepository.CreateAsync(task1);
        await _taskRepository.CreateAsync(task2);

        // Act
        var result = await _useCase.ExecuteAsync(user1.Id);

        // Assert
        var tasks = result.ToList();
        Assert.DoesNotContain(tasks, t => t.Title == "User2 Task");
        Assert.Contains(tasks, t => t.Title == "User1 Task");
    }
}


