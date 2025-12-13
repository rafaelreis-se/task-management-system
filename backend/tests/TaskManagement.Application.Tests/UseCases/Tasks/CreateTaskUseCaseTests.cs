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
        _mockTaskRepository.Verify(r => r.CreateAsync(It.IsAny<TaskEntity>()), Times.Once);
    }
}

