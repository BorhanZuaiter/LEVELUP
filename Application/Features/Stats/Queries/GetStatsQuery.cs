using Application.Common.Result;
using Application.DTOs.Stats;
using Domain.Interfaces.Stats;
using MediatR;

namespace Application.Features.Stats.Queries;

public class GetStatsQuery : IRequest<Result<StatsDto>>
{
    public Guid UserId { get; set; }
}

public class GetStatsQueryHandler : IRequestHandler<GetStatsQuery, Result<StatsDto>>
{
    private readonly IStatsRepository _statsRepository;

    public GetStatsQueryHandler(IStatsRepository statsRepository)
    {
        _statsRepository = statsRepository;
    }

    public async global::System.Threading.Tasks.Task<Result<StatsDto>> Handle(GetStatsQuery request, System.Threading.CancellationToken cancellationToken)
    {
        var stats = await _statsRepository.GetByUserIdAsync(request.UserId);
        if (stats == null)
        {
            var defaultStats = new Domain.Entities.Stats.Stats
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                UpdatedAt = DateTime.UtcNow
            };
            return Result.Success(MapToDto(defaultStats));
        }

        var dto = MapToDto(stats);
        return Result.Success(dto);
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
