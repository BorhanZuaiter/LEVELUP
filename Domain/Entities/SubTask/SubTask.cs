namespace Domain.Entities.SubTask;

public class SubTask
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }

    public Task.Task Task { get; set; } = null!;
}
