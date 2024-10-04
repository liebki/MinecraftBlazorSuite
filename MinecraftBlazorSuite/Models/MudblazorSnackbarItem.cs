using MudBlazor;

namespace MinecraftBlazorSuite.Models;

public class MudblazorSnackbarItem
{
    public MudblazorSnackbarItem(string message, Severity severity, string icon, Color iconColor)
    {
        this.message = message;
        this.severity = severity;
        this.icon = icon;
        this.iconColor = iconColor;
    }

    public string message { get; set; }

    public Severity severity { get; set; } = Severity.Normal;

    public string icon { get; set; } = Icons.Material.Filled.Message;

    public Color iconColor { get; set; } = Color.Dark;
    
}