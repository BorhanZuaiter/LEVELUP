namespace Application.DTOs.Auth;

public class UpdateProfileRequest
{
    public string Username { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
}
