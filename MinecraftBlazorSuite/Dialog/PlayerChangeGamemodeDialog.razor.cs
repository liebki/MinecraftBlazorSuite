using Microsoft.AspNetCore.Components;
using MinecraftBlazorSuite.Models.Enums;
using MudBlazor;

namespace MinecraftBlazorSuite.Dialog;

partial class PlayerChangeGamemodeDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }

    public MinecraftGamemodes SelectedGamemode { get; set; }

    private void Submit()
    {
        MudDialog.Close(DialogResult.Ok(SelectedGamemode));
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}