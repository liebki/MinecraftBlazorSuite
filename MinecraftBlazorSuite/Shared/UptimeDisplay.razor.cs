using Microsoft.AspNetCore.Components;
using MinecraftBlazorSuite.Manager;

namespace MinecraftBlazorSuite.Shared;

partial class UptimeDisplay
{
    private string shownUptime = "";

    [Inject] public MinecraftServerStateService stateMan { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Timer timer = new(
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