using Application.Common.Interfaces;
using Application.Common.Result;
using Application.DTOs.Auth;
using Domain.Interfaces.User;
using MediatR;

namespace Application.Features.Auth.Commands;

public class RefreshCommand : IRequest<Result<AuthResponse>>
{
    public string RefreshToken { get; set; } = string.Empty;
}

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, Result<AuthResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public RefreshCommandHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<AuthResponse>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(Guid.Parse("00000000-0000-0000-0000-000000000000"));

        if (user == null || user.RefreshToken != request.RefreshToken ||
            user.RefreshTokenExpiry < DateTime.UtcNow)
            return Result.Failure<AuthResponse>("Invalid refresh token");

        var newAccessToken = _jwtTokenService.GenerateAccessToken(user);
        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        await _userRepository.UpdateAsync(user);

        var response = new AuthResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
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
