using Microsoft.AspNetCore.Components;
using MinecraftBlazorSuite.Services;

namespace MinecraftBlazorSuite.Pages;

partial class Settings
{
    [Inject] private ServerManagementService minecraftServerService { get; set; }

    private void ForcePlayerlistReset()
    {
        minecraftServerService.Spielerliste.Clear();
    }

    private void ForcePlayerlistRecrawl()
    {
        //TODO: Recrawl using /list and parsing content
    }
}