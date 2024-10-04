using MinecraftBlazorSuite.Models;
using Newtonsoft.Json;

namespace MinecraftBlazorSuite.Services;

public class SettingsService
{
    private const string AppSettingsFile = "MinecraftBlazorSuiteSettings.json";
    private const string ErrorFile = "error-no-config-found.txt";
    private const string ErrorFileMessage = $"An error occurred: Configuration '{AppSettingsFile}' file not found!";
    
    private ServerWrapperConfig? Settings { get; set; }
    
    public void SetSettings(ServerWrapperConfig settings)
    {
        string settingsJson = JsonConvert.SerializeObject(settings, Formatting.Indented);   
        File.WriteAllText(AppSettingsFile, settingsJson);
    }
    
    public ServerWrapperConfig? GetSettings()
    {
        Settings = ReadFile();
        return Settings;
    }
    
    private static ServerWrapperConfig? ReadFile()
    {
        if (File.Exists(AppSettingsFile))
        {
            string settingsFileContent = File.ReadAllText(AppSettingsFile);
            ServerWrapperConfig? configObj = JsonConvert.DeserializeObject<ServerWrapperConfig>(settingsFileContent);

            return configObj;
        }

        CreateErrorFile();
        Environment.Exit(1);

        return null;
    }
    
    private static void CreateErrorFile()
    {
        using FileStream fileStream = new(ErrorFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        using StreamWriter writer = new(fileStream);

        writer.WriteLine(ErrorFileMessage);
    }
    
}