using Microsoft.AspNetCore.Components;
using MinecraftBlazorSuite.Models;
using MinecraftBlazorSuite.Services;

namespace MinecraftBlazorSuite.Pages;

partial class Settings
{
    [Inject] public CryptoService cryptoService { get; set; }

    [Inject] public SettingsService settingsMan { get; set; }

    private string RawPasswordContent { get; set; }

    private void UpdatePass()
    {
        string hashedPass = CryptoService.HashPassword(RawPasswordContent);
        RawPasswordContent = "";

        ServerWrapperConfig activeSettings = settingsMan.GetSettings();
        activeSettings.PanelAccess = hashedPass;

        settingsMan.SetSettings(activeSettings);
        StateHasChanged();
    }
}