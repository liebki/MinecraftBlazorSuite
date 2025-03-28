using MinecraftBlazorSuite.Models.LogEntries;

namespace MinecraftBlazorSuite.Services;

public class PlayerManager
{
    public List<LoginLogEntry> Players { get; private set; } = [];
    public event Action? OnPlayerListChanged;

    public void AddPlayer(LoginLogEntry player)
    {
        Players.Add(player);
        OnPlayerListChanged?.Invoke();
    }

    public void UpdatePlayerIP(string playerName, string ipAddress)
    {
        var player = Players.FirstOrDefault(p =>
            p.PlayerName.Equals(playerName, StringComparison.InvariantCultureIgnoreCase));

        if (player != null)
        {
            player.IPAddress = ipAddress;
            OnPlayerListChanged?.Invoke();
        }
    }

    public void RemovePlayer(string playerName)
    {
        Players.RemoveAll(p =>
            p.PlayerName.Equals(playerName, StringComparison.InvariantCultureIgnoreCase));
        OnPlayerListChanged?.Invoke();
    }

    public void ClearPlayers()
    {
        Players.Clear();
        OnPlayerListChanged?.Invoke();
    }

    public void HandleDuplicatePlayers(string playerName)
    {
        var matchingPlayers = Players.Where(p =>
            p.PlayerName.Equals(playerName, StringComparison.InvariantCultureIgnoreCase)).ToList();

        if (matchingPlayers.Count > 1)
        {
            var playerToKeep = matchingPlayers.First();
            Players = Players.Where(p =>
                    p != playerToKeep &&
                    !p.PlayerName.Equals(playerName, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
            Players.Add(playerToKeep);
            OnPlayerListChanged?.Invoke();
        }
    }
}