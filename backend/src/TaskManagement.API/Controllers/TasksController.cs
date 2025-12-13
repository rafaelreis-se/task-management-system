using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.UseCases.Tasks;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly CreateTaskUseCase _createTaskUseCase;
    private readonly GetTasksUseCase _getTasksUseCase;
    private readonly UpdateTaskUseCase _updateTaskUseCase;
    private readonly DeleteTaskUseCase _deleteTaskUseCase;

    public TasksController(
        CreateTaskUseCase createTaskUseCase,
        GetTasksUseCase getTasksUseCase,
        UpdateTaskUseCase updateTaskUseCase,
        DeleteTaskUseCase deleteTaskUseCase)
    {
        _createTaskUseCase = createTaskUseCase;
        _getTasksUseCase = getTasksUseCase;
        _updateTaskUseCase = updateTaskUseCase;
        _deleteTaskUseCase = deleteTaskUseCase;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks()
    {
        var userId = GetUserId();
        var tasks = await _getTasksUseCase.ExecuteAsync(userId);
        return Ok(tasks);
    }

    [HttpPost]
    public async Task<ActionResult<TaskDto>> CreateTask([FromBody] CreateTaskRequest request)
    {
        try
        {
            var userId = GetUserId();
            var task = await _createTaskUseCase.ExecuteAsync(userId, request);
            return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskDto>> UpdateTask(Guid id, [FromBody] UpdateTaskRequest request)
    {
        try
        {
            var userId = GetUserId();
            var task = await _updateTaskUseCase.ExecuteAsync(id, userId, request);

            if (task == null)
                return NotFound(new { error = "Task not found" });

            return Ok(task);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTask(Guid id)
    {
        var userId = GetUserId();
        var success = await _deleteTaskUseCase.ExecuteAsync(id, userId);

        if (!success)
            return NotFound(new { error = "Task not found" });

        return NoContent();
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }
}

