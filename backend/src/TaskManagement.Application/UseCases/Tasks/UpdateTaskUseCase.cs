using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.UseCases.Tasks;

public class UpdateTaskUseCase
{
    private readonly ITaskRepository _taskRepository;

    public UpdateTaskUseCase(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskDto?> ExecuteAsync(Guid taskId, Guid userId, UpdateTaskRequest request)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);

        if (task == null || task.UserId != userId)
            return null;

        task.Update(request.Title, request.Description, request.DueDate);
        task.ChangeStatus(request.Status);

        await _taskRepository.UpdateAsync(task);

        return new TaskDto(
            task.Id,
            task.Title,
            task.Description,
            task.Status,
            task.DueDate,
            task.CreatedAt,
            task.UpdatedAt
        );
    }
}

