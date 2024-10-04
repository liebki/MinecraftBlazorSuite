using MinecraftBlazorSuite.Models.Enums;

namespace MinecraftBlazorSuite.Manager;

public class Utils
{
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
}