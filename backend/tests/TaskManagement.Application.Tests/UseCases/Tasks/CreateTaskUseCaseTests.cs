using Moq;
using Xunit;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.UseCases.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Tests.UseCases.Tasks;

public class CreateTaskUseCaseTests
{
    private readonly Mock<ITaskRepository> _mockTaskRepository;
    private readonly CreateTaskUseCase _useCase;

    public CreateTaskUseCaseTests()
    {
        _mockTaskRepository = new Mock<ITaskRepository>();
        _useCase = new CreateTaskUseCase(_mockTaskRepository.Object);
    }

    [Fact]
    public async Task Execute_WithValidData_ShouldCreateTask()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new CreateTaskRequest(
            "Complete project",
            "Finish all tasks",
            DateTime.UtcNow.AddDays(7)
        );

        _mockTaskRepository
            .Setup(r => r.CreateAsync(It.IsAny<TaskEntity>()))
            .ReturnsAsync((TaskEntity t) => t);

        // Act
        var result = await _useCase.ExecuteAsync(userId, request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Title, result.Title);
        Assert.Equal(request.Description, result.Description);
        Assert.Equal(request.DueDate, result.DueDate);
        _mockTaskRepository.Verify(r => r.CreateAsync(It.IsAny<TaskEntity>()), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldCreateTaskWithPendingStatus()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new CreateTaskRequest("New Task", "Description", DateTime.UtcNow.AddDays(3));

        _mockTaskRepository
            .Setup(r => r.CreateAsync(It.IsAny<TaskEntity>()))
            .ReturnsAsync((TaskEntity t) => t);

        // Act
        var result = await _useCase.ExecuteAsync(userId, request);

        // Assert
        Assert.Equal(TaskManagement.Domain.ValueObjects.TaskStatus.Pending, result.Status);
    }

    [Fact]
    public async Task Execute_WithNullDescription_ShouldCreateTask()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new CreateTaskRequest("Task without description", null, DateTime.UtcNow.AddDays(1));

        _mockTaskRepository
            .Setup(r => r.CreateAsync(It.IsAny<TaskEntity>()))
            .ReturnsAsync((TaskEntity t) => t);

        // Act
        var result = await _useCase.ExecuteAsync(userId, request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Task without description", result.Title);
        Assert.Null(result.Description);
    }
}

