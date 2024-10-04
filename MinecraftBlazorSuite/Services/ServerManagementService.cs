using System.Diagnostics;
using MinecraftBlazorSuite.Manager;
using MinecraftBlazorSuite.Models;
using MinecraftBlazorSuite.Models.Enums;
using MinecraftBlazorSuite.Models.LogEntries;
using MudBlazor;

namespace MinecraftBlazorSuite.Services;

public class ServerManagementService : IDisposable
{
    private const string JavaArgsPrefix = "-jar -Djline.terminal=jline.UnsupportedTerminal";
    private const string JavaArgsSuffix = "nogui";
    private readonly object _processLock = new();
    private Process? _minecraftServerProcess = null;
    
    public List<LoginLogEntry> Spielerliste { get; set; } = [];
    public string ServerMinecraftVersion { get; set; }
    public event Action<bool, string, string>? OnServerDoneReloadStart;
    public event Action<(string, LogEntryType)>? OnOutputReceived;
    public event Action? OnPlayerJoinLeave;
    public event Action<ServerType>? OnServerTypeDetect;
    public event Action OnRefresh;
    public event Action OnNotifyCrawlingDone;

    private readonly Dictionary<LogEntryType, Func<dynamic, Task>> _logHandlers = [];

    private SettingsService _settingsService;

    public int getServerProcPid()
    {
        return _minecraftServerProcess.Id;
    }
    
    public long MemoryUsage()
    {
        if (_minecraftServerProcess is not { HasExited: false }) 
            return 0;
        
        long memoryUsageByte = _minecraftServerProcess.WorkingSet64;
        long memoryUsageMegabyte = memoryUsageByte / 1024 / 1024;

        return memoryUsageMegabyte;
    }

    private TimeSpan _lastCpuTime;
    private DateTime _lastTime;
    
    public double ProcessorUsage()
    {
        if (_minecraftServerProcess == null || _minecraftServerProcess.HasExited)
            return 0;
        
        TimeSpan currentCpuTime = _minecraftServerProcess.TotalProcessorTime;
        DateTime currentTime = DateTime.Now;

        TimeSpan cpuUsed = currentCpuTime - _lastCpuTime;
        TimeSpan timeElapsed = currentTime - _lastTime;

        _lastCpuTime = currentCpuTime;
        _lastTime = currentTime;

        double cpuUsagePercentage = cpuUsed.TotalMilliseconds / (timeElapsed.TotalMilliseconds * Environment.ProcessorCount) * 100;
        double rounded = Math.Round(cpuUsagePercentage, 3);
        
        return rounded;
    }

    public ServerManagementService(SettingsService settingsService)
    {
        _settingsService = settingsService;
        
        if (_logHandlers.Count == 0)
        {
            _logHandlers = new Dictionary<LogEntryType, Func<dynamic, Task>>
            {
                { LogEntryType.JoinUuid, HandlePlayerJoinUuid },
                { LogEntryType.Login, HandlePlayerLogin },
                { LogEntryType.PlayerLeft, HandlePlayerLeft },
                { LogEntryType.ServerVersion, HandleServerVersion },
                { LogEntryType.ServerStartupDone, HandleServerStartupDone },
                { LogEntryType.ServerReloadDone, HandleServerReloadDone },
                { LogEntryType.ServerTypeSponge, HandleServerTypeDetect },
                { LogEntryType.ServerTypeCraftbukkit, HandleServerTypeDetect },
                { LogEntryType.ConnectionLost, HandlePlayerConnectionLost },
                { LogEntryType.PlayerListParsing, HandlePlayerListParsing },
                { LogEntryType.None, HandleNone }
            };
        }
        
        StartServerProcess(ServerStates.Offline);
        
        _lastCpuTime = _minecraftServerProcess.TotalProcessorTime;
        _lastTime = DateTime.Now;
    }

    public bool StartServerProcess(ServerStates serverState)
    {
        if (_minecraftServerProcess is { HasExited: false })
            return false;

        if (serverState != ServerStates.Offline) 
            return false;
        
        StartServer();
        return true;

    }

