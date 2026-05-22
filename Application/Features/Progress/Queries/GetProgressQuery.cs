using Application.Common.Interfaces.Progression;
using Application.Common.Result;
using Application.DTOs.Progression;
using Domain.Interfaces.User;
using MediatR;

namespace Application.Features.Progress.Queries;

public class GetProgressQuery : IRequest<Result<ProgressDto>>
{
    public Guid UserId { get; set; }
}

public class GetProgressQueryHandler : IRequestHandler<GetProgressQuery, Result<ProgressDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILevelService _levelService;

    public GetProgressQueryHandler(
        IUserRepository userRepository,
        ILevelService levelService)
    {
        _userRepository = userRepository;
        _levelService = levelService;
    }

    public async global::System.Threading.Tasks.Task<Result<ProgressDto>> Handle(GetProgressQuery request, System.Threading.CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            return Result.Failure<ProgressDto>("User not found");

        var dto = MapToDto(user);
        return Result.Success(dto);
    }

    private ProgressDto MapToDto(Domain.Entities.User.User user)
    {
        return new ProgressDto
        {
            Level = user.Level,
            TotalXP = user.XP,
            CurrentHP = user.CurrentHP,
            MaxHP = user.MaxHP,
            CurrentStreak = user.CurrentStreak,
            ShieldCount = user.ShieldCount,
            XPProgressToNextLevel = _levelService.GetXPProgressToNextLevel(user.XP),
            XPNeededForNextLevel = _levelService.GetXPNeededForNextLevel(user.XP)
        };
    }
}
