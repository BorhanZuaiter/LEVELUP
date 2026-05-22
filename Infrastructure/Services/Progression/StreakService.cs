using Application.Common.Interfaces.Progression;

namespace Infrastructure.Services.Progression;

public class StreakService : IStreakService
{
    public bool ShouldIncreaseStreak(bool allRequiredTasksCompleted)
    {
        return allRequiredTasksCompleted;
    }

    public int BreakStreak()
    {
        return 0;
    }

    public int IncreaseStreak(int currentStreak)
    {
        return currentStreak + 1;
    }
}
