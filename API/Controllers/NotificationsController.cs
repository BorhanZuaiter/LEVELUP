using Application.Common.Result;
using Application.DTOs.Notifications;
using Application.Features.Notifications.Commands;
using Application.Features.Notifications.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async global::System.Threading.Tasks.Task<ActionResult<Result<NotificationDto>>> CreateReminder([FromBody] CreateReminderRequest request)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var command = new CreateReminderCommand
        {
            UserId = Guid.Parse(userId),
            TaskId = request.TaskId,
            ReminderTime = request.ReminderTime
        };

        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async global::System.Threading.Tasks.Task<ActionResult<Result<NotificationDto>>> UpdateReminder(Guid id, [FromBody] UpdateReminderRequest request)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var command = new UpdateReminderCommand
        {
            Id = id,
            UserId = Guid.Parse(userId),
            ReminderTime = request.ReminderTime
        };

        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async global::System.Threading.Tasks.Task<ActionResult<Result<Unit>>> DeleteReminder(Guid id)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var command = new DeleteReminderCommand
        {
            Id = id,
            UserId = Guid.Parse(userId)
        };

        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet]
    public async global::System.Threading.Tasks.Task<ActionResult<Result<List<NotificationDto>>>> GetReminders()
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var query = new GetRemindersQuery { UserId = Guid.Parse(userId) };
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
