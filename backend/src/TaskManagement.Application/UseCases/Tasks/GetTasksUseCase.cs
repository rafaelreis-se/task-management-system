using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.UseCases.Tasks;

public class GetTasksUseCase
{
    private readonly ITaskRepository _taskRepository;

    public GetTasksUseCase(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IEnumerable<TaskDto>> ExecuteAsync(Guid userId)
    {
        var tasks = await _taskRepository.GetByUserIdAsync(userId);

        return tasks.Select(task => new TaskDto(
            task.Id,
            task.Title,
            task.Description,
            task.Status,
            task.DueDate,
            task.CreatedAt,
            task.UpdatedAt
        ));
    }
}