    private void StartServer()
    {
        string? jarDirectory = Path.GetDirectoryName(_settingsService.GetSettings().ServerJarPath);
        _minecraftServerProcess = null;
        
        _minecraftServerProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "java",
                Arguments =
                    $"{JavaArgsPrefix} {_settingsService.GetSettings().JavaArguments} \"{_settingsService.GetSettings().ServerJarPath}\" {JavaArgsSuffix}",
                WorkingDirectory = jarDirectory,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                CreateNoWindow = true
            }
        };

        _minecraftServerProcess.OutputDataReceived += OnOutputDataReceived;
        _minecraftServerProcess.Start();
        
        _minecraftServerProcess.BeginOutputReadLine();
    }
    
    private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.Data))
            return;

        (LogEntryType entryType, dynamic entryData) = LogParser.ParseLogEntry(e.Data);
        if (_logHandlers.TryGetValue(entryType, out Func<dynamic, Task>? handler))
        {
            handler(entryData);
        }

        OnOutputReceived?.Invoke((e.Data, entryType));
    }

    private Task HandlePlayerJoinUuid(dynamic consoleLineData)
    {
        (string playerName, string playerUUID, string joinTime) basePlayerData = consoleLineData;
        LoginLogEntry joinedPlayer = new(basePlayerData.joinTime, basePlayerData.playerName, string.Empty,
            basePlayerData.playerUUID);

        Spielerliste.Add(joinedPlayer);
        OnPlayerJoinLeave?.Invoke();
        
        return Task.CompletedTask;
    }

    private Task HandlePlayerLogin(dynamic consoleLineData)
    {
        (string PlayerName, string IPAddress) extendedPlayerData = consoleLineData;
        List<LoginLogEntry> matchingPlayers = Spielerliste.Where(pl =>
            pl.PlayerName.Equals(extendedPlayerData.PlayerName, StringComparison.InvariantCultureIgnoreCase)).ToList();

        if (matchingPlayers.Count > 1)
        {
            Console.WriteLine($"Warning: Found {matchingPlayers.Count} players with the name {extendedPlayerData.PlayerName}");
            LoginLogEntry playerToKeep = matchingPlayers.First();
            Spielerliste = Spielerliste
                .Where(pl => pl != playerToKeep && 
                             !pl.PlayerName.Equals(extendedPlayerData.PlayerName, StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            Spielerliste.Add(playerToKeep);
        }

        LoginLogEntry? playerData = Spielerliste.SingleOrDefault(pl =>
            pl.PlayerName.Equals(extendedPlayerData.PlayerName, StringComparison.InvariantCultureIgnoreCase));

        if (playerData != null)
        {
            playerData.IPAddress = extendedPlayerData.IPAddress;
        }

        OnPlayerJoinLeave?.Invoke();
        return Task.CompletedTask;
    }

    private Task HandlePlayerLeft(dynamic consoleLineData)
    {
        PlayerJoinLeaveEntry playerLeftEntry = consoleLineData;
        Spielerliste.RemoveAll(x =>
            x.PlayerName.Equals(playerLeftEntry.PlayerName, StringComparison.InvariantCultureIgnoreCase));

        OnPlayerJoinLeave?.Invoke();
        return Task.CompletedTask;
    }

    private Task HandleServerVersion(dynamic consoleLineData)
    {
        ServerMinecraftVersion = consoleLineData;
        return Task.CompletedTask;
    }

    private Task HandleServerReloadDone(dynamic consoleLineData)
    {
        string reloadDoneTime = consoleLineData;
        OnServerDoneReloadStart?.Invoke(true, reloadDoneTime, string.Empty);
        
        return Task.CompletedTask;
    }
    
    private Task HandleServerStartupDone(dynamic consoleLineData)
    {
        ServerStartupDone serverStartupDone = consoleLineData;
        OnServerDoneReloadStart?.Invoke(false, serverStartupDone.Time, serverStartupDone.StartupDuration);
        
        return Task.CompletedTask;
    }
    
    private Task HandleServerTypeDetect(dynamic consoleLineData)
    {
        ServerType type = consoleLineData;
        OnServerTypeDetect?.Invoke(type);
        
        return Task.CompletedTask;
    }

    private Task HandlePlayerConnectionLost(dynamic consoleLineData)
    {
        return Task.CompletedTask;
    }

    private Task HandleNone(dynamic consoleLineData)
    {
        return Task.CompletedTask;
    }

    private async Task HandlePlayerListParsing(dynamic consoleLineData)
    {
        PlayerListEntry playerData = consoleLineData;
        foreach (string playerName in playerData.players)
        {
            MojangProfileUser mojangUser = await MinecraftProfileService.GetMinecraftProfileAsync(playerName);
            string playerUuid = string.Empty;
            
            if (!string.IsNullOrEmpty(mojangUser.Id))
                playerUuid = mojangUser.Id;
            
            Spielerliste.Add(new LoginLogEntry(time: playerData.time, playerName: playerName, string.Empty, playerUuid));
        }
        
        OnRefresh?.Invoke();
        OnNotifyCrawlingDone?.Invoke();
    }

    public void Dispose()
    {
        Stop();
    }

    public void SendCommand(string command)
    {
        lock (_processLock)
        {
            if (_minecraftServerProcess is { HasExited: false })
                _minecraftServerProcess.StandardInput.WriteLineAsync(command);
        }
    }

    private void Stop()
    {
        lock (_processLock)
        {
            if (_minecraftServerProcess is not { HasExited: false })
                return;
            
            _minecraftServerProcess.StandardInput.WriteLineAsync("stop");
            _minecraftServerProcess.WaitForExitAsync();

            _minecraftServerProcess.Dispose();
            _minecraftServerProcess = null;
        }
    }
    
}