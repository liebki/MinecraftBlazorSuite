using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MinecraftBlazorSuite.Dialog;

partial class PlayerMessageInputDialog
{
    public string MessageContent = string.Empty;
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }

    private void Submit()
    {
        MudDialog.Close(DialogResult.Ok(MessageContent));
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}