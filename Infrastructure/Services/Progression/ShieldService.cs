using Application.Common.Interfaces.Progression;

namespace Infrastructure.Services.Progression;

public class ShieldService : IShieldService
{
    private const int StreakDaysPerShield = 100;

    public int CalculateShieldsEarned(int currentStreak)
    {
        return currentStreak / StreakDaysPerShield;
    }

    public int ConsumeShield(int currentShields)
    {
        return Math.Max(currentShields - 1, 0);
    }
}
