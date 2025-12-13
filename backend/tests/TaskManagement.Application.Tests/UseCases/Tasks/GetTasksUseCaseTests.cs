using Moq;
using Xunit;
using TaskManagement.Application.UseCases.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Tests.UseCases.Tasks;

public class GetTasksUseCaseTests
{
    private readonly Mock<ITaskRepository> _mockTaskRepository;
    private readonly GetTasksUseCase _useCase;

    public GetTasksUseCaseTests()
    {
        _mockTaskRepository = new Mock<ITaskRepository>();
        _useCase = new GetTasksUseCase(_mockTaskRepository.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnUserTasks()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var tasks = new List<TaskEntity>
        {
            new TaskEntity(userId, "Task 1", "Description 1", DateTime.UtcNow.AddDays(1)),
            new TaskEntity(userId, "Task 2", "Description 2", DateTime.UtcNow.AddDays(2))
        };

        _mockTaskRepository
            .Setup(r => r.GetByUserIdAsync(userId))
            .ReturnsAsync(tasks);

        // Act
        var result = await _useCase.ExecuteAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockTaskRepository.Verify(r => r.GetByUserIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task Execute_WithNoTasks_ShouldReturnEmptyList()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _mockTaskRepository
            .Setup(r => r.GetByUserIdAsync(userId))
            .ReturnsAsync(new List<TaskEntity>());

        // Act
        var result = await _useCase.ExecuteAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task Execute_ShouldReturnTasksWithCorrectData()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var dueDate = DateTime.UtcNow.AddDays(5);
        var task = new TaskEntity(userId, "Important Task", "Do this now", dueDate);

        _mockTaskRepository
            .Setup(r => r.GetByUserIdAsync(userId))
            .ReturnsAsync(new List<TaskEntity> { task });

        // Act
        var result = await _useCase.ExecuteAsync(userId);

        // Assert
        var taskDto = result.First();
        Assert.Equal("Important Task", taskDto.Title);
        Assert.Equal("Do this now", taskDto.Description);
        Assert.Equal(TaskManagement.Domain.ValueObjects.TaskStatus.Pending, taskDto.Status);
    }
}

