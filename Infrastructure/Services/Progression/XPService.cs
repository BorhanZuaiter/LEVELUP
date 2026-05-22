using Application.Common.Interfaces.Progression;
using Domain.Enums;

namespace Infrastructure.Services.Progression;

public class XPService : IXPService
{
    private const int DailyXPCap = 500;
    private const int MinDuration = 10;
    private const int MaxDuration = 240;
    private const int RequiredTaskBonus = 50;

    public int CalculateXP(Difficulty difficulty, int priority, int durationMinutes, bool isRequired)
    {
        int baseXP = GetDifficultyXP(difficulty);

        int clampedDuration = Math.Clamp(durationMinutes, MinDuration, MaxDuration);
        double durationMultiplier = clampedDuration / 30.0;

        int priorityBonus = GetPriorityBonus(priority);

        int totalXP = (int)(baseXP * durationMultiplier) + priorityBonus;

        if (isRequired)
        {
            totalXP += RequiredTaskBonus;
        }

        return totalXP;
    }

    public int ApplyAntiFarmCoefficient(int baseXP, int completionCount)
    {
        return completionCount switch
        {
            1 => baseXP,
            2 => (int)(baseXP * 0.8),
            3 => (int)(baseXP * 0.6),
            4 => (int)(baseXP * 0.4),
            _ => 0
        };
    }

    public bool IsXPCapReached(int currentDailyXP)
    {
        return currentDailyXP >= DailyXPCap;
    }

    private int GetDifficultyXP(Difficulty difficulty)
    {
        return difficulty switch
        {
            Difficulty.Easy => 10,
            Difficulty.Medium => 25,
            Difficulty.Hard => 50,
            _ => 10
        };
    }

    private int GetPriorityBonus(int priority)
    {
        return priority switch
        {
            0 => 0,
            1 => 5,
            2 => 10,
            _ => 0
        };
    }
}
