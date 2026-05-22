using Application.Common.Result;
using Domain.Interfaces.Notification;
using MediatR;

namespace Application.Features.Notifications.Commands;

public class DeleteReminderCommand : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}

public class DeleteReminderCommandHandler : IRequestHandler<DeleteReminderCommand, Result<Unit>>
{
    private readonly INotificationRepository _notificationRepository;

    public DeleteReminderCommandHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async global::System.Threading.Tasks.Task<Result<Unit>> Handle(DeleteReminderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var notification = await _notificationRepository.GetByIdAsync(request.Id);

            if (notification.UserId != request.UserId)
            {
                return Result.Failure<Unit>("Unauthorized");
            }

            await _notificationRepository.DeleteAsync(request.Id);
            return Result.Success(Unit.Value);
        }
        catch (Exception ex)
        {
            return Result.Failure<Unit>(ex.Message);
        }
    }
}
