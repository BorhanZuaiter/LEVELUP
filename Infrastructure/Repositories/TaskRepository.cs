using Domain.Entities.Task;
using Domain.Enums;
using Domain.Interfaces.Task;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly LevelUpDbContext _context;

    public TaskRepository(LevelUpDbContext context)
    {
        _context = context;
    }

    public async global::System.Threading.Tasks.Task<Domain.Entities.Task.Task?> GetByIdAsync(Guid taskId)
    {
        return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
    }

    public async global::System.Threading.Tasks.Task<List<Domain.Entities.Task.Task>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Tasks
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }

    public async global::System.Threading.Tasks.Task<List<Domain.Entities.Task.Task>> GetByStatusAsync(Guid userId, Domain.Enums.TaskStatus status)
    {
        return await _context.Tasks
            .Where(t => t.UserId == userId && t.TaskStatus == status)
            .ToListAsync();
    }

    public async global::System.Threading.Tasks.Task<List<Domain.Entities.Task.Task>> GetByColorAsync(Guid userId, int color)
    {
        return await _context.Tasks
            .Where(t => t.UserId == userId && t.TaskColor == color.ToString())
            .ToListAsync();
    }

    public async global::System.Threading.Tasks.Task<List<Domain.Entities.Task.Task>> GetRequiredTasksAsync(Guid userId)
    {
        return await _context.Tasks
            .Where(t => t.UserId == userId && t.IsRequired)
            .ToListAsync();
    }

    public async global::System.Threading.Tasks.Task<List<Domain.Entities.Task.Task>> SearchAsync(Guid userId, string query)
    {
        var lowerQuery = query.ToLower();
        return await _context.Tasks
            .Where(t => t.UserId == userId &&
                   (t.Title.ToLower().Contains(lowerQuery) ||
                    t.Description.ToLower().Contains(lowerQuery)))
            .ToListAsync();
    }

    public global::System.Threading.Tasks.Task<List<Domain.Entities.Task.Task>> FilterAsync(IEnumerable<Domain.Entities.Task.Task> tasks, TaskFilterCriteria criteria)
    {
        var filtered = tasks.AsQueryable();

        if (criteria.Color.HasValue)
            filtered = filtered.Where(t => t.TaskColor == criteria.Color.ToString());

        if (criteria.StatCategory.HasValue)
            filtered = filtered.Where(t => (int)t.StatCategory == criteria.StatCategory);

        if (criteria.Status.HasValue)
            filtered = filtered.Where(t => (int)t.TaskStatus == criteria.Status);

        if (criteria.IsRequired.HasValue)
            filtered = filtered.Where(t => t.IsRequired == criteria.IsRequired);

        if (criteria.Priority.HasValue)
            filtered = filtered.Where(t => (int)t.Priority == criteria.Priority);

        return global::System.Threading.Tasks.Task.FromResult(filtered.ToList());
    }

    public async global::System.Threading.Tasks.Task AddAsync(Domain.Entities.Task.Task task)
    {
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
    }

    public async global::System.Threading.Tasks.Task UpdateAsync(Domain.Entities.Task.Task task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }

    public async global::System.Threading.Tasks.Task DeleteAsync(Guid taskId)
    {
        var task = await GetByIdAsync(taskId);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}
