using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.ValueObjects;
using TaskStatus = TaskManagement.Domain.ValueObjects.TaskStatus;

namespace TaskManagement.Domain.Entities;

public class TaskEntity
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public TaskStatus Status { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public TaskEntity(Guid userId, string title, string? description, DateTime dueDate)
    {
        ValidateTitle(title);
        ValidateDueDate(dueDate);

        Id = Guid.NewGuid();
        UserId = userId;
        Title = title;
        Description = description;
        Status = TaskStatus.Pending;
        DueDate = dueDate;
        CreatedAt = DateTime.UtcNow;
    }

    private TaskEntity() { }

    public void Update(string title, string? description, DateTime dueDate)
    {
        ValidateTitle(title);
        ValidateDueDate(dueDate);

        Title = title;
        Description = description;
        DueDate = dueDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeStatus(TaskStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    private void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Task title cannot be empty");

        if (title.Length > 200)
            throw new DomainException("Task title cannot exceed 200 characters");
    }

    private void ValidateDueDate(DateTime dueDate)
    {
        if (dueDate.Date < DateTime.UtcNow.Date)
            throw new DomainException("Task due date cannot be in the past");
    }
}

