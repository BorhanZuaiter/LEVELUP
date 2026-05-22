using Domain.Entities.Notification;
using Domain.Interfaces.Notification;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly LevelUpDbContext _context;

    public NotificationRepository(LevelUpDbContext context)
    {
        _context = context;
    }

    public async global::System.Threading.Tasks.Task<Notification> GetByIdAsync(Guid id)
    {
        var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
        return notification ?? throw new InvalidOperationException($"Notification with ID {id} not found");
    }

    public async global::System.Threading.Tasks.Task<List<Notification>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.ScheduledTime)
            .ToListAsync();
    }

    public async global::System.Threading.Tasks.Task<List<Notification>> GetUpcomingAsync(Guid userId, DateTime from, DateTime to)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId && n.ScheduledTime >= from && n.ScheduledTime <= to)
            .OrderBy(n => n.ScheduledTime)
            .ToListAsync();
    }

    public async global::System.Threading.Tasks.Task<Notification> AddAsync(Notification notification)
    {
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
        return notification;
    }

    public async global::System.Threading.Tasks.Task UpdateAsync(Notification notification)
    {
        _context.Notifications.Update(notification);
        await _context.SaveChangesAsync();
    }

    public async global::System.Threading.Tasks.Task DeleteAsync(Guid id)
    {
        var notification = await GetByIdAsync(id);
        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync();
    }

    public async global::System.Threading.Tasks.Task MarkAsReadAsync(Guid id)
    {
        var notification = await GetByIdAsync(id);
        notification.IsRead = true;
        await UpdateAsync(notification);
    }
}
