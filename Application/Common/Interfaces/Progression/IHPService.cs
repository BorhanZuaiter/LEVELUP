using Domain.Enums;

namespace Application.Common.Interfaces.Progression;

public interface IHPService
{
    int RecoverHP(int currentHP, int maxHP, Difficulty difficulty);
    int ApplyHPDamage(int currentHP, Difficulty difficulty);
}
