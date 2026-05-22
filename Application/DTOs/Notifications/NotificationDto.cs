using Domain.Entities.Notification;

namespace Application.DTOs.Notifications;

public class NotificationDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public Guid? TaskId { get; set; }
    public Guid? JournalId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ScheduledTime { get; set; }
    public bool IsRead { get; set; }
}

public class CreateReminderRequest
{
    public Guid TaskId { get; set; }
    public TimeSpan ReminderTime { get; set; }
}

public class UpdateReminderRequest
{
    public TimeSpan ReminderTime { get; set; }
}
