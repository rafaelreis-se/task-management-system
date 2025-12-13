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

    [Fact]
    public void CreateUser_EmailShouldBeLowercase()
    {
        // Arrange
        var email = "John@EXAMPLE.COM";

        // Act
        var user = new User("John Doe", email, "hash");

        // Assert
        Assert.Equal("john@example.com", user.Email);
    }

    [Fact]
    public void CreateUser_WithEmailMissingAtSign_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            new User("John Doe", "invalidemail.com", "hash"));
    }

    [Fact]
    public void CreateUser_WithEmailMissingDomain_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            new User("John Doe", "john@", "hash"));
    }

    [Fact]
    public void CreateUser_WithWhitespaceName_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            new User("   ", "john@example.com", "hash"));
    }

    [Fact]
    public void CreateUser_WithValidEmailVariations_ShouldSucceed()
    {
        // All should succeed
        var user1 = new User("User 1", "user+tag@example.com", "hash");
        var user2 = new User("User 2", "user.name@example.co.uk", "hash");
        var user3 = new User("User 3", "user_name@sub.example.com", "hash");

        Assert.Equal("user+tag@example.com", user1.Email);
        Assert.Equal("user.name@example.co.uk", user2.Email);
        Assert.Equal("user_name@sub.example.com", user3.Email);
    }
}

