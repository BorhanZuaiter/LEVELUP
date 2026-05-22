namespace Application.Common.Interfaces.Progression;

public interface ILevelService
{
    int GetCurrentLevel(int totalXP);
    int GetXPForLevel(int level);
    int GetXPProgressToNextLevel(int totalXP);
    int GetXPNeededForNextLevel(int totalXP);
    bool ShouldLevelUp(int oldLevel, int newLevel);
}
