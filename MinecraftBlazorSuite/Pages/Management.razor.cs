using Microsoft.AspNetCore.Components;
using MinecraftBlazorSuite.Models;
using MinecraftBlazorSuite.Models.Data;
using MinecraftBlazorSuite.Services;
using MudBlazor;

namespace MinecraftBlazorSuite.Pages;

partial class Management
{
    private readonly ChartOptions Options = new()
    {
        ShowLegend = false,
        YAxisTicks = 200,
        ChartPalette = ["#1565C0"]
    };


    private readonly ChartOptions Options2 = new()
    {
        ShowLegend = false,
        YAxisTicks = 2,

        ChartPalette = ["#8B0000"]
    };

    private readonly List<ChartSeries> SeriesCpu = [new() { Name = "CPU Usage", Data = [0] }];

    private readonly List<ChartSeries> SeriesMemory = [new() { Name = "Memory Usage", Data = [0] }];

    private int Index;

    private string LastCpuValueAvailable = string.Empty;

    private string LastMemoryValueAvailable = string.Empty;

    private string[] XAxisLabels = ["0"];
    [Inject] private ServerManagementService MinecraftServerService { get; set; }

    [Inject] private NotificationService notifyServ { get; set; }

    protected override Task OnInitializedAsync()
    {
        Timer timer = new(
            e =>
            {
                InvokeAsync(() =>
                {
                    LastMemoryValueAvailable = Convert.ToString(MinecraftServerService.MemoryUsage());
                    AddToDataMemoryGraph(MinecraftServerService.MemoryUsage());

                    LastCpuValueAvailable = Convert.ToString(MinecraftServerService.ProcessorUsage());
                    AddToDataCpuGraph(MinecraftServerService.ProcessorUsage() * 10);

                    StateHasChanged();
                });
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(5)
        );

        MinecraftServerService.OnNotifyCrawlingDone += OnCrawlingDone;
        return base.OnInitializedAsync();
    }

    private void AddToDataCpuGraph(double value)
    {
        List<double> existingData = SeriesCpu[0].Data.ToList();
        List<string> labels = XAxisLabels.ToList();

        if (existingData.Count >= 10)
        {
            existingData.RemoveAt(0);
            labels.RemoveAt(0);
        }

        existingData.Add(value);
        string currentTime = DateTime.Now.ToString("HH:mm:ss");

        labels.Add(currentTime);
        SeriesCpu[0].Data = existingData.ToArray();

        XAxisLabels = labels.ToArray();
    }

    private void AddToDataMemoryGraph(double value)
    {
        List<double> existingData = SeriesMemory[0].Data.ToList();
        List<string> labels = XAxisLabels.ToList();

        if (existingData.Count >= 10)
        {
            existingData.RemoveAt(0);
            labels.RemoveAt(0);
        }

        existingData.Add(value);
        string currentTime = DateTime.Now.ToString("HH:mm:ss");

        labels.Add(currentTime);
        SeriesMemory[0].Data = existingData.ToArray();

        XAxisLabels = labels.ToArray();
    }

    private void ForcePlayerlistReset()
    {
        ShowSnack(BasicMessages.PlayerListResetted, icon: Icons.Material.Filled.Settings);
        MinecraftServerService.Spielerliste.Clear();
    }

    private void ForcePlayerlistRecrawl()
    {
        ShowSnack(BasicMessages.PlayerRecrawlStarted, icon: Icons.Material.Filled.Start);
        MinecraftServerService.Spielerliste.Clear();

        MinecraftServerService.SendCommand("list");
    }

    private void OnCrawlingDone()
    {
        ShowSnack(BasicMessages.PlayerRecrawlDone, icon: Icons.Material.Filled.Done);
    }

    private void ShowSnack(string message, Severity severity = Severity.Success,
        string icon = Icons.Material.Filled.Message, Color color = Color.Dark)
    {
        MudblazorSnackbarItem msg = new(message, severity, icon, color);
        notifyServ.NotifyStateChanged(msg);
    }
}