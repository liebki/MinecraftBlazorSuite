using MinecraftBlazorSuite.Models.Enums;
using MinecraftBlazorSuite.Models.LogEntries;

namespace MinecraftBlazorSuite.Services;

public class LogHandler
{
    private readonly Dictionary<LogEntryType, Func<object, Task>> _logHandlers;
    private readonly PlayerManager _playerManager;

    public LogHandler(PlayerManager playerManager)
    {
        _playerManager = playerManager;
        _logHandlers = new Dictionary<LogEntryType, Func<object, Task>>
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

    public string ServerMinecraftVersion { get; private set; }

    public event Action<(string, LogEntryType)>? OnOutputReceived;
    public event Action<bool, string, string>? OnServerDoneReloadStart;
    public event Action<ServerType>? OnServerTypeDetect;
    public event Action? OnRefresh;
    public event Action? OnNotifyCrawlingDone;

    public void HandleLog(string logLine)
    {
        if (string.IsNullOrEmpty(logLine))
            return;

        var (entryType, entryData) = LogParser.ParseLogEntry(logLine);
        OnOutputReceived?.Invoke((logLine, entryType));

        if (_logHandlers.TryGetValue(entryType, out Func<object, Task>? handler)) handler(entryData);
    }

    private Task HandlePlayerJoinUuid(object data)
    {
        (string playerName, string playerUUID, string joinTime) = (ValueTuple<string, string, string>)data;
        LoginLogEntry joinedPlayer = new(joinTime, playerName, string.Empty, playerUUID);
        _playerManager.AddPlayer(joinedPlayer);
        return Task.CompletedTask;
    }

    private Task HandlePlayerLogin(object data)
    {
        var (playerName, ipAddress) = (ValueTuple<string, string>)data;
        _playerManager.HandleDuplicatePlayers(playerName);
        _playerManager.UpdatePlayerIP(playerName, ipAddress);
        return Task.CompletedTask;
    }

    private Task HandlePlayerLeft(object data)
    {
        var playerLeftEntry = (PlayerJoinLeaveEntry)data;
        _playerManager.RemovePlayer(playerLeftEntry.PlayerName);
        return Task.CompletedTask;
    }

    private Task HandleServerVersion(object data)
    {
        ServerMinecraftVersion = (string)data;
        return Task.CompletedTask;
    }

    private Task HandleServerReloadDone(object data)
    {
        var reloadDoneTime = (string)data;
        OnServerDoneReloadStart?.Invoke(true, reloadDoneTime, string.Empty);
        return Task.CompletedTask;
    }

    private Task HandleServerStartupDone(object data)
    {
        var serverStartupDone = (ServerStartupDone)data;
        OnServerDoneReloadStart?.Invoke(false, serverStartupDone.Time, serverStartupDone.StartupDuration);
        return Task.CompletedTask;
    }

    private Task HandleServerTypeDetect(object data)
    {
        var type = (ServerType)data;
        OnServerTypeDetect?.Invoke(type);
        return Task.CompletedTask;
    }

    private Task HandlePlayerConnectionLost(object data)
    {
        return Task.CompletedTask;
    }

    private Task HandleNone(object data)
    {
        return Task.CompletedTask;
    }

    private async Task HandlePlayerListParsing(object data)
    {
        var playerData = (PlayerListEntry)data;
        foreach (var playerName in playerData.players)
        {
            var mojangUser = await MinecraftProfileService.GetMinecraftProfileAsync(playerName);
            var playerUuid = string.IsNullOrEmpty(mojangUser.Id) ? string.Empty : mojangUser.Id;
            _playerManager.AddPlayer(new LoginLogEntry(playerData.time, playerName, string.Empty, playerUuid));
        }

        OnRefresh?.Invoke();
        OnNotifyCrawlingDone?.Invoke();
    }
}