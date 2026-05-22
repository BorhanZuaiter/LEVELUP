using Domain.Enums;

namespace Domain.Entities.Journal;

public class Journal
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public JournalCategory Category { get; set; }
    public string? SubCategory { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public User.User User { get; set; } = null!;
}
