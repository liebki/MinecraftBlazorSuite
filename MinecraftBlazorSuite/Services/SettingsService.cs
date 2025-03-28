using MinecraftBlazorSuite.Manager;
using MinecraftBlazorSuite.Models;
using Newtonsoft.Json;

namespace MinecraftBlazorSuite.Services;

public class SettingsService
{
    private const string ErrorFile = "error-no-config-found.txt";

    private static readonly string ErrorFileMessage =
        $"An error occurred: Configuration '{Utils.AppSettingsFile}' file not found!";

    private ServerWrapperConfig? Settings { get; set; }

    public void SetSettings(ServerWrapperConfig settings)
    {
        string settingsJson = JsonConvert.SerializeObject(settings, Formatting.Indented);
        File.WriteAllText(Utils.AppSettingsFile, settingsJson);
    }

    public ServerWrapperConfig? GetSettings()
    {
        Settings = Utils.ReadFile();
        return Settings;
    }

    private static void CreateErrorFile()
    {
        using FileStream fileStream = new(ErrorFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        using StreamWriter writer = new(fileStream);

        writer.WriteLine(ErrorFileMessage);
    }
}