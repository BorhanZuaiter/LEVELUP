namespace Application.DTOs.Tasks;

public class CreateTaskRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Difficulty { get; set; }
    public int Importance { get; set; }
    public int Priority { get; set; } = 1;
    public int DurationMinutes { get; set; }
    public string TaskColor { get; set; } = string.Empty;
    public int StatCategory { get; set; }
    public int RecurrenceType { get; set; }
    public bool IsRequired { get; set; } = false;
}
