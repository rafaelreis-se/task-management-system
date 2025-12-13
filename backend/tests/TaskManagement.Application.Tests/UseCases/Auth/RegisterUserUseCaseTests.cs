using Moq;
using Xunit;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.UseCases.Auth;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Tests.UseCases.Auth;

public class RegisterUserUseCaseTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly RegisterUserUseCase _useCase;

    public RegisterUserUseCaseTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _useCase = new RegisterUserUseCase(_mockUserRepository.Object, _mockPasswordHasher.Object);
    }

    [Fact]
    public async Task Execute_WithValidData_ShouldRegisterUser()
    {
        // Arrange
        var request = new RegisterRequest("John Doe", "john@example.com", "SecurePass123");

        _mockUserRepository
            .Setup(r => r.EmailExistsAsync(request.Email))
            .ReturnsAsync(false);

        _mockPasswordHasher
            .Setup(h => h.HashPassword(request.Password))
            .Returns("hashed_password");

        _mockUserRepository
            .Setup(r => r.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) => u);

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Name, result.Name);
        Assert.Equal(request.Email.ToLower(), result.Email);
        _mockPasswordHasher.Verify(h => h.HashPassword(request.Password), Times.Once);
        _mockUserRepository.Verify(r => r.CreateAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Execute_WithExistingEmail_ShouldThrowException()
    {
        // Arrange
        var request = new RegisterRequest("John Doe", "john@example.com", "SecurePass123");

        _mockUserRepository
            .Setup(r => r.EmailExistsAsync(request.Email))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _useCase.ExecuteAsync(request));
    }

    [Fact]
    public async Task Execute_WithShortPassword_ShouldThrowException()
    {
        // Arrange
        var request = new RegisterRequest("John Doe", "john@example.com", "short");

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _useCase.ExecuteAsync(request));
    }
}

