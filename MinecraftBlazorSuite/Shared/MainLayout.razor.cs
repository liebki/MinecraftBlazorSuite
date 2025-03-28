using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MinecraftBlazorSuite.Models;
using MinecraftBlazorSuite.Services;
using MudBlazor;

namespace MinecraftBlazorSuite.Shared;

partial class MainLayout
{
    private readonly MudTheme _theme = new()
    {
        PaletteLight = new PaletteLight
        {
            PrimaryContrastText = "#292929",
            AppbarText = "#292929",
            DrawerText = "#292929",
            TextPrimary = "#292929",
            Background = "#FFF",
            BackgroundGray = "#d9d9d9",
            DrawerBackground = "#d9d9d9",
            AppbarBackground = "#d9d9d9",
            Divider = "#808080"
        }
    };

    [Inject] public ProtectedLocalStorage localStore { get; set; }

    [Inject] public SettingsService settingsService { get; set; }

    [Inject] public CryptoService cryptoService { get; set; }

    [Inject] public SqliteService sqliteService { get; set; }

    [Inject] public NavigationManager navMan { get; set; }

    [Inject] public NotificationService notifyServ { get; set; }

    [Inject] public ISnackbar snackbarServ { get; set; }

    private bool IsLoggedIn { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender || IsLoggedIn) return;

        string currentUri = navMan.Uri;
        if (!currentUri.EndsWith('/'))
        {
            ProtectedBrowserStorageResult<string> sessionItem = await localStore.GetAsync<string>("session");

            if (!sessionItem.Success || string.IsNullOrWhiteSpace(sessionItem.Value))
            {
                navMan.NavigateTo("/", true);
                return;
            }

            string sessionValue = sessionItem.Value;
            Task<UserSession?> userSessionTask = Task.Run(() => sqliteService.GetSingleUserSession(sessionValue));

            UserSession? userSession = await userSessionTask;
            if (userSession != null)
            {
                bool isSessionExpired = CryptoService.HasThreeHoursPassed(userSession.SessionCreated);
                if (isSessionExpired)
                {
                    await localStore.DeleteAsync("session");
                    Task.Run(() => sqliteService.DeleteSession(sessionValue));

                    navMan.NavigateTo("/", true);
                    return;
                }

                IsLoggedIn = true;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                navMan.NavigateTo("/", true);
            }
        }
    }

    protected override Task OnInitializedAsync()
    {
        notifyServ.OnChange += Update;
        return base.OnInitializedAsync();
    }

    private void Update(MudblazorSnackbarItem msg)
    {
        snackbarServ.Add(msg.message, msg.severity, config =>
        {
            config.Icon = msg.icon;
            config.IconColor = msg.iconColor;
        });
    }
}