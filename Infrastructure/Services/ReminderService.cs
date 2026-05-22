using Domain.Entities.Notification;
using Domain.Interfaces.Notification;

namespace Infrastructure.Services;

public interface IReminderService
{
    global::System.Threading.Tasks.Task CreateTaskReminderAsync(Guid userId, Guid taskId, string taskTitle, TimeSpan reminderTime);
    global::System.Threading.Tasks.Task CreateJournalReminderAsync(Guid userId, TimeSpan fixedReminderTime);
    global::System.Threading.Tasks.Task CreateOverdueNotificationAsync(Guid userId, Guid taskId, string taskTitle);
    global::System.Threading.Tasks.Task CreateMissedTaskNotificationAsync(Guid userId, Guid taskId, string taskTitle);
    global::System.Threading.Tasks.Task UpdateReminderAsync(Guid notificationId, TimeSpan newReminderTime);
    global::System.Threading.Tasks.Task DeleteReminderAsync(Guid notificationId);
}

public class ReminderService : IReminderService
{
    private readonly INotificationRepository _notificationRepository;

    public ReminderService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async global::System.Threading.Tasks.Task CreateTaskReminderAsync(Guid userId, Guid taskId, string taskTitle, TimeSpan reminderTime)
    {
        var scheduledTime = DateTime.Now.Date.Add(reminderTime);
        if (scheduledTime < DateTime.Now)
        {
            scheduledTime = scheduledTime.AddDays(1);
        }

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = NotificationType.TaskReminder,
            Title = "Task Reminder",
            Message = $"Reminder: {taskTitle}",
            TaskId = taskId,
            CreatedAt = DateTime.Now,
            ScheduledTime = scheduledTime
        };

        await _notificationRepository.AddAsync(notification);
    }

    public async global::System.Threading.Tasks.Task CreateJournalReminderAsync(Guid userId, TimeSpan fixedReminderTime)
    {
        var scheduledTime = DateTime.Now.Date.Add(fixedReminderTime);
        if (scheduledTime < DateTime.Now)
        {
            scheduledTime = scheduledTime.AddDays(1);
        }

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = NotificationType.JournalReminder,
            Title = "Journal Reminder",
            Message = "Time to journal! Reflect on your day.",
            CreatedAt = DateTime.Now,
            ScheduledTime = scheduledTime
        };

        await _notificationRepository.AddAsync(notification);
    }

    public async global::System.Threading.Tasks.Task CreateOverdueNotificationAsync(Guid userId, Guid taskId, string taskTitle)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = NotificationType.OverdueTask,
            Title = "Task Overdue",
            Message = $"{taskTitle} is now overdue.",
            TaskId = taskId,
            CreatedAt = DateTime.Now,
            ScheduledTime = DateTime.Now
        };

        await _notificationRepository.AddAsync(notification);
    }

    public async global::System.Threading.Tasks.Task CreateMissedTaskNotificationAsync(Guid userId, Guid taskId, string taskTitle)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = NotificationType.MissedTask,
            Title = "Task Missed",
            Message = $"{taskTitle} was missed. HP decreased.",
            TaskId = taskId,
            CreatedAt = DateTime.Now,
            ScheduledTime = DateTime.Now
        };

        await _notificationRepository.AddAsync(notification);
    }

    public async global::System.Threading.Tasks.Task UpdateReminderAsync(Guid notificationId, TimeSpan newReminderTime)
    {
        var notification = await _notificationRepository.GetByIdAsync(notificationId);
        var scheduledTime = DateTime.Now.Date.Add(newReminderTime);
        if (scheduledTime < DateTime.Now)
        {
            scheduledTime = scheduledTime.AddDays(1);
        }
        notification.ScheduledTime = scheduledTime;
        await _notificationRepository.UpdateAsync(notification);
    }

    public async global::System.Threading.Tasks.Task DeleteReminderAsync(Guid notificationId)
    {
        await _notificationRepository.DeleteAsync(notificationId);
    }
}
