namespace Application.DTOs.Auth;

public class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public UserInfoDto User { get; set; } = null!;
}

public class UserInfoDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public int Level { get; set; }
    public int XP { get; set; }
    public int CurrentHP { get; set; }
    public int MaxHP { get; set; }
    public int CurrentStreak { get; set; }
    public int ShieldCount { get; set; }
}
