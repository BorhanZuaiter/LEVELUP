using Domain.Interfaces.Stats;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class StatsRepository : IStatsRepository
{
    private readonly LevelUpDbContext _context;

    public StatsRepository(LevelUpDbContext context)
    {
        _context = context;
    }

    public async global::System.Threading.Tasks.Task<Domain.Entities.Stats.Stats?> GetByUserIdAsync(Guid userId)
    {
        return await _context.Stats.FirstOrDefaultAsync(s => s.UserId == userId);
    }

    public async global::System.Threading.Tasks.Task<Domain.Entities.Stats.Stats?> GetByIdAsync(Guid id)
    {
        return await _context.Stats.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async global::System.Threading.Tasks.Task AddAsync(Domain.Entities.Stats.Stats stats)
    {
        await _context.Stats.AddAsync(stats);
        await _context.SaveChangesAsync();
    }

    public async global::System.Threading.Tasks.Task UpdateAsync(Domain.Entities.Stats.Stats stats)
    {
        _context.Stats.Update(stats);
        await _context.SaveChangesAsync();
    }

    public async global::System.Threading.Tasks.Task DeleteAsync(Guid id)
    {
        var stats = await _context.Stats.FirstOrDefaultAsync(s => s.Id == id);
        if (stats != null)
        {
            _context.Stats.Remove(stats);
            await _context.SaveChangesAsync();
        }
    }
}
