namespace Domain.Entities.Streak;

public class StreakHistory
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public int StreakCount { get; set; }
    public bool ShieldUsed { get; set; } = false;

    public User.User User { get; set; } = null!;
}
