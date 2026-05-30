namespace Domain.Interfaces.Stats;

public interface IStatsRepository
{
    global::System.Threading.Tasks.Task<Domain.Entities.Stats.Stats?> GetByUserIdAsync(Guid userId);
    global::System.Threading.Tasks.Task<Domain.Entities.Stats.Stats?> GetByIdAsync(Guid id);
    global::System.Threading.Tasks.Task AddAsync(Domain.Entities.Stats.Stats stats);
    global::System.Threading.Tasks.Task UpdateAsync(Domain.Entities.Stats.Stats stats);
    global::System.Threading.Tasks.Task DeleteAsync(Guid id);
}
