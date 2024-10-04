using System.Text.RegularExpressions;
using MinecraftBlazorSuite.Models.Data;
using MinecraftBlazorSuite.Models.Enums;
using MinecraftBlazorSuite.Models.LogEntries;

namespace MinecraftBlazorSuite.Services;

public static class LogParser
{
    // TODO: (Dictionary rework)
    public static (LogEntryType, dynamic) ParseLogEntry(string logEntry)
    {
        Match consoleLineMatch = LogParserRegexes.PlayerJoinedRegex.Match(logEntry);
        if (consoleLineMatch.Success) 
            return (LogEntryType.JoinUuid, GetJoinEntry(consoleLineMatch));

        consoleLineMatch = LogParserRegexes.PlayerLoggedinRegex.Match(logEntry);
        if (consoleLineMatch.Success) 
            return (LogEntryType.Login, GetLoginEntry(consoleLineMatch));

        consoleLineMatch = LogParserRegexes.PlayerChatRegex.Match(logEntry);
        if (consoleLineMatch.Success) 
            return (LogEntryType.Chat, GetChatEntry(consoleLineMatch));

        consoleLineMatch = LogParserRegexes.PlayerCommandExecuteRegex.Match(logEntry);
        if (consoleLineMatch.Success) 
            return (LogEntryType.Command, GetCommandEntry(consoleLineMatch));

        consoleLineMatch = LogParserRegexes.PlayerConnectionLostRegex.Match(logEntry);
        if (consoleLineMatch.Success) 
            return (LogEntryType.ConnectionLost, GetConnectionLostEntry(consoleLineMatch));

        consoleLineMatch = LogParserRegexes.PlayerLeftRegex.Match(logEntry);
        if (consoleLineMatch.Success) 
            return (LogEntryType.PlayerLeft, GetPlayerJoinOrLeave(consoleLineMatch));

        consoleLineMatch = LogParserRegexes.PlayerJoinRegex.Match(logEntry);
        if (consoleLineMatch.Success) 
            return (LogEntryType.PlayerJoined, GetPlayerJoinOrLeave(consoleLineMatch));

        consoleLineMatch = LogParserRegexes.ServerVersionDetectRegex.Match(logEntry);
        if (consoleLineMatch.Success) 
            return (LogEntryType.ServerVersion, GetServerVersion(consoleLineMatch));

        consoleLineMatch = LogParserRegexes.ServerDoneStartupRegex.Match(logEntry);
        if (consoleLineMatch.Success) 
            return (LogEntryType.ServerStartupDone, GetServerStartupDone(consoleLineMatch));

        consoleLineMatch = LogParserRegexes.ServerReloadCompleteRegex.Match(logEntry);
        if (consoleLineMatch.Success) 
            return (LogEntryType.ServerReloadDone, GetServerReloadDone(consoleLineMatch));

        consoleLineMatch = LogParserRegexes.SpongeServerRegex.Match(logEntry);
        if (consoleLineMatch.Success) 
            return (LogEntryType.ServerTypeSponge, IsServerTypeSponge(consoleLineMatch));

        consoleLineMatch = LogParserRegexes.CraftbukkitServerRegex.Match(logEntry);
        if (consoleLineMatch.Success) 
            return (LogEntryType.ServerTypeCraftbukkit, GetSpigotServerType(consoleLineMatch));

        consoleLineMatch = LogParserRegexes.ListPlayerDetectionRegex.Match(logEntry);
        if (consoleLineMatch.Success) 
            return (LogEntryType.PlayerListParsing, GetPlayerList(consoleLineMatch));

        return (LogEntryType.None, consoleLineMatch);
    }

    private static (string, string, string) GetJoinEntry(Match match)
    {
        string joinTime = match.Groups["loggingTime"].Value;
        string playerName = match.Groups["playerName"].Value;

        string playerUuid = match.Groups["playerUuid"].Value;
        return (playerName, playerUuid, joinTime);
    }

    private static (string, string) GetLoginEntry(Match match)
    {
        string playerName = match.Groups["playerName"].Value;
        string ipAddress = match.Groups["playerAddress"].Value.Remove(0, 1);

        return (playerName, ipAddress);
    }

    private static ChatLogEntry GetChatEntry(Match match)
    {
        return new ChatLogEntry
        {
            Time = match.Groups["loggingTime"].Value,
            PlayerName = match.Groups["playerName"].Value,
            Message = match.Groups["chatMessage"].Value
        };
    }

    private static CommandLogEntry GetCommandEntry(Match match)
    {
        return new CommandLogEntry
        {
            Time = match.Groups["loggingTime"].Value,
            PlayerName = match.Groups["playerName"].Value,
            Command = match.Groups["commandMessage"].Value
        };
    }

    private static ConnectionLostLogEntry GetConnectionLostEntry(Match match)
    {
        return new ConnectionLostLogEntry
        {
            Time = match.Groups["loggingTime"].Value,
            PlayerName = match.Groups["playerName"].Value
        };
    }

    private static string GetServerReloadDone(Match match)
    {
        return match.Groups["loggingTime"].Value;
    }

    private static ServerStartupDone GetServerStartupDone(Match match)
    {
        return new ServerStartupDone
        {
            Time = match.Groups["loggingTime"].Value,
            StartupDuration = match.Groups["startupDuration"].Value.Replace("s", "")
        };
    }
    
    private static ServerType GetSpigotServerType(Match match)
    {
        string spigotServerType = match.Groups["craftbukkitServerType"].Value;
        if(spigotServerType.Contains("Spigot", StringComparison.InvariantCultureIgnoreCase))
        {
            return ServerType.Spigot;
        }
        
        return ServerType.Bukkit;
    }
    
    private static ServerType IsServerTypeSponge(Match match)
    {
        string sponge = match.Groups["spongeServer"].Value;
        if (string.Equals(sponge, "Sponge", StringComparison.InvariantCultureIgnoreCase))
        {
            return ServerType.Sponge;
        }

        return ServerType.Vanilla;
    }

    private static PlayerJoinLeaveEntry GetPlayerJoinOrLeave(Match match)
    {
        return new PlayerJoinLeaveEntry
        {
            Time = match.Groups["loggingTime"].Value,
            PlayerName = match.Groups["playerName"].Value
        };
    }
    
    private static PlayerListEntry GetPlayerList(Match match)
    {
        string rawPlayerList = match.Groups["playerNames"].Value;
        string[] playerList = rawPlayerList.Split(',');

        playerList = playerList.Select(x => x.Trim()).ToArray();
        PlayerListEntry players = new PlayerListEntry
        {
            time = match.Groups["loggingTime"].Value,
            activePlayer = match.Groups["playerCount"].Value,
            maxPlayers = match.Groups["maxPlayerCount"].Value,
            players = playerList
        };

        return players;
    }

    
    private static string GetServerVersion(Match match)
    {
        return match.Groups["minecraftVersion"].Value;
    }
}