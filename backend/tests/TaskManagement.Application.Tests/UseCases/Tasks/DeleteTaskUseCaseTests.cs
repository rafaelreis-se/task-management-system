using Moq;
using Xunit;
using TaskManagement.Application.UseCases.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Tests.UseCases.Tasks;

public class DeleteTaskUseCaseTests
{
    private readonly Mock<ITaskRepository> _mockTaskRepository;
    private readonly DeleteTaskUseCase _useCase;

    public DeleteTaskUseCaseTests()
    {
        _mockTaskRepository = new Mock<ITaskRepository>();
        _useCase = new DeleteTaskUseCase(_mockTaskRepository.Object);
    }

    [Fact]
    public async Task Execute_WithValidData_ShouldDeleteTask()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        var existingTask = new TaskEntity(userId, "Task to Delete", "Description", DateTime.UtcNow.AddDays(1));

        _mockTaskRepository
            .Setup(r => r.GetByIdAsync(taskId))
            .ReturnsAsync(existingTask);

        _mockTaskRepository
            .Setup(r => r.DeleteAsync(taskId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.ExecuteAsync(taskId, userId);

        // Assert
        Assert.True(result);
        _mockTaskRepository.Verify(r => r.DeleteAsync(taskId), Times.Once);
    }

    [Fact]
    public async Task Execute_WithNonExistentTask_ShouldReturnFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var taskId = Guid.NewGuid();

        _mockTaskRepository
            .Setup(r => r.GetByIdAsync(taskId))
            .ReturnsAsync((TaskEntity?)null);

        // Act
        var result = await _useCase.ExecuteAsync(taskId, userId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Execute_WithDifferentUserId_ShouldReturnFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var differentUserId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        var existingTask = new TaskEntity(differentUserId, "Task", "Description", DateTime.UtcNow.AddDays(1));

        _mockTaskRepository
            .Setup(r => r.GetByIdAsync(taskId))
            .ReturnsAsync(existingTask);

        // Act
        var result = await _useCase.ExecuteAsync(taskId, userId);

        // Assert
        Assert.False(result);
        _mockTaskRepository.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Execute_ShouldVerifyTaskOwnership()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        var existingTask = new TaskEntity(userId, "My Task", "My Description", DateTime.UtcNow.AddDays(1));

        _mockTaskRepository
            .Setup(r => r.GetByIdAsync(taskId))
            .ReturnsAsync(existingTask);

        // Act
        var result = await _useCase.ExecuteAsync(taskId, userId);

        // Assert
        Assert.True(result);
        _mockTaskRepository.Verify(r => r.GetByIdAsync(taskId), Times.Once);
    }
}

