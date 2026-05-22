using Application.Common.Interfaces.Progression;
using Domain.Enums;

namespace Infrastructure.Services.Progression;

public class HPService : IHPService
{
    private const int MaxHP = 100;
    private const int MinHP = 0;

    public int RecoverHP(int currentHP, int maxHP, Difficulty difficulty)
    {
        int recovery = GetHPRecoveryForDifficulty(difficulty);
        int newHP = currentHP + recovery;

        return Math.Min(newHP, maxHP);
    }

    public int ApplyHPDamage(int currentHP, Difficulty difficulty)
    {
        int damage = GetHPDamageForDifficulty(difficulty);
        int newHP = currentHP - damage;

        return Math.Max(newHP, MinHP);
    }

    private int GetHPRecoveryForDifficulty(Difficulty difficulty)
    {
        return difficulty switch
        {
            Difficulty.Easy => 2,
            Difficulty.Medium => 5,
            Difficulty.Hard => 8,
            _ => 2
        };
    }

    private int GetHPDamageForDifficulty(Difficulty difficulty)
    {
        return difficulty switch
        {
            Difficulty.Easy => 2,
            Difficulty.Medium => 5,
            Difficulty.Hard => 8,
            _ => 2
        };
    }
}
