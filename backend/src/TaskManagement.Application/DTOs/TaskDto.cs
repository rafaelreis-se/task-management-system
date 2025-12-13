using TaskManagement.Domain.ValueObjects;
using TaskStatus = TaskManagement.Domain.ValueObjects.TaskStatus;

namespace TaskManagement.Application.DTOs;

public record TaskDto(
    Guid Id,
    string Title,
    string? Description,
    TaskStatus Status,
    DateTime DueDate,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateTaskRequest(
    string Title,
    string? Description,
    DateTime DueDate
);

public record UpdateTaskRequest(
    string Title,
    string? Description,
    DateTime DueDate,
    TaskStatus Status
);

