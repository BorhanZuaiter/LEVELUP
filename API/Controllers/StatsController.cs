using Application.DTOs.Stats;
using Application.Features.Stats.Commands;
using Application.Features.Stats.Queries;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StatsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StatsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetStats()
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());
        var query = new GetStatsQuery { UserId = userId };
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("monthly")]
    public async Task<IActionResult> GetMonthlyStats()
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());
        var query = new GetMonthlyStatsQuery { UserId = userId };
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpPost("increase")]
    public async Task<IActionResult> IncreaseStat([FromBody] IncreaseStatRequest request)
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());

        var difficulty = (Difficulty)request.Difficulty;
        var statCategory = (StatCategory)request.StatCategory;

        var command = new IncreaseStatCommand
        {
            UserId = userId,
            Difficulty = difficulty,
            StatCategory = statCategory
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

public class IncreaseStatRequest
{
    public int Difficulty { get; set; }
    public int StatCategory { get; set; }
}
