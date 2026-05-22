namespace Application.Common.Interfaces.Progression;

public interface IStreakService
{
    bool ShouldIncreaseStreak(bool allRequiredTasksCompleted);
    int BreakStreak();
    int IncreaseStreak(int currentStreak);
}
