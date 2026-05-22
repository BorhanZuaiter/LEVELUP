using Domain.Interfaces.Journal;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class JournalRepository : IJournalRepository
{
    private readonly LevelUpDbContext _context;

    public JournalRepository(LevelUpDbContext context)
    {
        _context = context;
    }

    public async global::System.Threading.Tasks.Task<Domain.Entities.Journal.Journal?> GetByIdAsync(Guid id)
    {
        return await _context.Journals.FirstOrDefaultAsync(j => j.Id == id);
    }

    public async global::System.Threading.Tasks.Task<List<Domain.Entities.Journal.Journal>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Journals
            .Where(j => j.UserId == userId)
            .ToListAsync();
    }

    public async global::System.Threading.Tasks.Task<List<Domain.Entities.Journal.Journal>> GetByDateAsync(Guid userId, DateTime date)
    {
        var startDate = date.Date;
        var endDate = startDate.AddDays(1);

        return await _context.Journals
            .Where(j => j.UserId == userId &&
                   j.Date >= startDate &&
                   j.Date < endDate)
            .ToListAsync();
    }

    public async global::System.Threading.Tasks.Task<List<Domain.Entities.Journal.Journal>> GetTimelineAsync(Guid userId)
    {
        return await _context.Journals
            .Where(j => j.UserId == userId)
            .OrderByDescending(j => j.Date)
            .ThenByDescending(j => j.CreatedAt)
            .ToListAsync();
    }

    public async global::System.Threading.Tasks.Task<List<Domain.Entities.Journal.Journal>> SearchAsync(Guid userId, string title)
    {
        var lowerTitle = title.ToLower();
        return await _context.Journals
            .Where(j => j.UserId == userId &&
                   j.Title.ToLower().Contains(lowerTitle))
            .OrderByDescending(j => j.Date)
            .ToListAsync();
    }

    public async global::System.Threading.Tasks.Task<List<Domain.Entities.Journal.Journal>> GetOnThisDayAsync(Guid userId, int month, int day)
    {
        return await _context.Journals
            .Where(j => j.UserId == userId &&
                   j.Date.Month == month &&
                   j.Date.Day == day)
            .OrderByDescending(j => j.Date)
            .ToListAsync();
    }

    public async global::System.Threading.Tasks.Task AddAsync(Domain.Entities.Journal.Journal journal)
    {
        await _context.Journals.AddAsync(journal);
        await _context.SaveChangesAsync();
    }

    public async global::System.Threading.Tasks.Task UpdateAsync(Domain.Entities.Journal.Journal journal)
    {
        _context.Journals.Update(journal);
        await _context.SaveChangesAsync();
    }

    public async global::System.Threading.Tasks.Task DeleteAsync(Guid id)
    {
        var journal = await GetByIdAsync(id);
        if (journal != null)
        {
            _context.Journals.Remove(journal);
            await _context.SaveChangesAsync();
        }
    }
}
