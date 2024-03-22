using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Shulkerbox.Shared;

public partial class Layout
{
    [Inject] private IJSRuntime JavaScriptRuntime { get; init; }

    private async Task NavigateBack()
    {
        await JavaScriptRuntime.InvokeVoidAsync("history.back");
    }

    private void OpenGitHub()
    {
        ShulkUtils.ExecuteShell("https://github.com/dentolos19/Shulkerbox");
    }
}