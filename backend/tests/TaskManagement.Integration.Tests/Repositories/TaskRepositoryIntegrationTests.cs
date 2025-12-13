using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Repositories;

namespace TaskManagement.Integration.Tests.Repositories;

[Collection("Database")]
public class TaskRepositoryIntegrationTests : IClassFixture<TestDatabaseFixture>
{
    private readonly TaskRepository _repository;
    private readonly UserRepository _userRepository;

    public TaskRepositoryIntegrationTests(TestDatabaseFixture fixture)
    {
        _repository = new TaskRepository(fixture.ConnectionFactory);
        _userRepository = new UserRepository(fixture.ConnectionFactory);
    }

    private async Task<User> CreateTestUserAsync()
    {
        var email = $"test{Guid.NewGuid()}@example.com";
        var user = new User("Test User", email, "hashedPassword123");
        return await _userRepository.CreateAsync(user);
    }

    [Fact]
    public async System.Threading.Tasks.Task CreateAndGetTask_ShouldWork()
    {
        // Arrange
        var user = await CreateTestUserAsync();
        var task = new TaskEntity(user.Id, "Integration Test", "Description", DateTime.UtcNow.AddDays(7));

        // Act
        var created = await _repository.CreateAsync(task);
        var retrieved = await _repository.GetByIdAsync(created.Id);

        // Assert
        Assert.NotNull(retrieved);
        Assert.Equal(task.Title, retrieved.Title);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetByUserIdAsync_ShouldReturnOnlyUserTasks()
    {
        // Arrange
        var user1 = await CreateTestUserAsync();
        var user2 = await CreateTestUserAsync();
        
        var task1 = new TaskEntity(user1.Id, "User1 Task", "Desc", DateTime.UtcNow.AddDays(1));
        var task2 = new TaskEntity(user2.Id, "User2 Task", "Desc", DateTime.UtcNow.AddDays(2));

        await _repository.CreateAsync(task1);
        await _repository.CreateAsync(task2);

        // Act
        var results = await _repository.GetByUserIdAsync(user1.Id);

        // Assert
        var taskList = results.ToList();
        Assert.True(taskList.Count >= 1);
        Assert.Contains(taskList, t => t.Id == task1.Id);
        Assert.DoesNotContain(taskList, t => t.Id == task2.Id);
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateTask_ShouldUpdate()
    {
        // Arrange
        var user = await CreateTestUserAsync();
        var task = new TaskEntity(user.Id, "Original", "Desc", DateTime.UtcNow.AddDays(5));
        await _repository.CreateAsync(task);

        // Act
        task.Update("Updated", "New Desc", DateTime.UtcNow.AddDays(10));
        await _repository.UpdateAsync(task);
        var updated = await _repository.GetByIdAsync(task.Id);

        // Assert
        Assert.NotNull(updated);
        Assert.Equal("Updated", updated.Title);
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteTask_ShouldDelete()
    {
        // Arrange
        var user = await CreateTestUserAsync();
        var task = new TaskEntity(user.Id, "To Delete", "Desc", DateTime.UtcNow.AddDays(5));
        await _repository.CreateAsync(task);

        // Act
        await _repository.DeleteAsync(task.Id);

        // Assert (verify deleted)
        var deleted = await _repository.GetByIdAsync(task.Id);
        Assert.Null(deleted);
    }
}
