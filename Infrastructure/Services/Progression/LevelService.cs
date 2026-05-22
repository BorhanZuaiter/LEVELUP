using Application.Common.Interfaces.Progression;

namespace Infrastructure.Services.Progression;

public class LevelService : ILevelService
{
    private const int BaseXPForLevel1 = 100;

    public int GetCurrentLevel(int totalXP)
    {
        int level = 1;
        int cumulativeXP = 0;

        while (true)
        {
            int xpForNextLevel = GetXPForLevel(level);
            if (cumulativeXP + xpForNextLevel > totalXP)
            {
                break;
            }

            cumulativeXP += xpForNextLevel;
            level++;
        }

        return level;
    }

    public int GetXPForLevel(int level)
    {
        if (level <= 1) return BaseXPForLevel1;

        double exponent = (level - 1) * 0.5;
        return (int)(BaseXPForLevel1 * Math.Pow(1.5, exponent));
    }

    public int GetXPProgressToNextLevel(int totalXP)
    {
        int currentLevel = GetCurrentLevel(totalXP);
        int xpForCurrentLevel = GetCumulativeXPForLevel(currentLevel);

        return totalXP - xpForCurrentLevel;
    }

    public int GetXPNeededForNextLevel(int totalXP)
    {
        int currentLevel = GetCurrentLevel(totalXP);
        return GetXPForLevel(currentLevel + 1);
    }

    public bool ShouldLevelUp(int oldLevel, int newLevel)
    {
        return newLevel > oldLevel;
    }

    private int GetCumulativeXPForLevel(int level)
    {
        int cumulative = 0;
        for (int i = 1; i < level; i++)
        {
            cumulative += GetXPForLevel(i);
        }

        return cumulative;
    }
}
