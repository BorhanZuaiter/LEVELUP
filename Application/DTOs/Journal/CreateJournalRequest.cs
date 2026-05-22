namespace Application.DTOs.Journal;

public class CreateJournalRequest
{
    public string Title { get; set; } = string.Empty;
    public int Category { get; set; }
    public string? SubCategory { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}
