
using MinecraftBlazorSuite.Manager;
using MinecraftBlazorSuite.Models.Enums;
using MinecraftBlazorSuite.Models.LogEntries;
using System;
using System.Diagnostics;

namespace MinecraftBlazorSuite.Services
{
    public class ServerManagementService : IDisposable
    {
        public event Action<(string, LogEntryType)>? OnOutputReceived;
        public event Action<bool>? OnPlayerJoinLeave;
        public event Action<ChatLogEntry>? OnPlayerChat;
        public event Action<CommandLogEntry>? OnPlayerExecuteCommand;

        public List<LoginLogEntry> Spielerliste { get; set; } = new();

        private Process? minecraftServerProcess;
        private readonly object processLock = new();

        public ServerManagementService()
        {
            if (minecraftServerProcess != null)
                return;

            string? jarDirectory = Path.GetDirectoryName(Tools.Settings.ServerJarPath);
            minecraftServerProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "java",
                    Arguments = $"-jar -Djline.terminal=jline.UnsupportedTerminal {Tools.Settings.JavaArguments} \"{Tools.Settings.ServerJarPath}\" nogui",
                    WorkingDirectory = jarDirectory,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true
                }
            };

            minecraftServerProcess.OutputDataReceived += OnOutputDataReceived;

            minecraftServerProcess.Start();
            minecraftServerProcess.BeginOutputReadLine();
        }

        private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                (LogEntryType entryType, dynamic entryData) = LogParser.ParseLogEntry(e.Data);
                switch (entryType)
                {
                    case LogEntryType.JoinUUID:

                        (string playerName, string playerUUID, string joinTime) basePlayerData = entryData;
                        LoginLogEntry neuerSpieler = new(basePlayerData.joinTime, basePlayerData.playerName, string.Empty, basePlayerData.playerUUID);

                        Spielerliste.Add(neuerSpieler);
                        OnPlayerJoinLeave?.Invoke(true);

                        break;

                    case LogEntryType.Login:

                        (string PlayerName, string IPAddress) extendedPlayerData = entryData;
                        LoginLogEntry UpdatePlayerData = Spielerliste.SingleOrDefault(pl => pl.PlayerName.Equals(extendedPlayerData.PlayerName, StringComparison.InvariantCultureIgnoreCase));

                        if (UpdatePlayerData != null)
                        {
                            UpdatePlayerData.IPAddress = extendedPlayerData.IPAddress;
                        }

                        OnPlayerJoinLeave?.Invoke(true);
                        break;

                    case LogEntryType.PlayerLeft:

                        PlayerLeftLogEntry playerLeftEntry = entryData;
                        Spielerliste.RemoveAll(x => x.PlayerName.Equals(playerLeftEntry.PlayerName, StringComparison.InvariantCultureIgnoreCase));

                        OnPlayerJoinLeave?.Invoke(false);

                        break;

                    case LogEntryType.Chat:

                        ChatLogEntry playerChatEntry = entryData;
                        OnPlayerChat?.Invoke(playerChatEntry);

                        break;

                    case LogEntryType.Command:

                        CommandLogEntry playerCommandEntry = entryData;
                        OnPlayerExecuteCommand?.Invoke(playerCommandEntry);

                        break;

                    default:
                        break;
                }

                OnOutputReceived?.Invoke((e.Data, entryType));
            }
        }

        public void SendCommand(string command)
        {
            lock (processLock)
            {
                if (minecraftServerProcess != null && !minecraftServerProcess.HasExited)
                {
                    minecraftServerProcess.StandardInput.WriteLine(command);
                }
            }
        }

        public void Stop()
        {
            lock (processLock)
            {
                if (minecraftServerProcess != null && !minecraftServerProcess.HasExited)
                {
                    minecraftServerProcess.StandardInput.WriteLine("stop");
                    minecraftServerProcess.WaitForExit();

                    minecraftServerProcess.Dispose();
                    minecraftServerProcess = null;
                }
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
