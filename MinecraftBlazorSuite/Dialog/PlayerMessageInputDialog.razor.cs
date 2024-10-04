using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MinecraftBlazorSuite.Dialog;

partial class PlayerMessageInputDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }

    public string MessageContent = string.Empty;

    private void Submit()
    {
        MudDialog.Close(DialogResult.Ok(MessageContent));
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}