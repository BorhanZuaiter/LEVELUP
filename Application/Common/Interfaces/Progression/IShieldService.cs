namespace Application.Common.Interfaces.Progression;

public interface IShieldService
{
    int CalculateShieldsEarned(int currentStreak);
    int ConsumeShield(int currentShields);
}
