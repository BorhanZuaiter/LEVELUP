using Domain.Entities.Task;
using Domain.Enums;
using System.Threading.Tasks;

namespace Domain.Interfaces.Task;

public interface ITaskRepository
{
    global::System.Threading.Tasks.Task<Entities.Task.Task?> GetByIdAsync(Guid id);
    global::System.Threading.Tasks.Task<List<Entities.Task.Task>> GetByUserIdAsync(Guid userId);
    global::System.Threading.Tasks.Task<List<Entities.Task.Task>> GetByStatusAsync(Guid userId, Domain.Enums.TaskStatus status);
    global::System.Threading.Tasks.Task<List<Entities.Task.Task>> GetByColorAsync(Guid userId, int color);
    global::System.Threading.Tasks.Task<List<Entities.Task.Task>> GetRequiredTasksAsync(Guid userId);
    global::System.Threading.Tasks.Task<List<Entities.Task.Task>> SearchAsync(Guid userId, string query);
    global::System.Threading.Tasks.Task<List<Entities.Task.Task>> FilterAsync(IEnumerable<Entities.Task.Task> tasks, TaskFilterCriteria criteria);
    global::System.Threading.Tasks.Task AddAsync(Entities.Task.Task task);
    global::System.Threading.Tasks.Task UpdateAsync(Entities.Task.Task task);
    global::System.Threading.Tasks.Task DeleteAsync(Guid id);
}

public class TaskFilterCriteria
{
    public int? Color { get; set; }
    public int? StatCategory { get; set; }
    public int? Status { get; set; }
    public bool? IsRequired { get; set; }
    public int? Priority { get; set; }
}
