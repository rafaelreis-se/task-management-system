using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.ValueObjects;
using Xunit;
using TaskStatus = TaskManagement.Domain.ValueObjects.TaskStatus;

namespace TaskManagement.Domain.Tests.Entities;

public class TaskEntityTests
{
    [Fact]
    public void CreateTask_WithValidData_ShouldSucceed()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var title = "Complete project documentation";
        var description = "Write comprehensive documentation";
        var dueDate = DateTime.UtcNow.AddDays(7);

        // Act
        var task = new TaskEntity(userId, title, description, dueDate);

        // Assert
        Assert.NotEqual(Guid.Empty, task.Id);
        Assert.Equal(userId, task.UserId);
        Assert.Equal(title, task.Title);
        Assert.Equal(description, task.Description);
        Assert.Equal(TaskStatus.Pending, task.Status);
        Assert.Equal(dueDate, task.DueDate);
        Assert.True(task.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void CreateTask_WithEmptyTitle_ShouldThrowException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var dueDate = DateTime.UtcNow.AddDays(7);

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            new TaskEntity(userId, "", "Description", dueDate));
    }

    [Fact]
    public void CreateTask_WithTitleTooLong_ShouldThrowException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var longTitle = new string('a', 201);
        var dueDate = DateTime.UtcNow.AddDays(7);

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            new TaskEntity(userId, longTitle, "Description", dueDate));
    }

    [Fact]
    public void CreateTask_WithPastDueDate_ShouldThrowException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var pastDate = DateTime.UtcNow.AddDays(-1);

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            new TaskEntity(userId, "Title", "Description", pastDate));
    }

    [Fact]
    public void UpdateTask_WithValidData_ShouldSucceed()
    {
        // Arrange
        var task = new TaskEntity(Guid.NewGuid(), "Original", "Desc", DateTime.UtcNow.AddDays(7));
        var newTitle = "Updated Title";
        var newDescription = "Updated Description";
        var newDueDate = DateTime.UtcNow.AddDays(10);

        // Act
        task.Update(newTitle, newDescription, newDueDate);

        // Assert
        Assert.Equal(newTitle, task.Title);
        Assert.Equal(newDescription, task.Description);
        Assert.Equal(newDueDate, task.DueDate);
        Assert.NotNull(task.UpdatedAt);
    }

    [Fact]
    public void ChangeStatus_ToInProgress_ShouldSucceed()
    {
        // Arrange
        var task = new TaskEntity(Guid.NewGuid(), "Title", "Desc", DateTime.UtcNow.AddDays(7));

        // Act
        task.ChangeStatus(TaskStatus.InProgress);

        // Assert
        Assert.Equal(TaskStatus.InProgress, task.Status);
    }

    [Fact]
    public void ChangeStatus_ToCompleted_ShouldSucceed()
    {
        // Arrange
        var task = new TaskEntity(Guid.NewGuid(), "Title", "Desc", DateTime.UtcNow.AddDays(7));

        // Act
        task.ChangeStatus(TaskStatus.Completed);

        // Assert
        Assert.Equal(TaskStatus.Completed, task.Status);
    }

    [Fact]
    public void CreateTask_WithNullDescription_ShouldSucceed()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var dueDate = DateTime.UtcNow.AddDays(7);

        // Act
        var task = new TaskEntity(userId, "Title", null, dueDate);

        // Assert
        Assert.NotEqual(Guid.Empty, task.Id);
        Assert.Null(task.Description);
        Assert.Equal(TaskStatus.Pending, task.Status);
    }

    [Fact]
    public void UpdateTask_WithNullDescription_ShouldSucceed()
    {
        // Arrange
        var task = new TaskEntity(Guid.NewGuid(), "Original", "Desc", DateTime.UtcNow.AddDays(7));

        // Act
        task.Update("Updated", null, DateTime.UtcNow.AddDays(10));

        // Assert
        Assert.Equal("Updated", task.Title);
        Assert.Null(task.Description);
    }

    [Fact]
    public void UpdateTask_WithEmptyTitle_ShouldThrowException()
    {
        // Arrange
        var task = new TaskEntity(Guid.NewGuid(), "Original", "Desc", DateTime.UtcNow.AddDays(7));

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            task.Update("", "Description", DateTime.UtcNow.AddDays(10)));
    }

    [Fact]
    public void UpdateTask_WithPastDueDate_ShouldThrowException()
    {
        // Arrange
        var task = new TaskEntity(Guid.NewGuid(), "Original", "Desc", DateTime.UtcNow.AddDays(7));

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            task.Update("Title", "Description", DateTime.UtcNow.AddDays(-1)));
    }

    [Fact]
    public void UpdateTask_ShouldSetUpdatedAt()
    {
        // Arrange
        var task = new TaskEntity(Guid.NewGuid(), "Original", "Desc", DateTime.UtcNow.AddDays(7));
        var beforeUpdate = DateTime.UtcNow;

        // Act
        task.Update("Updated", "New Desc", DateTime.UtcNow.AddDays(10));

        // Assert
        Assert.NotNull(task.UpdatedAt);
        Assert.True(task.UpdatedAt >= beforeUpdate);
    }
}

