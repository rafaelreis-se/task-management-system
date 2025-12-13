using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces;

public interface ITaskRepository
{
    Task<TaskEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TaskEntity>> GetByUserIdAsync(Guid userId);
    Task<TaskEntity> CreateAsync(TaskEntity task);
    Task UpdateAsync(TaskEntity task);
    Task DeleteAsync(Guid id);
}

