using System.IdentityModel.Tokens.Jwt;
using Xunit;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Auth;

namespace TaskManagement.Infrastructure.Tests.Auth;

public class JwtTokenGeneratorTests
{
    [Fact]
    public void GenerateToken_ShouldReturnValidJwtToken()
    {
        // Arrange
        var secret = "this-is-a-very-secure-secret-key-for-testing-purposes-min-32-chars";
        var generator = new JwtTokenGenerator(secret, 60);
        var user = new User("John Doe", "john@example.com", "hashed_password");

        // Act
        var token = generator.GenerateToken(user);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        Assert.Equal(user.Id.ToString(), jwtToken.Claims.First(c => c.Type == "nameid").Value);
        Assert.Equal(user.Email, jwtToken.Claims.First(c => c.Type == "email").Value);
        Assert.Equal(user.Name, jwtToken.Claims.First(c => c.Type == "unique_name").Value);
    }

    [Fact]
    public void GenerateToken_ShouldIncludeExpirationTime()
    {
        // Arrange
        var secret = "this-is-a-very-secure-secret-key-for-testing-purposes-min-32-chars";
        var expirationMinutes = 30;
        var generator = new JwtTokenGenerator(secret, expirationMinutes);
        var user = new User("Jane Doe", "jane@example.com", "hashed_password");

        // Act
        var token = generator.GenerateToken(user);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // ValidTo is DateTime (value type), always has a value
        Assert.True(jwtToken.ValidTo > DateTime.UtcNow);
        Assert.True(jwtToken.ValidTo <= DateTime.UtcNow.AddMinutes(expirationMinutes + 1));
    }

    [Fact]
    public void GenerateToken_ShouldIncludeAllUserClaims()
    {
        // Arrange
        var secret = "this-is-a-very-secure-secret-key-for-testing-purposes-min-32-chars";
        var generator = new JwtTokenGenerator(secret, 60);
        var user = new User("Test User", "test@example.com", "hashed_password");

        // Act
        var token = generator.GenerateToken(user);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        Assert.Contains(jwtToken.Claims, c => c.Type == "nameid" && c.Value == user.Id.ToString());
        Assert.Contains(jwtToken.Claims, c => c.Type == "email" && c.Value == user.Email);
        Assert.Contains(jwtToken.Claims, c => c.Type == "unique_name" && c.Value == user.Name);
    }

    [Fact]
    public void GenerateToken_WithDifferentUsers_ShouldGenerateDifferentTokens()
    {
        // Arrange
        var secret = "this-is-a-very-secure-secret-key-for-testing-purposes-min-32-chars";
        var generator = new JwtTokenGenerator(secret, 60);
        var user1 = new User("User One", "user1@example.com", "hashed_password");
        var user2 = new User("User Two", "user2@example.com", "hashed_password");

        // Act
        var token1 = generator.GenerateToken(user1);
        var token2 = generator.GenerateToken(user2);

        // Assert
        Assert.NotEqual(token1, token2);
    }
}

