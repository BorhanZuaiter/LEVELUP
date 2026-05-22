using Application.Common.Result;
using Application.DTOs.Tasks;
using Application.Features.Tasks.Commands;
using Application.Features.Tasks.Queries;
using Domain.Interfaces.Task;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());
        var command = new CreateTaskCommand { UserId = userId, Request = request };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetTaskById), new { id = result.Data?.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks([FromQuery] int? color, [FromQuery] int? statCategory,
        [FromQuery] int? status, [FromQuery] bool? isRequired, [FromQuery] int? priority)
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());

        var criteria = new TaskFilterCriteria
        {
            Color = color,
            StatCategory = statCategory,
            Status = status,
            IsRequired = isRequired,
            Priority = priority
        };

        var query = new GetTasksQuery { UserId = userId, FilterCriteria = criteria };
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());
        var query = new GetTaskByIdQuery { TaskId = id, UserId = userId };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchTasks([FromQuery] string? query, [FromQuery] int? color,
        [FromQuery] int? statCategory, [FromQuery] int? status, [FromQuery] bool? isRequired,
        [FromQuery] int? priority)
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());

        var request = new SearchTasksRequest
        {
            Query = query,
            Color = color,
            StatCategory = statCategory,
            Status = status,
            IsRequired = isRequired,
            Priority = priority
        };

        var searchQuery = new SearchTasksQuery { UserId = userId, Request = request };
        var result = await _mediator.Send(searchQuery);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskRequest request)
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());
        var command = new UpdateTaskCommand { TaskId = id, UserId = userId, Request = request };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());
        var command = new DeleteTaskCommand { TaskId = id, UserId = userId };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPost("{id}/complete")]
    public async Task<IActionResult> CompleteTask(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());
        var command = new CompleteTaskCommand { TaskId = id, UserId = userId };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPost("{id}/restore")]
    public async Task<IActionResult> RestoreTask(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());
        var command = new RestoreTaskCommand { TaskId = id, UserId = userId };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("by-status/{status}")]
    public async Task<IActionResult> GetTasksByStatus(int status)
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());
        var taskStatus = (Domain.Enums.TaskStatus)status;
        var query = new GetTasksByStatusQuery { UserId = userId, Status = taskStatus };
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("by-color/{color}")]
    public async Task<IActionResult> GetTasksByColor(int color)
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());
        var query = new GetTasksByColorQuery { UserId = userId, Color = color };
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("required")]
    public async Task<IActionResult> GetRequiredTasks()
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());
        var query = new GetRequiredTasksQuery { UserId = userId };
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}
