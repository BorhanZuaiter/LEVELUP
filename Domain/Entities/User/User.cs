namespace Domain.Entities.User;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public DateTime JoinDate { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool EmailVerified { get; set; }
    public string? EmailVerificationToken { get; set; }
    public string? ForgotPasswordToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
    public int Level { get; set; } = 1;
    public int XP { get; set; } = 0;
    public int CurrentHP { get; set; } = 100;
    public int MaxHP { get; set; } = 100;
    public int CurrentStreak { get; set; } = 0;
    public int ShieldCount { get; set; } = 0;

    public ICollection<Task.Task> Tasks { get; set; } = new List<Task.Task>();
    public ICollection<Journal.Journal> Journals { get; set; } = new List<Journal.Journal>();
    public Stats.Stats? Stats { get; set; }
    public ICollection<Streak.StreakHistory> StreakHistories { get; set; } = new List<Streak.StreakHistory>();
}
