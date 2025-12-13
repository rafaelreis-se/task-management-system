using System.Data;
using Npgsql;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public UserRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        using var connection = (NpgsqlConnection)_connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = new NpgsqlCommand(@"
            SELECT id, name, email, password_hash, created_at
            FROM users
            WHERE id = @id", connection);

        command.Parameters.AddWithValue("@id", id);

        using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return MapToEntity(reader);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        using var connection = (NpgsqlConnection)_connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = new NpgsqlCommand(@"
            SELECT id, name, email, password_hash, created_at
            FROM users
            WHERE email = @email", connection);

        command.Parameters.AddWithValue("@email", email.ToLower());

        using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return MapToEntity(reader);
    }

    public async Task<User> CreateAsync(User user)
    {
        using var connection = (NpgsqlConnection)_connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = new NpgsqlCommand(@"
            INSERT INTO users (id, name, email, password_hash, created_at)
            VALUES (@id, @name, @email, @passwordHash, @createdAt)", connection);

        command.Parameters.AddWithValue("@id", user.Id);
        command.Parameters.AddWithValue("@name", user.Name);
        command.Parameters.AddWithValue("@email", user.Email);
        command.Parameters.AddWithValue("@passwordHash", user.PasswordHash);
        command.Parameters.AddWithValue("@createdAt", user.CreatedAt);

        await command.ExecuteNonQueryAsync();
        return user;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        using var connection = (NpgsqlConnection)_connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = new NpgsqlCommand("SELECT COUNT(1) FROM users WHERE email = @email", connection);

        command.Parameters.AddWithValue("@email", email.ToLower());

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt64(result) > 0;
    }

    private User MapToEntity(IDataReader reader)
    {
        var name = reader.GetString(reader.GetOrdinal("name"));
        var email = reader.GetString(reader.GetOrdinal("email"));
        var passwordHash = reader.GetString(reader.GetOrdinal("password_hash"));

        var user = (User)Activator.CreateInstance(typeof(User), true)!;

        SetProperty(user, "Id", reader.GetGuid(reader.GetOrdinal("id")));
        SetProperty(user, "Name", name);
        SetProperty(user, "Email", email);
        SetProperty(user, "PasswordHash", passwordHash);
        SetProperty(user, "CreatedAt", reader.GetDateTime(reader.GetOrdinal("created_at")));

        return user;
    }

    private void SetProperty(object obj, string propertyName, object value)
    {
        var property = obj.GetType().GetProperty(propertyName);
        property?.SetValue(obj, value);
    }
}

