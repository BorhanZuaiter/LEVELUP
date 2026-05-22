namespace Domain.Interfaces.Journal;

public interface IJournalRepository
{
    global::System.Threading.Tasks.Task<Domain.Entities.Journal.Journal?> GetByIdAsync(Guid id);
    global::System.Threading.Tasks.Task<List<Domain.Entities.Journal.Journal>> GetByUserIdAsync(Guid userId);
    global::System.Threading.Tasks.Task<List<Domain.Entities.Journal.Journal>> GetByDateAsync(Guid userId, DateTime date);
    global::System.Threading.Tasks.Task<List<Domain.Entities.Journal.Journal>> GetTimelineAsync(Guid userId);
    global::System.Threading.Tasks.Task<List<Domain.Entities.Journal.Journal>> SearchAsync(Guid userId, string title);
    global::System.Threading.Tasks.Task<List<Domain.Entities.Journal.Journal>> GetOnThisDayAsync(Guid userId, int month, int day);
    global::System.Threading.Tasks.Task AddAsync(Domain.Entities.Journal.Journal journal);
    global::System.Threading.Tasks.Task UpdateAsync(Domain.Entities.Journal.Journal journal);
    global::System.Threading.Tasks.Task DeleteAsync(Guid id);
}
