namespace Application.Common.Interfaces;

public interface IAvatarService
{
    Task<string> SaveAvatarAsync(Stream fileStream, string fileName, string userId);
    Task DeleteAvatarAsync(string avatarPath);
    bool IsValidAvatarFile(string fileName, long fileSize);
}
