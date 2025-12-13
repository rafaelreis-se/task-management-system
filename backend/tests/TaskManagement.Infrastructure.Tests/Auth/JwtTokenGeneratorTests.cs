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
    }
}

