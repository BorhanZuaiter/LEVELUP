using Application.Common.Result;
using Application.DTOs.Notifications;
using Domain.Interfaces.Notification;
using MediatR;

namespace Application.Features.Notifications.Commands;

public class UpdateReminderCommand : IRequest<Result<NotificationDto>>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public TimeSpan ReminderTime { get; set; }
}

public class UpdateReminderCommandHandler : IRequestHandler<UpdateReminderCommand, Result<NotificationDto>>
{
    private readonly INotificationRepository _notificationRepository;

    public UpdateReminderCommandHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async global::System.Threading.Tasks.Task<Result<NotificationDto>> Handle(UpdateReminderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var notification = await _notificationRepository.GetByIdAsync(request.Id);

            if (notification.UserId != request.UserId)
            {
                return Result.Failure<NotificationDto>("Unauthorized");
            }

            var scheduledTime = DateTime.Now.Date.Add(request.ReminderTime);
            if (scheduledTime < DateTime.Now)
            {
                scheduledTime = scheduledTime.AddDays(1);
            }

            notification.ScheduledTime = scheduledTime;
            await _notificationRepository.UpdateAsync(notification);

            var dto = new NotificationDto
            {
                Id = notification.Id,
                Type = notification.Type.ToString(),
                Title = notification.Title,
                Message = notification.Message,
                TaskId = notification.TaskId,
                JournalId = notification.JournalId,
                CreatedAt = notification.CreatedAt,
                ScheduledTime = notification.ScheduledTime,
                IsRead = notification.IsRead
            };

            return Result.Success(dto);
        }
        catch (Exception ex)
        {
            return Result.Failure<NotificationDto>(ex.Message);
        }
    }
}
