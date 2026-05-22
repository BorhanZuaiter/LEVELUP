namespace Application.DTOs.Progression;

public class ProgressDto
{
    public int Level { get; set; }
    public int TotalXP { get; set; }
    public int CurrentHP { get; set; }
    public int MaxHP { get; set; }
    public int CurrentStreak { get; set; }
    public int ShieldCount { get; set; }
    public int XPProgressToNextLevel { get; set; }
    public int XPNeededForNextLevel { get; set; }
}
