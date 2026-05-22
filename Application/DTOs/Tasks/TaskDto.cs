namespace Application.DTOs.Tasks;

public class TaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Difficulty { get; set; }
    public int Importance { get; set; }
    public int Priority { get; set; }
    public int DurationMinutes { get; set; }
    public string TaskColor { get; set; } = string.Empty;
    public int StatCategory { get; set; }
    public int RecurrenceType { get; set; }
    public int TaskStatus { get; set; }
    public bool IsRequired { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
