using Application.DTOs.Auth;
using Application.Features.Auth.Commands;
using Application.Features.Auth.Validators;
using Application.Features.Profile.Commands;
using Application.Features.Profile.Validators;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized();

        var validator = new UpdateProfileValidator();
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList() });

        var command = new UpdateProfileCommand
        {
            UserId = userId,
            Username = request.Username,
            AvatarUrl = request.AvatarUrl
        };

        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Message });

        return Ok(new { message = result.Message });
    }

    [HttpPost("avatar")]
    public async Task<IActionResult> UploadAvatar([FromForm] IFormFile file)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized();

        if (file == null || file.Length == 0)
            return BadRequest(new { message = "No file uploaded" });

        // Avatar upload handling would go here
        // For MVP, just return success
        return Ok(new { message = "Avatar uploaded successfully", url = "" });
    }

    [HttpDelete("account")]
    public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountRequest request)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized();

        // Validate request
        var validator = new DeleteAccountValidator();
        var command = new DeleteAccountCommand
        {
            UserId = userId,
            Password = request.Password
        };

        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
            return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList() });

        // Execute delete
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Message });

        return Ok(new { message = result.Message });
    }
}
