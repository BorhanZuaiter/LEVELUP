using Application.Common.Interfaces.Progression;
using Application.Common.Result;
using Application.DTOs.Progression;
using Domain.Interfaces.User;
using MediatR;

namespace Application.Features.Progress.Commands;

public class UpdateStreakCommand : IRequest<Result<ProgressDto>>
{
    public Guid UserId { get; set; }
    public bool AllRequiredTasksCompleted { get; set; }
}

public class UpdateStreakCommandHandler : IRequestHandler<UpdateStreakCommand, Result<ProgressDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IStreakService _streakService;
    private readonly IShieldService _shieldService;
    private readonly ILevelService _levelService;

    public UpdateStreakCommandHandler(
        IUserRepository userRepository,
        IStreakService streakService,
        IShieldService shieldService,
        ILevelService levelService)
    {
        _userRepository = userRepository;
        _streakService = streakService;
        _shieldService = shieldService;
        _levelService = levelService;
    }

    public async global::System.Threading.Tasks.Task<Result<ProgressDto>> Handle(UpdateStreakCommand request, System.Threading.CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            return Result.Failure<ProgressDto>("User not found");

        if (request.AllRequiredTasksCompleted)
        {
            user.CurrentStreak = _streakService.IncreaseStreak(user.CurrentStreak);
            user.ShieldCount = _shieldService.CalculateShieldsEarned(user.CurrentStreak);
        }
        else
        {
            user.CurrentStreak = _streakService.BreakStreak();
        }

        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        var dto = MapToDto(user);
        return Result.Success(dto, "Streak updated");
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
