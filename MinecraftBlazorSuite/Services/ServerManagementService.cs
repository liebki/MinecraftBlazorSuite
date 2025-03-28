using MinecraftBlazorSuite.Manager;
using MinecraftBlazorSuite.Models;
using MinecraftBlazorSuite.Models.Enums;
using MinecraftBlazorSuite.Models.LogEntries;

namespace MinecraftBlazorSuite.Services;

public class ServerManagementService : IDisposable
{
    private readonly LogHandler _logHandler;
    private readonly PlayerManager _playerManager;
    private readonly ProcessManager _processManager;
    private readonly MinecraftServerStateService _serverStateService;
    private readonly SettingsService _settingsService;

    private TimeSpan _lastCpuTime;
    private DateTime _lastTime;

    public ServerManagementService(SettingsService settingsService, MinecraftServerStateService serverStateService)
    {
        _settingsService = settingsService;
        _serverStateService = serverStateService;

        _playerManager = new PlayerManager();
        _logHandler = new LogHandler(_playerManager);

        ServerWrapperConfig? settings = _settingsService.GetSettings();
        _processManager = new ProcessManager(settings.ServerJarPath, settings.JavaArguments);
        _processManager.OnOutputReceived += _logHandler.HandleLog;
        _processManager.OnProcessExited += () => _serverStateService.SetServerState(ServerStates.Offline);

        _lastCpuTime = TimeSpan.Zero;
        _lastTime = DateTime.Now;
    }

    public List<LoginLogEntry> Spielerliste => _playerManager.Players;
    public string ServerMinecraftVersion => _logHandler.ServerMinecraftVersion;

    public void Dispose()
    {
        _processManager.Dispose();
    }

    public event Action<bool, string, string>? OnServerDoneReloadStart
    {
        add => _logHandler.OnServerDoneReloadStart += value;
        remove => _logHandler.OnServerDoneReloadStart -= value;
    }

    public event Action<(string, LogEntryType)>? OnOutputReceived
    {
        add => _logHandler.OnOutputReceived += value;
        remove => _logHandler.OnOutputReceived -= value;
    }

    public event Action? OnPlayerJoinLeave
    {
        add => _playerManager.OnPlayerListChanged += value;
        remove => _playerManager.OnPlayerListChanged -= value;
    }

    public event Action<ServerType>? OnServerTypeDetect
    {
        add => _logHandler.OnServerTypeDetect += value;
        remove => _logHandler.OnServerTypeDetect -= value;
    }

    public event Action OnRefresh
    {
        add => _logHandler.OnRefresh += value;
        remove => _logHandler.OnRefresh -= value;
    }

    public event Action OnNotifyCrawlingDone
    {
        add => _logHandler.OnNotifyCrawlingDone += value;
        remove => _logHandler.OnNotifyCrawlingDone -= value;
    }

    public int getServerProcPid()
    {
        return _processManager.ProcessId ?? -1;
    }

    public long MemoryUsage()
    {
        return _processManager.MemoryUsage();
    }

    public double ProcessorUsage()
    {
        if (!_processManager.IsRunning)
            return 0;

        var currentCpuTime = _processManager.CurrentCpuTime;
        var currentTime = DateTime.Now;

        var cpuUsed = currentCpuTime - _lastCpuTime;
        var timeElapsed = currentTime - _lastTime;

        _lastCpuTime = currentCpuTime;
        _lastTime = currentTime;

        var cpuUsagePercentage = cpuUsed.TotalMilliseconds /
            (timeElapsed.TotalMilliseconds * Environment.ProcessorCount) * 100;
        return Math.Round(cpuUsagePercentage, 3);
    }

    public bool StartServerProcess(ServerStates serverState)
    {
        if (serverState != ServerStates.Offline)
            return false;

        if (_processManager.Start())
        {
            _serverStateService.SetServerState(ServerStates.Starting);
            return true;
        }

        return false;
    }

    public void SendCommand(string command)
    {
        _processManager.SendCommand(command);
    }
}