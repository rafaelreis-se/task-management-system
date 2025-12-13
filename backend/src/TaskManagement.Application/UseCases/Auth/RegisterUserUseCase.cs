using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.UseCases.Auth;

public class RegisterUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserDto> ExecuteAsync(RegisterRequest request)
    {
        if (request.Password.Length < 8)
            throw new DomainException("Password must be at least 8 characters long");

        var emailExists = await _userRepository.EmailExistsAsync(request.Email);
        if (emailExists)
            throw new DomainException("Email already registered");

        var passwordHash = _passwordHasher.HashPassword(request.Password);

        var user = new User(request.Name, request.Email, passwordHash);

        var createdUser = await _userRepository.CreateAsync(user);

        return new UserDto(
            createdUser.Id,
            createdUser.Name,
            createdUser.Email,
            createdUser.CreatedAt
        );
    }
}

