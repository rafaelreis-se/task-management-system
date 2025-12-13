using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.UseCases.Tasks;

public class CreateTaskUseCase
{
    private readonly ITaskRepository _taskRepository;

    public CreateTaskUseCase(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskDto> ExecuteAsync(Guid userId, CreateTaskRequest request)
    {
        var task = new TaskEntity(
            userId,
            request.Title,
            request.Description,
            request.DueDate
        );

        var createdTask = await _taskRepository.CreateAsync(task);

        return new TaskDto(
            createdTask.Id,
            createdTask.Title,
            createdTask.Description,
            createdTask.Status,
            createdTask.DueDate,
            createdTask.CreatedAt,
            createdTask.UpdatedAt
        );
    }
}

