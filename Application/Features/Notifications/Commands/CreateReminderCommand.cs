using Application.Common.Result;
using Application.DTOs.Notifications;
using Domain.Interfaces.Notification;
using MediatR;

namespace Application.Features.Notifications.Commands;

public class CreateReminderCommand : IRequest<Result<NotificationDto>>
{
    public Guid UserId { get; set; }
    public Guid TaskId { get; set; }
    public TimeSpan ReminderTime { get; set; }
}

public class CreateReminderCommandHandler : IRequestHandler<CreateReminderCommand, Result<NotificationDto>>
{
    private readonly INotificationRepository _notificationRepository;

    public CreateReminderCommandHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async global::System.Threading.Tasks.Task<Result<NotificationDto>> Handle(CreateReminderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var scheduledTime = DateTime.Now.Date.Add(request.ReminderTime);
            if (scheduledTime < DateTime.Now)
            {
                scheduledTime = scheduledTime.AddDays(1);
            }

            var notification = new Domain.Entities.Notification.Notification
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Type = Domain.Entities.Notification.NotificationType.TaskReminder,
                Title = "Task Reminder",
                Message = "You have a task coming up.",
                TaskId = request.TaskId,
                CreatedAt = DateTime.Now,
                ScheduledTime = scheduledTime
            };

            var created = await _notificationRepository.AddAsync(notification);

            var dto = new NotificationDto
            {
                Id = created.Id,
                Type = created.Type.ToString(),
                Title = created.Title,
                Message = created.Message,
                TaskId = created.TaskId,
                JournalId = created.JournalId,
                CreatedAt = created.CreatedAt,
                ScheduledTime = created.ScheduledTime,
                IsRead = created.IsRead
            };

            return Result.Success(dto);
        }
        catch (Exception ex)
        {
            return Result.Failure<NotificationDto>(ex.Message);
        }
    }
}
