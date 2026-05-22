using Application.Common.Interfaces.Progression;
using Application.Common.Result;
using Application.DTOs.Progression;
using Domain.Enums;
using Domain.Interfaces.User;
using MediatR;

namespace Application.Features.Progress.Commands;

public class GainXPCommand : IRequest<Result<ProgressDto>>
{
    public Guid UserId { get; set; }
    public Difficulty Difficulty { get; set; }
    public int Priority { get; set; }
    public int DurationMinutes { get; set; }
    public bool IsRequired { get; set; }
    public int CompletionCount { get; set; }
}

public class GainXPCommandHandler : IRequestHandler<GainXPCommand, Result<ProgressDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IXPService _xpService;
    private readonly ILevelService _levelService;
    private readonly IHPService _hpService;

    public GainXPCommandHandler(
        IUserRepository userRepository,
        IXPService xpService,
        ILevelService levelService,
        IHPService hpService)
    {
        _userRepository = userRepository;
        _xpService = xpService;
        _levelService = levelService;
        _hpService = hpService;
    }

    public async global::System.Threading.Tasks.Task<Result<ProgressDto>> Handle(GainXPCommand request, System.Threading.CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            return Result.Failure<ProgressDto>("User not found");

        int baseXP = _xpService.CalculateXP(request.Difficulty, request.Priority, request.DurationMinutes, request.IsRequired);
        int adjustedXP = _xpService.ApplyAntiFarmCoefficient(baseXP, request.CompletionCount);

        if (_xpService.IsXPCapReached(user.XP))
            adjustedXP = 0;

        int oldLevel = _levelService.GetCurrentLevel(user.XP);
        user.XP += adjustedXP;
        int newLevel = _levelService.GetCurrentLevel(user.XP);

        user.Level = newLevel;
        user.CurrentHP = _hpService.RecoverHP(user.CurrentHP, user.MaxHP, request.Difficulty);
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);

        var dto = MapToDto(user);
        return Result.Success(dto, "XP gained successfully");
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
