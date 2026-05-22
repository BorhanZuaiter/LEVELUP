using Domain.Enums;

namespace Domain.Entities.Task;

public class RecurrenceRule
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public RecurrenceType RecurrenceType { get; set; }
    public int Interval { get; set; } = 1;
    public string? WeekDays { get; set; }

    public Task Task { get; set; } = null!;
}
