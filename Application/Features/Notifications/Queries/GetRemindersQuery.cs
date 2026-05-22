using Application.Common.Result;
using Application.DTOs.Notifications;
using Domain.Interfaces.Notification;
using MediatR;

namespace Application.Features.Notifications.Queries;

public class GetRemindersQuery : IRequest<Result<List<NotificationDto>>>
{
    public Guid UserId { get; set; }
}

public class GetRemindersQueryHandler : IRequestHandler<GetRemindersQuery, Result<List<NotificationDto>>>
{
    private readonly INotificationRepository _notificationRepository;

    public GetRemindersQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async global::System.Threading.Tasks.Task<Result<List<NotificationDto>>> Handle(GetRemindersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var notifications = await _notificationRepository.GetByUserIdAsync(request.UserId);

            var dtos = notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                Type = n.Type.ToString(),
                Title = n.Title,
                Message = n.Message,
                TaskId = n.TaskId,
                JournalId = n.JournalId,
                CreatedAt = n.CreatedAt,
                ScheduledTime = n.ScheduledTime,
                IsRead = n.IsRead
            }).ToList();

            return Result.Success(dtos);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<NotificationDto>>(ex.Message);
        }
    }
}
