using Application.Common.Interfaces.Stats;
using Application.Common.Result;
using Application.DTOs.Stats;
using Domain.Enums;
using Domain.Interfaces.Stats;
using MediatR;

namespace Application.Features.Stats.Commands;

public class IncreaseStatCommand : IRequest<Result<StatsDto>>
{
    public Guid UserId { get; set; }
    public Difficulty Difficulty { get; set; }
    public StatCategory StatCategory { get; set; }
}

public class IncreaseStatCommandHandler : IRequestHandler<IncreaseStatCommand, Result<StatsDto>>
{
    private readonly IStatsRepository _statsRepository;
    private readonly IStatsService _statsService;

    public IncreaseStatCommandHandler(
        IStatsRepository statsRepository,
        IStatsService statsService)
    {
        _statsRepository = statsRepository;
        _statsService = statsService;
    }

    public async global::System.Threading.Tasks.Task<Result<StatsDto>> Handle(IncreaseStatCommand request, System.Threading.CancellationToken cancellationToken)
    {
        var stats = await _statsRepository.GetByUserIdAsync(request.UserId);
        if (stats == null)
        {
            stats = new Domain.Entities.Stats.Stats
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                UpdatedAt = DateTime.UtcNow
            };
            await _statsRepository.AddAsync(stats);
        }

        int statGain = _statsService.CalculateStatGain(request.Difficulty);

        int totalCurrentGain = stats.Power + stats.Wisdom + stats.Luck + stats.Determination;
        if (_statsService.IsStatCapReached(totalCurrentGain))
        {
            statGain = 0;
        }

        if (statGain > 0)
        {
            switch (request.StatCategory)
            {
                case StatCategory.Power:
                    stats.Power += statGain;
                    break;
                case StatCategory.Wisdom:
                    stats.Wisdom += statGain;
                    break;
                case StatCategory.Determination:
                    stats.Determination += statGain;
                    break;
                case StatCategory.Luck:
                    break;
            }
        }

        stats.UpdatedAt = DateTime.UtcNow;
        await _statsRepository.UpdateAsync(stats);

        var dto = MapToDto(stats);
        return Result.Success(dto, "Stat increased successfully");
    }

    private StatsDto MapToDto(Domain.Entities.Stats.Stats stats)
    {
        return new StatsDto
        {
            Power = stats.Power,
            Wisdom = stats.Wisdom,
            Luck = stats.Luck,
            Determination = stats.Determination
        };
    }
}
