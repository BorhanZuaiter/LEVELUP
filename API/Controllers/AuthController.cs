using Application.DTOs.Auth;
using Application.Features.Auth.Commands;
using Application.Features.Auth.Queries;
using Application.Features.Auth.Validators;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var validator = new RegisterValidator();
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList() });

        var command = new RegisterCommand
        {
            Username = request.Username,
            Email = request.Email,
            Password = request.Password
        };

        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Message });

        return Ok(new { message = result.Message });
    }

    [HttpPost("login")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var validator = new LoginValidator();
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList() });

        var command = new LoginCommand
        {
            Email = request.Email,
            Password = request.Password
        };

        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return Unauthorized(new { message = result.Message });

        return Ok(result.Data);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        var command = new RefreshCommand { RefreshToken = request.RefreshToken };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return Unauthorized(new { message = result.Message });

        return Ok(result.Data);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized();

        var command = new LogoutCommand { UserId = userId };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { message = result.Message });

        return Ok(new { message = result.Message });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var command = new ForgotPasswordCommand { Email = request.Email };
        var result = await _mediator.Send(command);

        return Ok(new { message = result.Message });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var validator = new ResetPasswordValidator();
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList() });

        var command = new ResetPasswordCommand
        {
            Token = request.Token,
            Password = request.Password
        };

        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Message });

        return Ok(new { message = result.Message });
    }

    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
    {
        var command = new VerifyEmailCommand { Token = request.Token };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { message = result.Message });

        return Ok(new { message = result.Message });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized();

        var query = new GetCurrentUserQuery { UserId = userId };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(new { message = result.Message });

        return Ok(result.Data);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized();

        var command = new ChangePasswordCommand
        {
            UserId = userId,
            CurrentPassword = request.CurrentPassword,
            NewPassword = request.NewPassword
        };

        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Message });

        return Ok(new { message = result.Message });
    }
}
