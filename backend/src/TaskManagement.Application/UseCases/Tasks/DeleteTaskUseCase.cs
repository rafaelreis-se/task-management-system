using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.UseCases.Tasks;

public class DeleteTaskUseCase
{
    private readonly ITaskRepository _taskRepository;

    public DeleteTaskUseCase(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<bool> ExecuteAsync(Guid taskId, Guid userId)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);

        if (task == null || task.UserId != userId)
            return false;

        await _taskRepository.DeleteAsync(taskId);
        return true;
    }
}

