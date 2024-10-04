using MinecraftBlazorSuite.Models;

namespace MinecraftBlazorSuite.Services;

public class NotificationService
{
    public Action<MudblazorSnackbarItem> OnChange { get; set; }
    
    public void NotifyStateChanged(MudblazorSnackbarItem snackItem)
    {
        OnChange?.Invoke(snackItem);
    }
}