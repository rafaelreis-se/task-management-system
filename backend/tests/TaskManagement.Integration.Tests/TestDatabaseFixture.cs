using Npgsql;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Integration.Tests;

public class TestDatabaseFixture : IDisposable
{
    public string ConnectionString { get; }
    public DbConnectionFactory ConnectionFactory { get; }

    public TestDatabaseFixture()
    {
        // Usa variável de ambiente ou fallback para porta de teste
        var port = Environment.GetEnvironmentVariable("TEST_DB_PORT") ?? "5433";
        var database = Environment.GetEnvironmentVariable("TEST_DB_NAME") ?? "taskmanagement_test";
        
        ConnectionString = $"Host=localhost;Port={port};Database={database};Username=postgres;Password=postgres";
        ConnectionFactory = new DbConnectionFactory(ConnectionString);
        
        CleanDatabase().Wait();
    }

    private async System.Threading.Tasks.Task CleanDatabase()
    {
        try
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                DELETE FROM tasks;
                DELETE FROM users;
            ";
            await command.ExecuteNonQueryAsync();
        }
        catch
        {
            // Database might not be running
        }
    }

    public void Dispose()
    {
        // Cleanup
    }
}
