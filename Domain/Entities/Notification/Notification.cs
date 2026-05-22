namespace Domain.Entities.Notification;

public enum NotificationType
{
    TaskReminder,
    JournalReminder,
    OverdueTask,
    MissedTask
}

public class Notification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public Guid? TaskId { get; set; }
    public Guid? JournalId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ScheduledTime { get; set; }
    public bool IsRead { get; set; } = false;

    public User.User User { get; set; } = null!;
}
