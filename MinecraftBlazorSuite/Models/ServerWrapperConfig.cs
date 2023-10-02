
namespace MinecraftBlazorSuite.Models
{
    public class ServerWrapperConfig
    {
        public string DatabaseHashSalt { get; set; }
        public int MaxConsoleMessages { get; set; }
        public string ServerJarPath { get; set; }
        public string JavaArguments { get; set; }
        public string MinecraftServerType { get; set; }
        public string MinecraftServerVersion { get; set; }

    }
}
