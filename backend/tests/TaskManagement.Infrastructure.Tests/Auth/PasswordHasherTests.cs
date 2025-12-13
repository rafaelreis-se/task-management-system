using Xunit;
using TaskManagement.Infrastructure.Auth;

namespace TaskManagement.Infrastructure.Tests.Auth;

public class PasswordHasherTests
{
    private readonly PasswordHasher _passwordHasher;

    public PasswordHasherTests()
    {
        _passwordHasher = new PasswordHasher();
    }

    [Fact]
    public void HashPassword_ShouldReturnHashedPassword()
    {
        // Arrange
        var password = "SecurePassword123";

        // Act
        var hash = _passwordHasher.HashPassword(password);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEqual(password, hash);
    }

    [Fact]
    public void VerifyPassword_WithCorrectPassword_ShouldReturnTrue()
    {
        // Arrange
        var password = "SecurePassword123";
        var hash = _passwordHasher.HashPassword(password);

        // Act
        var result = _passwordHasher.VerifyPassword(password, hash);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_WithIncorrectPassword_ShouldReturnFalse()
    {
        // Arrange
        var password = "SecurePassword123";
        var hash = _passwordHasher.HashPassword(password);

        // Act
        var result = _passwordHasher.VerifyPassword("WrongPassword", hash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HashPassword_ShouldGenerateDifferentHashesForSamePassword()
    {
        // Arrange
        var password = "SecurePassword123";

        // Act
        var hash1 = _passwordHasher.HashPassword(password);
        var hash2 = _passwordHasher.HashPassword(password);

        // Assert
        Assert.NotEqual(hash1, hash2); // BCrypt uses salt, so hashes should differ
        Assert.True(_passwordHasher.VerifyPassword(password, hash1));
        Assert.True(_passwordHasher.VerifyPassword(password, hash2));
    }

    [Fact]
    public void HashPassword_WithEmptyString_ShouldReturnHash()
    {
        // Arrange
        var password = "";

        // Act
        var hash = _passwordHasher.HashPassword(password);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        Assert.True(_passwordHasher.VerifyPassword(password, hash));
    }

    [Fact]
    public void VerifyPassword_WithCaseSensitivePassword_ShouldBeCaseSensitive()
    {
        // Arrange
        var password = "SecurePassword123";
        var hash = _passwordHasher.HashPassword(password);

        // Act
        var resultLower = _passwordHasher.VerifyPassword("securepassword123", hash);
        var resultUpper = _passwordHasher.VerifyPassword("SECUREPASSWORD123", hash);

        // Assert
        Assert.False(resultLower);
        Assert.False(resultUpper);
    }
}

