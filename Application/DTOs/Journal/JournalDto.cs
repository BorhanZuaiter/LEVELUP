namespace Application.DTOs.Journal;

public class JournalDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Category { get; set; }
    public string? SubCategory { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
