namespace MinecraftBlazorSuite.Models.Enums;

public enum LogEntryType
{
    JoinUuid,
    Login,
    Chat,
    Command,
    ConnectionLost,
    PlayerLeft,
    PlayerJoined,
    ServerVersion,
    ServerStartupDone,
    ServerReloadDone,
    ServerTypeSponge,
    ServerTypeCraftbukkit,
    PlayerListParsing,
    None
}