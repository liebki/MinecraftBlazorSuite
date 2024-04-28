using Microsoft.AspNetCore.Components;
using MinecraftBlazorSuite.Dialog;
using MinecraftBlazorSuite.Models.Data;
using MinecraftBlazorSuite.Models.Enums;
using MinecraftBlazorSuite.Services;
using MudBlazor;

namespace MinecraftBlazorSuite.Pages;

partial class PlayersView
{
    [Inject] private ServerManagementService minecraftServerService { get; set; }

    [Inject] private IDialogService DialogService { get; set; }

    private void SendCommand(string cmd)
    {
        minecraftServerService.SendCommand(cmd);
    }

    private async Task ChangeOperatorStatus(string playername, bool giveRights)
    {
        SendCommand(giveRights
            ? string.Format(MinecraftCommands.OpCommand, playername)
            : string.Format(MinecraftCommands.DeopCommand, playername));
    }

    private async Task KillPlayer(string playername)
    {
        SendCommand(string.Format(MinecraftCommands.KillCommand, playername));
    }

    private async Task KickWithMessage(string playername)
    {
        IDialogReference dialog =
            await DialogService.ShowAsync<PlayerMessageInputDialog>($"Kick player {playername} because:");
        DialogResult result = await dialog.Result;

        if (!result.Canceled) SendCommand(string.Format(MinecraftCommands.KickCommand, playername, result.Data));
    }

    private async Task BanWithMessage(string playername)
    {
        IDialogReference dialog =
            await DialogService.ShowAsync<PlayerMessageInputDialog>($"Ban player {playername} because:");
        DialogResult result = await dialog.Result;

        if (!result.Canceled) SendCommand(string.Format(MinecraftCommands.BanCommand, playername, result.Data));
    }

    private async Task SendMessage(string playername)
    {
        IDialogReference dialog =
            await DialogService.ShowAsync<PlayerMessageInputDialog>($"Send message to {playername}");
        DialogResult result = await dialog.Result;

        if (!result.Canceled) SendCommand(string.Format(MinecraftCommands.MsgCommand, playername, result.Data));
    }

    private async Task ChangeGamemode(string playername)
    {
        IDialogReference dialog =
            await DialogService.ShowAsync<PlayerChangeGamemodeDialog>($"Change the gamemode of {playername}");
        DialogResult result = await dialog.Result;

        if (!result.Canceled)
        {
            MinecraftGamemodes gamemode = (MinecraftGamemodes)result.Data;
            SendCommand(string.Format(MinecraftCommands.GamemodeCommand, gamemode.ToString().ToLower(), playername));
        }
    }

    protected override void OnInitialized()
    {
        minecraftServerService.OnPlayerJoinLeave += HandleOutputReceived;
    }

    private void HandleOutputReceived(bool PlayerJoined)
    {
        InvokeAsync(StateHasChanged);
    }
}