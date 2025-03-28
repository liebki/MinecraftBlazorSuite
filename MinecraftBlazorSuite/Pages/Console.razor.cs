using Microsoft.AspNetCore.Components;
using MinecraftBlazorSuite.Manager;
using MinecraftBlazorSuite.Models;
using MinecraftBlazorSuite.Models.Data;
using MinecraftBlazorSuite.Models.Enums;
using MinecraftBlazorSuite.Services;
using MudBlazor;

namespace MinecraftBlazorSuite.Pages;

partial class Console : IDisposable
{
    private readonly List<(string, LogEntryType)> _consoleContent = [];
    [Inject] private ServerManagementService MinecraftServerService { get; set; }

    [Inject] private NotificationService notifyServ { get; set; }

    [Inject] private MinecraftServerStateService ServerStateService { get; set; }

    [Inject] private SettingsService settingsMan { get; set; }

    private string CommandInput { get; set; } = string.Empty;

    public void Dispose()
    {
        _consoleContent.Clear();
        CommandInput = string.Empty;

        MinecraftServerService.OnOutputReceived -= HandleOutputReceived;
        MinecraftServerService.OnOutputReceived -= HandleOutputReceived;
    }

    private void SendStopCommand()
    {
        if (ServerStateService.GetCurrentServerState() == ServerStates.Online)
        {
            ShowSnack(StatusMessages.StatusMessageServerShutdown,
                Severity.Warning, Icons.Material.Filled.PowerOff);
            SendCommand(command: "stop", showExecution: false);

            ServerStateService.SetServerState(ServerStates.Offline);
            ServerStateService.AreControlsLocked = true;

            InvokeAsync(StateHasChanged);
            return;
        }

        ShowSnack(StatusMessages.StatusMessageServerIsOffline, Severity.Error,
            Icons.Material.Filled.SmsFailed);
    }


    private void StartServerExec()
    {
        MinecraftServerService.StartServerProcess(ServerStateService.GetCurrentServerState());
        ServerStateService.SetServerState(ServerStates.Online);

        InvokeAsync(StateHasChanged);

        ShowSnack(StatusMessages.StatusMessageServerIsBootingUp, Severity.Warning,
            Icons.Material.Filled.Timer);
    }

    private void SendCommand(bool directCommand = false, string command = "", bool showExecution = true)
    {
        if (ServerStateService.GetCurrentServerState() != ServerStates.Online)
        {
            ShowSnack(BasicMessages.ServerIsOfflineError, Severity.Error,
                Icons.Material.Filled.SmsFailed);
            return;
        }

        string cmd = command;
        if (directCommand)
        {
            cmd = CommandInput;
            CommandInput = string.Empty;
        }

        if (string.IsNullOrEmpty(cmd) || cmd.Length < 3)
        {
            ShowSnack(BasicMessages.NoCommandInInputfield, Severity.Error,
                Icons.Material.Filled.Error);
            return;
        }

        MinecraftServerService.SendCommand(cmd);

        if (showExecution)
            ShowSnack(string.Format(BasicMessages.CommandExecutionInformation, cmd),
                icon: Icons.Material.Filled.KeyboardCommandKey);
    }

    protected override void OnInitialized()
    {
        MinecraftServerService.OnOutputReceived += HandleOutputReceived;
        MinecraftServerService.OnServerDoneReloadStart += Event_StartupReloadDone;

        MinecraftServerService.OnServerTypeDetect += SetServerType;
        MinecraftServerService.OnRefresh += CallRefresh;
    }

    private void Event_StartupReloadDone(bool isReload, string doneTime, string startupDuration)
    {
        if (isReload)
        {
            ShowSnack(string.Format(StatusMessages.StatusMessageFinishedReload, doneTime),
                icon: Icons.Material.Filled.SecurityUpdateGood);
        }
        else
        {
            ServerStateService.AreControlsLocked = false;
            ServerStateService.SetServerState(ServerStates.Online);

            ShowSnack(string.Format(StatusMessages.StatusMessageFinishedStartup, doneTime, startupDuration),
                icon: Icons.Material.Filled.ThumbUp);
            InvokeAsync(StateHasChanged);
        }
    }

    private void HandleOutputReceived((string consoleLine, LogEntryType entryType) output)
    {
        InvokeAsync(() =>
        {
            if (output.entryType is LogEntryType.None)
                PrintBasicMessageToConsole(output.consoleLine);
            else
                _consoleContent.Add((output.consoleLine, output.entryType));

            TrimConsoleContentIfNeeded();
            StateHasChanged();
        });
    }

    private void PrintBasicMessageToConsole(string messageContent)
    {
        string[] consoleMessageParts = messageContent.Split("] ");
        if (consoleMessageParts.Length != 2)
            return;

        string timestamp = $"<small>{consoleMessageParts[0]}]</small>";
        string message = $"[{consoleMessageParts[1].Substring(1)}";

        _consoleContent.Add(($"{timestamp} {message}", LogEntryType.None));
    }

    private void SetServerType(ServerType type)
    {
        ServerStateService.Type = type;
        MinecraftServerService.OnServerTypeDetect += SetServerType;

        InvokeAsync(StateHasChanged);
    }

    private void TrimConsoleContentIfNeeded()
    {
        if (_consoleContent.Count > settingsMan.GetSettings().MaxConsoleMessages)
            _consoleContent.RemoveAt(0);
    }

    private void CallRefresh()
    {
        InvokeAsync(StateHasChanged);
    }

    private void ShowSnack(string message, Severity severity = Severity.Success,
        string icon = Icons.Material.Filled.Message, Color color = Color.Dark)
    {
        MudblazorSnackbarItem msg = new(message, severity, icon, color);
        notifyServ.NotifyStateChanged(msg);
    }
}