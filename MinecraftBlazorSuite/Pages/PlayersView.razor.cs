using Microsoft.AspNetCore.Components;
using MinecraftBlazorSuite.Dialog;
using MinecraftBlazorSuite.Models.Data;
using MinecraftBlazorSuite.Models.Enums;
using MinecraftBlazorSuite.Services;
using MudBlazor;

namespace MinecraftBlazorSuite.Pages;

partial class PlayersView
{
    [Inject] private ServerManagementService MinecraftServerService { get; set; }

    [Inject] private IDialogService DialogService { get; set; }

    private void SendCommand(string cmd)
    {
        MinecraftServerService.SendCommand(cmd);
    }

    private void ChangeOperatorStatus(string playername, bool giveRights)
    {
        SendCommand(giveRights
            ? string.Format(MinecraftCommands.OpCommand, playername)
            : string.Format(MinecraftCommands.DeopCommand, playername));
    }

    private Task KillPlayer(string playername)
    {
        SendCommand(string.Format(MinecraftCommands.KillCommand, playername));
        return Task.CompletedTask;
    }

    private async Task KickWithMessage(string playername)
    {
        IDialogReference dialog =
            await DialogService.ShowAsync<PlayerMessageInputDialog>($"Kick player {playername}:");
        DialogResult? result = await dialog.Result;

        if (result is { Canceled: false })
            SendCommand(string.Format(MinecraftCommands.KickCommand, playername, result.Data));
    }

    private async Task BanWithMessage(string playername)
    {
        IDialogReference dialog =
            await DialogService.ShowAsync<PlayerMessageInputDialog>($"Ban player {playername}:");
        DialogResult? result = await dialog.Result;

        if (result is { Canceled: false })
            SendCommand(string.Format(MinecraftCommands.BanCommand, playername, result.Data));
    }

    private async Task SendMessage(string playername)
    {
        IDialogReference dialog =
            await DialogService.ShowAsync<PlayerMessageInputDialog>($"Send {playername} a message:");
        DialogResult? result = await dialog.Result;

        if (result is { Canceled: false })
            SendCommand(string.Format(MinecraftCommands.MsgCommand, playername, result.Data));
    }

    private Task HealPlayer(string playername)
    {
        SendCommand(string.Format(MinecraftCommands.HealCommand, playername));
        return Task.CompletedTask;
    }


    private async Task ChangeGamemode(string playername)
    {
        IDialogReference dialog =
            await DialogService.ShowAsync<PlayerChangeGamemodeDialog>($"Change {playername}'s gamemode to:");
        DialogResult? result = await dialog.Result;

        if (result is { Canceled: false })
        {
            MinecraftGamemodes gamemode = (MinecraftGamemodes)result.Data;
            SendCommand(string.Format(MinecraftCommands.GamemodeCommand, gamemode.ToString().ToLower(), playername));
        }
    }

    protected override void OnInitialized()
    {
        MinecraftServerService.OnPlayerJoinLeave += HandleOutputReceived;
    }

    private void HandleOutputReceived()
    {
        InvokeAsync(StateHasChanged);
    }
}