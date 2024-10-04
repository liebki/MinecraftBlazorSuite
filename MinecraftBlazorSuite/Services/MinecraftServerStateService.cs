using System.Diagnostics;
using MinecraftBlazorSuite.Models.Enums;

namespace MinecraftBlazorSuite.Manager;

public class MinecraftServerStateService
{
    private ServerStates _currentServerState = ServerStates.Offline;

    public ServerType Type { get; set; } = ServerType.Vanilla;

    private Stopwatch ServerUptime = new();
    
    public bool AreControlsLocked { get; set; } = true;

    public ServerStates GetCurrentServerState()
    {
        return _currentServerState;
    }
    
    public string GetCurrentUptime()
    {
        TimeSpan tSpan = ServerUptime.Elapsed;
        string uptimeString = $"{tSpan.Hours:D2}:{tSpan.Minutes:D2}";
        
        return uptimeString;
    }
    
    public void SetServerState(ServerStates state)
    {
        if (_currentServerState == state)
            return;

        switch (state)
        {
            case ServerStates.Online:
                ServerUptime.Start();
                break;
            
            case ServerStates.Offline:
                ServerUptime = new Stopwatch();
                break;
        }

        _currentServerState = state;
    }
    
}