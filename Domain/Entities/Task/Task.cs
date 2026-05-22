using Domain.Enums;
using Domain.Entities.SubTask;
using Priority = Domain.Enums.Priority;

namespace Domain.Entities.Task;

public class Task
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Difficulty Difficulty { get; set; }
    public Importance Importance { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
    public int DurationMinutes { get; set; }
    public string TaskColor { get; set; } = string.Empty;
    public StatCategory StatCategory { get; set; }
    public RecurrenceType RecurrenceType { get; set; }
    public Enums.TaskStatus TaskStatus { get; set; } = Enums.TaskStatus.Pending;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool IsRequired { get; set; } = false;
    public TimeSpan? ReminderTime { get; set; }
    public bool ReminderEnabled { get; set; } = false;

    public User.User User { get; set; } = null!;
    public ICollection<SubTask.SubTask> SubTasks { get; set; } = new List<SubTask.SubTask>();
    public ICollection<RecurrenceRule> RecurrenceRules { get; set; } = new List<RecurrenceRule>();
}
