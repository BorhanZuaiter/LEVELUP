using Application.Common.Result;
using Application.DTOs.Stats;
using Domain.Enums;
using Domain.Interfaces.Task;
using MediatR;

namespace Application.Features.Stats.Queries;

public class GetMonthlyStatsQuery : IRequest<Result<MonthlyStatsDto>>
{
    public Guid UserId { get; set; }
}

public class GetMonthlyStatsQueryHandler : IRequestHandler<GetMonthlyStatsQuery, Result<MonthlyStatsDto>>
{
    private readonly ITaskRepository _taskRepository;

    public GetMonthlyStatsQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async global::System.Threading.Tasks.Task<Result<MonthlyStatsDto>> Handle(GetMonthlyStatsQuery request, System.Threading.CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository.GetByUserIdAsync(request.UserId);

        var currentMonth = DateTime.UtcNow;
        var completedThisMonth = tasks
            .Where(t => t.TaskStatus == Domain.Enums.TaskStatus.Completed &&
                   t.CompletedAt.HasValue &&
                   t.CompletedAt.Value.Year == currentMonth.Year &&
                   t.CompletedAt.Value.Month == currentMonth.Month)
            .ToList();

        int powerGained = 0;
        int wisdomGained = 0;
        int determinationGained = 0;

        foreach (var task in completedThisMonth)
        {
            int gain = task.Difficulty switch
            {
                Difficulty.Easy => 1,
                Difficulty.Medium => 2,
                Difficulty.Hard => 3,
                _ => 1
            };

            switch (task.StatCategory)
            {
                case StatCategory.Power:
                    powerGained += gain;
                    break;
                case StatCategory.Wisdom:
                    wisdomGained += gain;
                    break;
                case StatCategory.Determination:
                    determinationGained += gain;
                    break;
            }
        }

        var dto = new MonthlyStatsDto
        {
            PowerGained = powerGained,
            WisdomGained = wisdomGained,
            DeterminationGained = determinationGained,
            TasksCompleted = completedThisMonth.Count
        };

        return Result.Success(dto);
    }
}
