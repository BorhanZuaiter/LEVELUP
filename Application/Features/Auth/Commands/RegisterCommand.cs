using Application.Common.Interfaces;
using Application.Common.Result;
using Application.DTOs.Auth;
using Domain.Entities.User;
using Domain.Interfaces.User;
using MediatR;

namespace Application.Features.Auth.Commands;

public class RegisterCommand : IRequest<Result>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;

    public RegisterCommandHandler(IUserRepository userRepository, IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
    }

    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.EmailExistsAsync(request.Email))
            return Result.Failure("Email already registered");

        if (await _userRepository.UsernameExistsAsync(request.Username))
            return Result.Failure("Username already taken");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            PasswordHash = _passwordService.HashPassword(request.Password),
            JoinDate = DateTime.UtcNow,
            EmailVerified = false,
            Level = 1,
            XP = 0,
            CurrentHP = 100,
            MaxHP = 100,
            CurrentStreak = 0,
            ShieldCount = 0
        };

        var stats = new Domain.Entities.Stats.Stats
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Power = 0,
            Wisdom = 0,
            Luck = 0,
            Determination = 0,
            UpdatedAt = DateTime.UtcNow
        };

        user.Stats = stats;

        await _userRepository.AddAsync(user);

        return Result.Success("Registration successful. Please verify your email.");
    }
}
