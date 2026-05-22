using Application.Common.Result;
using Application.DTOs.Auth;
using Domain.Interfaces.User;
using MediatR;

namespace Application.Features.Auth.Queries;

public class GetCurrentUserQuery : IRequest<Result<UserInfoDto>>
{
    public Guid UserId { get; set; }
}

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, Result<UserInfoDto>>
{
    private readonly IUserRepository _userRepository;

    public GetCurrentUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserInfoDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            return Result.Failure<UserInfoDto>("User not found");

        var userInfo = new UserInfoDto
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
        };

        return Result.Success(userInfo);
    }
}
