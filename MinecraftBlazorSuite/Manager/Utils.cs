using MinecraftBlazorSuite.Models;
using MinecraftBlazorSuite.Models.Enums;
using Newtonsoft.Json;

namespace MinecraftBlazorSuite.Manager;

public class Utils
{
    public static string AppSettingsFile  { get; } = "MinecraftBlazorSuiteSettings.json";
    
    /// <summary>
    /// Uses mc-heads.net to get the heads of minecraft skins
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="username"></param>
    /// <param name="userSelection"></param>
    /// <param name="size"></param>
    /// <returns>URL with selected type</returns>
    public static string GetUserSkin(string uuid, string username, SkinSelectionType userSelection, int size)
    {
        if (string.IsNullOrEmpty(uuid))
        {
            return userSelection switch
            {
                SkinSelectionType.Head => $"https://mc-heads.net/head/{username}/{size}",
                SkinSelectionType.Avatar => $"https://mc-heads.net/avatar/{username}/{size}",
                SkinSelectionType.Skin => $"https://mc-heads.net/player/{username}/{size}",
                SkinSelectionType.Combo => $"https://mc-heads.net/combo/{username}/{size}",
                _ => $"https://mc-heads.net/avatar/{username}/{size}"
            };
        }
        
        return userSelection switch
        {
            SkinSelectionType.Head => $"https://mc-heads.net/head/{uuid}/{size}",
            SkinSelectionType.Avatar => $"https://mc-heads.net/avatar/{uuid}/{size}",
            SkinSelectionType.Skin => $"https://mc-heads.net/player/{uuid}/{size}",
            SkinSelectionType.Combo => $"https://mc-heads.net/combo/{uuid}/{size}",
            _ => $"https://mc-heads.net/avatar/{uuid}/{size}"
        };
    }
    
    /// <summary>
    /// Central method to get settings as a ServerWrapperConfig object
    /// </summary>
    /// <returns>ServerWrapperConfig</returns>
    public static ServerWrapperConfig? ReadFile()
    {
        if (File.Exists(AppSettingsFile))
        {
            string settingsFileContent = File.ReadAllText(AppSettingsFile);
            ServerWrapperConfig? configObj = JsonConvert.DeserializeObject<ServerWrapperConfig>(settingsFileContent);

            return configObj;
        }

        Environment.Exit(1);
        return null;
    }
}