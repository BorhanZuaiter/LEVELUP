using Application.DTOs.Journal;
using Application.Features.Journal.Commands;
using Application.Features.Journal.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JournalController : ControllerBase
{
    private readonly IMediator _mediator;

    public JournalController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateJournal([FromBody] CreateJournalRequest request)
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());
        var command = new CreateJournalCommand { UserId = userId, Request = request };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetTimeline), new { }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateJournal(Guid id, [FromBody] UpdateJournalRequest request)
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());
        var command = new UpdateJournalCommand { Id = id, UserId = userId, Request = request };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJournal(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());
        var command = new DeleteJournalCommand { Id = id, UserId = userId };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("timeline")]
    public async Task<IActionResult> GetTimeline()
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());
        var query = new GetTimelineQuery { UserId = userId };
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("date")]
    public async Task<IActionResult> GetByDate([FromQuery] DateTime date)
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());
        var query = new GetByDateQuery { UserId = userId, Date = date };
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchJournal([FromQuery] string title)
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());
        var query = new SearchJournalQuery { UserId = userId, Title = title };
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("on-this-day")]
    public async Task<IActionResult> GetOnThisDay([FromQuery] int month, [FromQuery] int day)
    {
        var userId = Guid.Parse(User.FindFirst("id")?.Value ?? throw new UnauthorizedAccessException());
        var query = new GetOnThisDayQuery { UserId = userId, Month = month, Day = day };
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}
