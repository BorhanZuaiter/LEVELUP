using Domain.Enums;

namespace Application.Common.Interfaces.Progression;

public interface IXPService
{
    int CalculateXP(Difficulty difficulty, int priority, int durationMinutes, bool isRequired);
    int ApplyAntiFarmCoefficient(int baseXP, int completionCount);
    bool IsXPCapReached(int currentDailyXP);
}
