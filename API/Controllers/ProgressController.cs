using Application.Common.Result;
using Application.DTOs.Progression;
using Application.Features.Progress.Commands;
using Application.Features.Progress.Queries;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProgressController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProgressController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetProgress()
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());
        var query = new GetProgressQuery { UserId = userId };
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpPost("xp")]
    public async Task<IActionResult> GainXP([FromBody] GainXPRequest request)
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());

        var difficulty = (Difficulty)request.Difficulty;
        var command = new GainXPCommand
        {
            UserId = userId,
            Difficulty = difficulty,
            Priority = request.Priority,
            DurationMinutes = request.DurationMinutes,
            IsRequired = request.IsRequired,
            CompletionCount = request.CompletionCount
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("hp")]
    public async Task<IActionResult> ApplyHPDamage([FromBody] ApplyHPDamageRequest request)
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());

        var difficulty = (Difficulty)request.Difficulty;
        var command = new ApplyHPDamageCommand
        {
            UserId = userId,
            Difficulty = difficulty
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("streak")]
    public async Task<IActionResult> UpdateStreak([FromBody] UpdateStreakRequest request)
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());
        var command = new UpdateStreakCommand
        {
            UserId = userId,
            AllRequiredTasksCompleted = request.AllRequiredTasksCompleted
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

public class GainXPRequest
{
    public int Difficulty { get; set; }
    public int Priority { get; set; }
    public int DurationMinutes { get; set; }
    public bool IsRequired { get; set; }
    public int CompletionCount { get; set; }
}

public class ApplyHPDamageRequest
{
    public int Difficulty { get; set; }
}

public class UpdateStreakRequest
{
    public bool AllRequiredTasksCompleted { get; set; }
}
