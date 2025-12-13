using System.Data;
using Npgsql;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.ValueObjects;
using TaskManagement.Infrastructure.Data;
using TaskStatus = TaskManagement.Domain.ValueObjects.TaskStatus;

namespace TaskManagement.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public TaskRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<TaskEntity?> GetByIdAsync(Guid id)
    {
        using var connection = (NpgsqlConnection)_connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = new NpgsqlCommand(@"
            SELECT id, user_id, title, description, status, due_date, created_at, updated_at
            FROM tasks
            WHERE id = @id", connection);

        command.Parameters.AddWithValue("@id", id);

        using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return MapToEntity(reader);
    }

    public async Task<IEnumerable<TaskEntity>> GetByUserIdAsync(Guid userId)
    {
        using var connection = (NpgsqlConnection)_connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = new NpgsqlCommand(@"
            SELECT id, user_id, title, description, status, due_date, created_at, updated_at
            FROM tasks
            WHERE user_id = @userId
            ORDER BY created_at DESC", connection);

        command.Parameters.AddWithValue("@userId", userId);

        var tasks = new List<TaskEntity>();
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            tasks.Add(MapToEntity(reader));
        }

        return tasks;
    }

    public async Task<TaskEntity> CreateAsync(TaskEntity task)
    {
        using var connection = (NpgsqlConnection)_connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = new NpgsqlCommand(@"
            INSERT INTO tasks (id, user_id, title, description, status, due_date, created_at)
            VALUES (@id, @userId, @title, @description, @status, @dueDate, @createdAt)", connection);

        command.Parameters.AddWithValue("@id", task.Id);
        command.Parameters.AddWithValue("@userId", task.UserId);
        command.Parameters.AddWithValue("@title", task.Title);
        command.Parameters.AddWithValue("@description", (object?)task.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("@status", task.Status.ToString());
        command.Parameters.AddWithValue("@dueDate", task.DueDate);
        command.Parameters.AddWithValue("@createdAt", task.CreatedAt);

        await command.ExecuteNonQueryAsync();
        return task;
    }

    public async Task UpdateAsync(TaskEntity task)
    {
        using var connection = (NpgsqlConnection)_connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = new NpgsqlCommand(@"
            UPDATE tasks
            SET title = @title, description = @description, status = @status, 
                due_date = @dueDate, updated_at = @updatedAt
            WHERE id = @id", connection);

        command.Parameters.AddWithValue("@id", task.Id);
        command.Parameters.AddWithValue("@title", task.Title);
        command.Parameters.AddWithValue("@description", (object?)task.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("@status", task.Status.ToString());
        command.Parameters.AddWithValue("@dueDate", task.DueDate);
        command.Parameters.AddWithValue("@updatedAt", (object?)task.UpdatedAt ?? DBNull.Value);

        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        using var connection = (NpgsqlConnection)_connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = new NpgsqlCommand("DELETE FROM tasks WHERE id = @id", connection);

        command.Parameters.AddWithValue("@id", id);

        await command.ExecuteNonQueryAsync();
    }

    private TaskEntity MapToEntity(IDataReader reader)
    {
        var userId = reader.GetGuid(reader.GetOrdinal("user_id"));
        var title = reader.GetString(reader.GetOrdinal("title"));
        var description = reader.IsDBNull(reader.GetOrdinal("description"))
            ? null
            : reader.GetString(reader.GetOrdinal("description"));
        var dueDate = reader.GetDateTime(reader.GetOrdinal("due_date"));

        var task = (TaskEntity)Activator.CreateInstance(typeof(TaskEntity), true)!;

        SetProperty(task, "Id", reader.GetGuid(reader.GetOrdinal("id")));
        SetProperty(task, "UserId", userId);
        SetProperty(task, "Title", title);
        SetProperty(task, "Description", description);
        SetProperty(task, "Status", Enum.Parse<TaskStatus>(reader.GetString(reader.GetOrdinal("status"))));
        SetProperty(task, "DueDate", dueDate);
        SetProperty(task, "CreatedAt", reader.GetDateTime(reader.GetOrdinal("created_at")));

        if (!reader.IsDBNull(reader.GetOrdinal("updated_at")))
            SetProperty(task, "UpdatedAt", reader.GetDateTime(reader.GetOrdinal("updated_at")));

        return task;
    }

    private void SetProperty(object obj, string propertyName, object value)
    {
        var property = obj.GetType().GetProperty(propertyName);
        property?.SetValue(obj, value);
    }
}

