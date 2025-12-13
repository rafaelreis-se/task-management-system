namespace TaskManagement.Application.DTOs;

public record UserDto(
    Guid Id,
    string Name,
    string Email,
    DateTime CreatedAt
);

public record RegisterRequest(
    string Name,
    string Email,
    string Password
);

public record LoginRequest(
    string Email,
    string Password
);

public record LoginResponse(
    string Token,
    UserDto User
);

