using Application.Common.Interfaces.Stats;
using Domain.Enums;

namespace Infrastructure.Services.Stats;

public class StatsService : IStatsService
{
    private const int DailyStatCap = 20;

    public int CalculateStatGain(Difficulty difficulty)
    {
        return difficulty switch
        {
            Difficulty.Easy => 1,
            Difficulty.Medium => 2,
            Difficulty.Hard => 3,
            _ => 1
        };
    }

    public bool IsStatCapReached(int currentDailyStatGain)
    {
        return currentDailyStatGain >= DailyStatCap;
    }
}
