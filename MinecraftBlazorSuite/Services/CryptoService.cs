using BCrypt.Net;

namespace MinecraftBlazorSuite.Services;

public class CryptoService
{
    private static string GenerateSessionValue()
    {
        return Guid.NewGuid().ToString().Replace("-", "")[..9];
    }

    public static string GetSessionValue()
    {
        return GenerateSessionValue();
    }

    public static string GetCreatedTimestamp()
    {
        return Convert.ToString(TimeProvider.System.GetUtcNow().ToUnixTimeSeconds());
    }

    public static bool HasThreeHoursPassed(string rawTimestamp)
    {
        bool ok = long.TryParse(rawTimestamp, out long savedUnixTime);
        if (!ok)
            return false;

        long currentUnixTime = TimeProvider.System.GetUtcNow().ToUnixTimeSeconds();
        long timePassedInSeconds = currentUnixTime - savedUnixTime;

        const long threeHoursInSeconds = 3 * 60 * 60;
        return timePassedInSeconds >= threeHoursInSeconds;
    }

    public static string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentNullException(nameof(password), "Password cannot be null or empty.");

        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, HashType.SHA512, 14);
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentNullException(nameof(password), "Password cannot be null or empty.");
        if (string.IsNullOrWhiteSpace(hashedPassword))
            throw new ArgumentNullException(nameof(hashedPassword), "Hashed password cannot be null or empty.");

        return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword, HashType.SHA512);
    }
}