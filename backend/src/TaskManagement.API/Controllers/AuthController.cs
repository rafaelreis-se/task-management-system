using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.UseCases.Auth;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly RegisterUserUseCase _registerUseCase;
    private readonly LoginUserUseCase _loginUseCase;

    public AuthController(RegisterUserUseCase registerUseCase, LoginUserUseCase loginUseCase)
    {
        _registerUseCase = registerUseCase;
        _loginUseCase = loginUseCase;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var user = await _registerUseCase.ExecuteAsync(request);
            return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var result = await _loginUseCase.ExecuteAsync(request);

        if (result == null)
            return Unauthorized(new { error = "Invalid email or password" });

        return Ok(result);
    }
}

