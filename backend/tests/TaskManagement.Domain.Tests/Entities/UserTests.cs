using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Exceptions;
using Xunit;

namespace TaskManagement.Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void CreateUser_WithValidData_ShouldSucceed()
    {
        // Arrange
        var name = "John Doe";
        var email = "john@example.com";
        var passwordHash = "hashed_password_123";

        // Act
        var user = new User(name, email, passwordHash);

        // Assert
        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal(name, user.Name);
        Assert.Equal(email, user.Email);
        Assert.Equal(passwordHash, user.PasswordHash);
        Assert.True(user.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void CreateUser_WithEmptyName_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            new User("", "john@example.com", "hash"));
    }

    [Fact]
    public void CreateUser_WithInvalidEmail_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            new User("John Doe", "invalid-email", "hash"));
    }

    [Fact]
    public void CreateUser_WithEmptyPasswordHash_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            new User("John Doe", "john@example.com", ""));
    }
}

