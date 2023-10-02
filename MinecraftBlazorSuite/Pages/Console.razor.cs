
using Microsoft.AspNetCore.Components;
using MinecraftBlazorSuite.Manager;
using MinecraftBlazorSuite.Models.Enums;
using MinecraftBlazorSuite.Services;

namespace MinecraftBlazorSuite.Pages
{
    partial class Console
    {
        [Inject]
        private ServerManagementService minecraftServerService { get; set; }


        private List<(string, LogEntryType)> consoleContent = new();
        private string commandInput = string.Empty;

        private void SendCommand(string cmd = "")
        {
            if (string.IsNullOrEmpty(cmd))
            {
                minecraftServerService.SendCommand(commandInput);
                commandInput = string.Empty;
            }
            else
            {
                minecraftServerService.SendCommand(cmd);
            }
        }

        protected override void OnInitialized()
        {
            minecraftServerService.OnOutputReceived += HandleOutputReceived;
        }

        private void HandleOutputReceived((string consoleLine, LogEntryType entryType) output)
        {
            InvokeAsync(() =>
            {
                if (output.entryType == LogEntryType.None)
                {
                    ProcessServerThreadMessage(output.consoleLine);
                }
                else
                {
                    consoleContent.Add((output.consoleLine, output.entryType));
                }

                TrimConsoleContentIfNeeded();
                StateHasChanged();
            });
        }

        private void ProcessServerThreadMessage(string consoleLine)
        {
            string[] consoleMessageParts = consoleLine.Split("] ");
            if (consoleMessageParts.Length == 2)
            {
                string timestamp = $"<small>{consoleMessageParts[0]}]</small>";
                string messageType = consoleMessageParts[1].Substring(1);

                string message = $"<b>{messageType}</b>";

                consoleContent.Add(($"{timestamp} {message}", LogEntryType.None));
            }
        }

        private void TrimConsoleContentIfNeeded()
        {
            if (consoleContent.Count > Tools.Settings.MaxConsoleMessages)
            {
                consoleContent.RemoveAt(0);
            }
        }

        public void Dispose()
        {
            consoleContent.Clear();
            commandInput = string.Empty;

            minecraftServerService.OnOutputReceived -= HandleOutputReceived;
        }


    }
}
