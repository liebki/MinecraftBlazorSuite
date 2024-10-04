namespace MinecraftBlazorSuite.Models;

public class ServerWrapperConfig
{
    public int MaxConsoleMessages { get; set; }

    public string ServerJarPath { get; set; }

    public string JavaArguments { get; set; }
    
    public string PanelAccess { get; set; }
}