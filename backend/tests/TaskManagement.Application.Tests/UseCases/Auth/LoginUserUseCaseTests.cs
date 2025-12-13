using Moq;
using Xunit;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.UseCases.Auth;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Tests.UseCases.Auth;

public class LoginUserUseCaseTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly Mock<IJwtTokenGenerator> _mockJwtTokenGenerator;
    private readonly LoginUserUseCase _useCase;

    public LoginUserUseCaseTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _mockJwtTokenGenerator = new Mock<IJwtTokenGenerator>();
        _useCase = new LoginUserUseCase(
            _mockUserRepository.Object,
            _mockPasswordHasher.Object,
            _mockJwtTokenGenerator.Object
        );
    }

    [Fact]
    public async Task Execute_WithValidCredentials_ShouldReturnToken()
    {
        // Arrange
        var request = new LoginRequest("john@example.com", "SecurePass123");
        var user = new User("John Doe", "john@example.com", "hashed_password");

        _mockUserRepository
            .Setup(r => r.GetByEmailAsync(request.Email))
            .ReturnsAsync(user);

        _mockPasswordHasher
            .Setup(h => h.VerifyPassword(request.Password, user.PasswordHash))
            .Returns(true);

        _mockJwtTokenGenerator
            .Setup(g => g.GenerateToken(user))
            .Returns("jwt_token_here");

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("jwt_token_here", result.Token);
        Assert.Equal(user.Email, result.User.Email);
    }

    [Fact]
    public async Task Execute_WithInvalidEmail_ShouldReturnNull()
    {
        // Arrange
        var request = new LoginRequest("nonexistent@example.com", "password");

        _mockUserRepository
            .Setup(r => r.GetByEmailAsync(request.Email))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Execute_WithInvalidPassword_ShouldReturnNull()
    {
        // Arrange
        var request = new LoginRequest("john@example.com", "WrongPassword");
        var user = new User("John Doe", "john@example.com", "hashed_password");

        _mockUserRepository
            .Setup(r => r.GetByEmailAsync(request.Email))
            .ReturnsAsync(user);

        _mockPasswordHasher
            .Setup(h => h.VerifyPassword(request.Password, user.PasswordHash))
            .Returns(false);

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        Assert.Null(result);
        _mockJwtTokenGenerator.Verify(g => g.GenerateToken(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Execute_ShouldGenerateTokenWithCorrectUser()
    {
        // Arrange
        var request = new LoginRequest("jane@example.com", "Password123");
        var user = new User("Jane Doe", "jane@example.com", "hashed_password");

        _mockUserRepository
            .Setup(r => r.GetByEmailAsync(request.Email))
            .ReturnsAsync(user);

        _mockPasswordHasher
            .Setup(h => h.VerifyPassword(request.Password, user.PasswordHash))
            .Returns(true);

        _mockJwtTokenGenerator
            .Setup(g => g.GenerateToken(user))
            .Returns("token_for_jane");

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Name, result.User.Name);
        Assert.Equal(user.Email, result.User.Email);
        _mockJwtTokenGenerator.Verify(g => g.GenerateToken(user), Times.Once);
    }
}

