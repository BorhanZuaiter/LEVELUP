namespace Domain.Entities.Stats;

public class Stats
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public int Power { get; set; } = 0;
    public int Wisdom { get; set; } = 0;
    public int Luck { get; set; } = 0;
    public int Determination { get; set; } = 0;
    public DateTime UpdatedAt { get; set; }

    public User.User User { get; set; } = null!;
}
