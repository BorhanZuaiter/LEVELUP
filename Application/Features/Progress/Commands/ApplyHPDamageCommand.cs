using Application.Common.Interfaces.Progression;
using Application.Common.Result;
using Application.DTOs.Progression;
using Domain.Enums;
using Domain.Interfaces.User;
using MediatR;

namespace Application.Features.Progress.Commands;

public class ApplyHPDamageCommand : IRequest<Result<ProgressDto>>
{
    public Guid UserId { get; set; }
    public Difficulty Difficulty { get; set; }
}

public class ApplyHPDamageCommandHandler : IRequestHandler<ApplyHPDamageCommand, Result<ProgressDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IHPService _hpService;
    private readonly ILevelService _levelService;

    public ApplyHPDamageCommandHandler(
        IUserRepository userRepository,
        IHPService hpService,
        ILevelService levelService)
    {
        _userRepository = userRepository;
        _hpService = hpService;
        _levelService = levelService;
    }

    public async global::System.Threading.Tasks.Task<Result<ProgressDto>> Handle(ApplyHPDamageCommand request, System.Threading.CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            return Result.Failure<ProgressDto>("User not found");

        user.CurrentHP = _hpService.ApplyHPDamage(user.CurrentHP, request.Difficulty);
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);

        var dto = MapToDto(user);
        return Result.Success(dto, "HP damage applied");
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
