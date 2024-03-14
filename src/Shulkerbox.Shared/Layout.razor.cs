using Microsoft.AspNetCore.Components;
using Shulkerbox.Shared.Services;

namespace Shulkerbox.Shared;

public partial class Layout
{
    [Inject] private LayoutService LayoutService { get; init; }

    private bool IsDrawerOpen { get; set; } = true;

    protected override void OnInitialized()
    {
        LayoutService.StateChanged += (_, _) =>
        {
            if (LayoutService.IsLockDownMode)
                IsDrawerOpen = false;
            StateHasChanged();
        };
    }

    private void ToggleDrawer()
    {
        IsDrawerOpen = !IsDrawerOpen;
    }
}