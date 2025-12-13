using Moq;
using Xunit;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.UseCases.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Interfaces;
using TaskStatus = TaskManagement.Domain.ValueObjects.TaskStatus;

namespace TaskManagement.Application.Tests.UseCases.Tasks;

public class UpdateTaskUseCaseTests
{
    private readonly Mock<ITaskRepository> _mockTaskRepository;
    private readonly UpdateTaskUseCase _useCase;

    public UpdateTaskUseCaseTests()
    {
        _mockTaskRepository = new Mock<ITaskRepository>();
        _useCase = new UpdateTaskUseCase(_mockTaskRepository.Object);
    }

    [Fact]
    public async Task Execute_WithValidData_ShouldUpdateTask()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        var existingTask = new TaskEntity(userId, "Old Title", "Old Description", DateTime.UtcNow.AddDays(1));

        var request = new UpdateTaskRequest(
            "New Title",
            "New Description",
            DateTime.UtcNow.AddDays(7),
            TaskStatus.InProgress
        );

        _mockTaskRepository
            .Setup(r => r.GetByIdAsync(taskId))
            .ReturnsAsync(existingTask);

        _mockTaskRepository
            .Setup(r => r.UpdateAsync(It.IsAny<TaskEntity>()))
            .Returns((TaskEntity t) => Task.FromResult(t));

        // Act
        var result = await _useCase.ExecuteAsync(taskId, userId, request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Title, result.Title);
        Assert.Equal(request.Description, result.Description);
        Assert.Equal(request.Status, result.Status);
        _mockTaskRepository.Verify(r => r.UpdateAsync(It.IsAny<TaskEntity>()), Times.Once);
    }

    [Fact]
    public async Task Execute_WithNonExistentTask_ShouldReturnNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        var request = new UpdateTaskRequest("Title", "Description", DateTime.UtcNow.AddDays(1), TaskStatus.Pending);

        _mockTaskRepository
            .Setup(r => r.GetByIdAsync(taskId))
            .ReturnsAsync((TaskEntity?)null);

        // Act
        var result = await _useCase.ExecuteAsync(taskId, userId, request);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Execute_WithDifferentUserId_ShouldReturnNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var differentUserId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        var existingTask = new TaskEntity(differentUserId, "Title", "Description", DateTime.UtcNow.AddDays(1));

        var request = new UpdateTaskRequest("New Title", "New Description", DateTime.UtcNow.AddDays(1), TaskStatus.Pending);

        _mockTaskRepository
            .Setup(r => r.GetByIdAsync(taskId))
            .ReturnsAsync(existingTask);

        // Act
        var result = await _useCase.ExecuteAsync(taskId, userId, request);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Execute_StatusChange_ShouldUpdateCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        var existingTask = new TaskEntity(userId, "Task", "Description", DateTime.UtcNow.AddDays(1));

        var request = new UpdateTaskRequest(
            "Task",
            "Description",
            DateTime.UtcNow.AddDays(1),
            TaskStatus.Completed
        );

        _mockTaskRepository
            .Setup(r => r.GetByIdAsync(taskId))
            .ReturnsAsync(existingTask);

        _mockTaskRepository
            .Setup(r => r.UpdateAsync(It.IsAny<TaskEntity>()))
            .Returns((TaskEntity t) => Task.FromResult(t));

        // Act
        var result = await _useCase.ExecuteAsync(taskId, userId, request);

        // Assert
        Assert.Equal(TaskStatus.Completed, result!.Status);
    }
}

