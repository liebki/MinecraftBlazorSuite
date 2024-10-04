using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MinecraftBlazorSuite.Services;

namespace MinecraftBlazorSuite.Pages;

partial class Main
{
    [Inject]
    public ProtectedLocalStorage localStore { get; set; }
    
    [Inject]
    public SettingsService settingsService { get; set; }
    
    [Inject]
    public CryptoService cryptoService { get; set; }
    
    [Inject]
    public SqliteService sqliteService { get; set; }
    
    [Inject]
    public NavigationManager navMan { get; set; }
    
    private string RawPasswordContent { get; set; }
    
    public async Task ExecuteLogin()
    {
        bool verificationSuccess = false;
        string tempAdminHash = settingsService.GetSettings().PanelAccess;
        
        verificationSuccess = CryptoService.VerifyPassword(RawPasswordContent, tempAdminHash);

        if (!verificationSuccess)
            return;
        
        string sessionValue = CryptoService.GetSessionValue();
        string sessionCreated = CryptoService.GetCreatedTimestamp();
            
        await localStore.SetAsync("session", sessionValue);
        sqliteService.AddSession(sessionValue, sessionCreated);
            
        navMan.NavigateTo("/help", true);
    }

}