using System.Security.Cryptography;
using Application.Common.Interfaces;

namespace Infrastructure.Services;

public class PasswordService : IPasswordService
{
    private const int SaltSize = 16;
    private const int HashSize = 20;
    private const int Iterations = 10000;

    public string HashPassword(string password)
    {
#pragma warning disable SYSLIB0060
        using var algorithm = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithmName.SHA256);
        var key = Convert.ToBase64String(algorithm.GetBytes(HashSize));
        var salt = Convert.ToBase64String(algorithm.Salt);
#pragma warning restore SYSLIB0060

        return $"{Iterations}.{salt}.{key}";
    }

    public bool VerifyPassword(string password, string hash)
    {
        var parts = hash.Split('.', 3);
        if (parts.Length != 3)
            return false;

        if (!int.TryParse(parts[0], out var iterations))
            return false;

        var salt = Convert.FromBase64String(parts[1]);
        var key = Convert.FromBase64String(parts[2]);

#pragma warning disable SYSLIB0060
        using var algorithm = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
        var keyToCheck = algorithm.GetBytes(HashSize);
#pragma warning restore SYSLIB0060

        return keyToCheck.SequenceEqual(key);
    }
}
