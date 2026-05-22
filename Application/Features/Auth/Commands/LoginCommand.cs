using Application.Common.Interfaces;
using Application.Common.Result;
using Application.DTOs.Auth;
using Domain.Interfaces.User;
using MediatR;

namespace Application.Features.Auth.Commands;

public class LoginCommand : IRequest<Result<AuthResponse>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordService passwordService,
        IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null || !_passwordService.VerifyPassword(request.Password, user.PasswordHash))
            return Result.Failure<AuthResponse>("Invalid email or password");

        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        await _userRepository.UpdateAsync(user);

        var response = new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            User = new UserInfoDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl,
                Level = user.Level,
                XP = user.XP,
                CurrentHP = user.CurrentHP,
                MaxHP = user.MaxHP,
                CurrentStreak = user.CurrentStreak,
                ShieldCount = user.ShieldCount
            }
        };

        return Result.Success(response);
    }
}
