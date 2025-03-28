using System.Text.RegularExpressions;

namespace MinecraftBlazorSuite.Models.Data;

public static partial class LogParserRegexes
{
    // Server Type Detection

    public static readonly Regex SpongeServerRegex = SpongeServerDetection();

    public static readonly Regex CraftbukkitServerRegex = CraftbukkitServerDetection();

    // Basics

    public static readonly Regex ServerReloadCompleteRegex = ServerReloadCompleteDetection();

    public static readonly Regex ServerDoneStartupRegex = ServerStartupDoneDetection();

    public static readonly Regex PlayerJoinedRegex = PlayerJoinedServerDetection();

    public static readonly Regex ServerVersionDetectRegex = ServerVersionDetection();

    public static readonly Regex PlayerLoggedinRegex = PlayerServerLoginDetection();

    public static readonly Regex PlayerChatRegex = PlayerServerChatDetection();

    public static readonly Regex PlayerCommandExecuteRegex = PlayerServerCommandExecDetection();

    public static readonly Regex PlayerConnectionLostRegex = PlayerServerConnectionLostDetection();

    public static readonly Regex PlayerLeftRegex = PlayerServerLeftDetection();

    public static readonly Regex PlayerJoinRegex = PlayerServerJoinDetection();

    public static readonly Regex ListPlayerDetectionRegex = PlayerServerListPlayersDetection();

    // Generated Regex:

    [GeneratedRegex(@"\[(?<loggingTime>\d+:\d+:\d+)\] \[.*\]: Loading (?<spongeServer>[A-Za-z])+, please wait\.\.\.",
        RegexOptions.Compiled)]
    private static partial Regex SpongeServerDetection();

    [GeneratedRegex(
        @"\[(?<loggingTime>\d+:\d+:\d+)\] \[.*\]: This server is running (?<craftbukkit>[A-Za-z]+) version (?<craftbukkitServerType>[A-Za-z0-9]+(-[A-Za-z0-9]+)+) \(MC: (?<mcVersion>[0-9]+(\.[0-9]+)+)\) \((?<spigotApiVersion>[^)]*\))",
        RegexOptions.Compiled)]
    private static partial Regex CraftbukkitServerDetection();

    [GeneratedRegex(@"\[(?<loggingTime>\d+:\d+:\d+)\] \[.*\]: CONSOLE: Reload complete.", RegexOptions.Compiled)]
    private static partial Regex ServerReloadCompleteDetection();

    [GeneratedRegex(
        "\\[(?<loggingTime>\\d+:\\d+:\\d+)\\] \\[.*\\]: Done \\((?<startupDuration>[0-9]*\\.[0-9]+)s\\)! For help, type \"help\"",
        RegexOptions.Compiled)]
    private static partial Regex ServerStartupDoneDetection();

    [GeneratedRegex(
        @"\[(?<loggingTime>\d+:\d+:\d+)\] \[.*\]: UUID of player (?<playerName>\w+) is (?<playerUuid>[\w-]+)",
        RegexOptions.Compiled)]
    private static partial Regex PlayerJoinedServerDetection();

    [GeneratedRegex(
        @"\[(?<loggingTime>\d+:\d+:\d+)\] \[.*\]: Starting minecraft server version (?<minecraftVersion>[0-9].+)",
        RegexOptions.Compiled)]
    private static partial Regex ServerVersionDetection();

    [GeneratedRegex(
        @"\[(?<loggingTime>\d+:\d+:\d+)\] \[.*\]: (?<playerName>\w+)\[(?<playerAddress>[^\]]+)\] logged in with entity id \d+ at \((\[[^\]]+\])?(.*?)\)",
        RegexOptions.Compiled)]
    private static partial Regex PlayerServerLoginDetection();

    [GeneratedRegex(@"\[(?<loggingTime>\d+:\d+:\d+)\] \[.*\]: <(?<playerName>\w+)> (?<chatMessage>.+)",
        RegexOptions.Compiled)]
    private static partial Regex PlayerServerChatDetection();

    [GeneratedRegex(
        @"\[(?<loggingTime>\d+:\d+:\d+)\] \[.*\]: (?<playerName>\w+) issued server command: (?<commandMessage>.+)",
        RegexOptions.Compiled)]
    private static partial Regex PlayerServerCommandExecDetection();

    [GeneratedRegex(@"\[(?<loggingTime>\d+:\d+:\d+)\] \[.*\]: (?<playerName>\w+) lost connection: Disconnected",
        RegexOptions.Compiled)]
    private static partial Regex PlayerServerConnectionLostDetection();

    [GeneratedRegex(@"\[(?<loggingTime>\d+:\d+:\d+)\] \[.*\]: (?<playerName>\w+) left the game", RegexOptions.Compiled)]
    private static partial Regex PlayerServerLeftDetection();

    [GeneratedRegex(@"\[(?<loggingTime>\d+:\d+:\d+)\] \[.*\]: (?<playerName>\w+) joined the game",
        RegexOptions.Compiled)]
    private static partial Regex PlayerServerJoinDetection();

    [GeneratedRegex(
        @"\[(?<loggingTime>\d+:\d+:\d+)\] \[.*\]: There are (?<playerCount>\d+) of a max of (?<maxPlayerCount>\d+) players online: (?<playerNames>.+)",
        RegexOptions.Compiled)]
    private static partial Regex PlayerServerListPlayersDetection();
}