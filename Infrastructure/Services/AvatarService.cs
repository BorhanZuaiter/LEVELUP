using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class AvatarService : IAvatarService
{
    private readonly IConfiguration _configuration;
    private readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
    private const long MaxFileSize = 3 * 1024 * 1024; // 3 MB

    public AvatarService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<string> SaveAvatarAsync(Stream fileStream, string fileName, string userId)
    {
        var uploadsFolder = _configuration["Avatar:UploadFolder"] ?? "uploads/avatars";
        var extension = Path.GetExtension(fileName).ToLower();

        if (!AllowedExtensions.Contains(extension))
            throw new InvalidOperationException("Invalid file format");

        var uniqueFileName = $"{userId}_{DateTime.UtcNow.Ticks}{extension}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        // Ensure directory exists
        Directory.CreateDirectory(uploadsFolder);

        using (var fileToSave = new FileStream(filePath, FileMode.Create))
        {
            fileStream.CopyTo(fileToSave);
        }

        return Task.FromResult(filePath);
    }

    public Task DeleteAvatarAsync(string avatarPath)
    {
        if (File.Exists(avatarPath))
        {
            File.Delete(avatarPath);
        }

        return Task.CompletedTask;
    }

    public bool IsValidAvatarFile(string fileName, long fileSize)
    {
        var extension = Path.GetExtension(fileName).ToLower();
        return AllowedExtensions.Contains(extension) && fileSize <= MaxFileSize;
    }
}
