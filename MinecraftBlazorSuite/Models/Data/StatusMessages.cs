namespace MinecraftBlazorSuite.Models.Data;

public static class StatusMessages
{
    public const string StatusMessageFinishedStartup = "[{0}] The server is done starting, it took {1} seconds!";
    public const string StatusMessageFinishedReload = "[{0}] The server is done reloading!";
    public const string StatusMessageServerShutdown = "The server is shutting down!";
    public const string StatusMessageServerIsOffline = "The server is already offline, can't shut down!";
    public const string StatusMessageServerIsBootingUp = "The server is now booting up, please wait..";
}