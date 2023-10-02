
using MinecraftBlazorSuite.Models;
using MinecraftBlazorSuite.Models.Enums;
using Newtonsoft.Json;

namespace MinecraftBlazorSuite.Manager
{
    public static class Tools
    {
        private const string AppSettingsFile = "BlazorMinecraftServerSettings.json";
        private const string ErrorFile = "error-no-config-found.txt";
        private const string ErrorFileMessage = $"An error occurred: Configuration '{AppSettingsFile}' file not found!";


        private static ServerWrapperConfig activeSettings;

        public static ServerWrapperConfig Settings
        {
            get
            {
                return activeSettings;
            }
        }

        public static void GetSettings()
        {
            activeSettings = ReadSettingsFile();
        }

        private static ServerWrapperConfig ReadSettingsFile()
        {
            if (File.Exists(AppSettingsFile))
            {
                string SettingsFileContent = File.ReadAllText(AppSettingsFile);
                return JsonConvert.DeserializeObject<ServerWrapperConfig>(SettingsFileContent);
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

        public static string GetUserSkin(string UUID, SkinSelectionType userSelection, int size)
        {
            return userSelection switch
            {
                SkinSelectionType.Head => $"https://mc-heads.net/head/{UUID}/{size}",
                SkinSelectionType.Avatar => $"https://mc-heads.net/avatar/{UUID}/{size}",
                SkinSelectionType.Skin => $"https://mc-heads.net/player/{UUID}/{size}",
                SkinSelectionType.Combo => $"https://mc-heads.net/combo/{UUID}/{size}",
                _ => $"https://mc-heads.net/avatar/{UUID}/{size}",
            };
        }

    }
}
