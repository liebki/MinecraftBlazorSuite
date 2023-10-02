
using MinecraftBlazorSuite.Models.Enums;
using MinecraftBlazorSuite.Models.LogEntries;
using System;
using System.Text.RegularExpressions;

namespace MinecraftBlazorSuite.Services
{

    public static partial class LogParser
    {

        private static readonly RegexOptions CompiledRegexOptions = RegexOptions.Compiled;
        private static readonly Regex joinRegex = new(@"\[(\d+:\d+:\d+)\] \[.*\]: UUID of player (\w+) is ([\w-]+)", CompiledRegexOptions);

        private static readonly Regex loginRegex = new(@"\[(\d+:\d+:\d+)\] \[.*\]: (\w+)\[([^\]]+)\] logged in with entity id \d+ at \((\[[^\]]+\])?(.*?)\)", CompiledRegexOptions);
        private static readonly Regex chatRegex = new(@"\[(\d+:\d+:\d+)\] \[.*\]: <(\w+)> (.+)", CompiledRegexOptions);

        private static readonly Regex commandRegex = new(@"\[(\d+:\d+:\d+)\] \[.*\]: (\w+) issued server command: (.+)", CompiledRegexOptions);
        private static readonly Regex connectionLostRegex = new(@"\[(\d+:\d+:\d+)\] \[.*\]: (\w+) lost connection: Disconnected", CompiledRegexOptions);

        private static readonly Regex playerLeftRegex = new(@"\[(\d+:\d+:\d+)\] \[.*\]: (\w+) left the game", CompiledRegexOptions);

        public static (LogEntryType, dynamic) ParseLogEntry(string logEntry)
        {
            Match match;

            match = joinRegex.Match(logEntry);
            if (match.Success)
            {
                return (LogEntryType.JoinUUID, GetJoinEntry(match));
            }

            match = loginRegex.Match(logEntry);
            if (match.Success)
            {
                return (LogEntryType.Login, GetLoginEntry(match));
            }

            match = chatRegex.Match(logEntry);
            if (match.Success)
            {
                return (LogEntryType.Chat, GetChatEntry(match));
            }

            match = commandRegex.Match(logEntry);
            if (match.Success)
            {
                return (LogEntryType.Command, GetCommandEntry(match));
            }

            match = connectionLostRegex.Match(logEntry);
            if (match.Success)
            {
                return (LogEntryType.ConnectionLost, GetConnectionLostEntry(match));
            }

            match = playerLeftRegex.Match(logEntry);
            if (match.Success)
            {
                return (LogEntryType.PlayerLeft, GetPlayerLeftEntry(match));
            }

            return (LogEntryType.None, null);
        }

        private static (string, string, string) GetJoinEntry(Match match)
        {
            string JoinTime = match.Groups[1].Value;
            string PlayerName = match.Groups[2].Value;

            string PlayerUUID = match.Groups[3].Value;
            return (PlayerName, PlayerUUID, JoinTime);
        }

        private static (string, string) GetLoginEntry(Match match)
        {
            string PlayerName = match.Groups[2].Value;
            string IPAddress = match.Groups[3].Value.Remove(0, 1);

            return (PlayerName, IPAddress);
        }

        private static ChatLogEntry GetChatEntry(Match match)
        {
            return new ChatLogEntry
            {
                Time = match.Groups[1].Value,
                PlayerName = match.Groups[2].Value,
                Message = match.Groups[3].Value
            };
        }

        private static CommandLogEntry GetCommandEntry(Match match)
        {
            return new CommandLogEntry
            {
                Time = match.Groups[1].Value,
                PlayerName = match.Groups[2].Value,
                Command = match.Groups[3].Value
            };
        }

        private static ConnectionLostLogEntry GetConnectionLostEntry(Match match)
        {
            return new ConnectionLostLogEntry
            {
                Time = match.Groups[1].Value,
                PlayerName = match.Groups[2].Value
            };
        }

        private static PlayerLeftLogEntry GetPlayerLeftEntry(Match match)
        {
            return new PlayerLeftLogEntry
            {
                Time = match.Groups[1].Value,
                PlayerName = match.Groups[2].Value
            };
        }
    }

}
