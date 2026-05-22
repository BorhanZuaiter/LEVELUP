using Domain.Enums;

namespace Application.Common.Interfaces.Stats;

public interface IStatsService
{
    int CalculateStatGain(Difficulty difficulty);
    bool IsStatCapReached(int currentDailyStatGain);
}
