using global::System.Threading.Tasks;
using NotificationEntity = Domain.Entities.Notification.Notification;

namespace Domain.Interfaces.Notification;

public interface INotificationRepository
{
    global::System.Threading.Tasks.Task<NotificationEntity> GetByIdAsync(Guid id);
    global::System.Threading.Tasks.Task<List<NotificationEntity>> GetByUserIdAsync(Guid userId);
    global::System.Threading.Tasks.Task<List<NotificationEntity>> GetUpcomingAsync(Guid userId, DateTime from, DateTime to);
    global::System.Threading.Tasks.Task<NotificationEntity> AddAsync(NotificationEntity notification);
    global::System.Threading.Tasks.Task UpdateAsync(NotificationEntity notification);
    global::System.Threading.Tasks.Task DeleteAsync(Guid id);
    global::System.Threading.Tasks.Task MarkAsReadAsync(Guid id);
}
