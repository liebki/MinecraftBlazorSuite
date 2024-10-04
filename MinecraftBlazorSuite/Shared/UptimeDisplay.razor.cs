using Microsoft.AspNetCore.Components;
using MinecraftBlazorSuite.Manager;

namespace MinecraftBlazorSuite.Shared;

partial class UptimeDisplay
{
    [Inject]
    public MinecraftServerStateService stateMan { get; set; }
    
    private string shownUptime = "";

    protected override async Task OnInitializedAsync()
    {
        Timer timer = new Timer(
            e =>
            {
                InvokeAsync(() =>
                {
                    if (shownUptime.Equals(stateMan.GetCurrentUptime(), StringComparison.InvariantCultureIgnoreCase)) 
                        return;
                    
                    shownUptime = stateMan.GetCurrentUptime();
                    StateHasChanged();
                });
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(3)
        );
    }   
}