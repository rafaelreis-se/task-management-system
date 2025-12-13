using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Repositories;

namespace TaskManagement.Integration.Tests.Repositories;

[Collection("Database")]
public class UserRepositoryIntegrationTests : IClassFixture<TestDatabaseFixture>
{
    private readonly UserRepository _repository;

    public UserRepositoryIntegrationTests(TestDatabaseFixture fixture)
    {
        _repository = new UserRepository(fixture.ConnectionFactory);
    }

    [Fact]
    public async System.Threading.Tasks.Task CreateAndGetUser_ShouldWork()
    {
        // Arrange
        var email = $"integration{Guid.NewGuid()}@test.com";
        var user = new User("Test User", email, "hashedPassword123");

        // Act
        var created = await _repository.CreateAsync(user);
        var retrieved = await _repository.GetByIdAsync(created.Id);

        // Assert
        Assert.NotNull(retrieved);
        Assert.Equal(email, retrieved.Email);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetByEmailAsync_ShouldFindUser()
    {
        // Arrange
        var email = $"findme{Guid.NewGuid()}@test.com";
        var user = new User("Find Me", email, "hashedPassword123");
        await _repository.CreateAsync(user);

        // Act
        var result = await _repository.GetByEmailAsync(email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(email, result.Email);
    }

    [Fact]
    public async System.Threading.Tasks.Task EmailExistsAsync_ShouldReturnTrueForExisting()
    {
        // Arrange
        var email = $"exists{Guid.NewGuid()}@test.com";
        var user = new User("User", email, "hashedPassword123");
        await _repository.CreateAsync(user);

        // Act
        var result = await _repository.EmailExistsAsync(email);

        // Assert
        Assert.True(result);
    }
}
