using Microsoft.AspNetCore.Components;
using Shulkerbox.Services;

namespace Shulkerbox.Shared;

public partial class Layout
{
    [Inject] private LayoutService LayoutService { get; init; }

    private bool IsDrawerOpened { get; set; } = true;

    protected override void OnInitialized()
    {
        LayoutService.StateChanged += (_, _) =>
        {
            if (LayoutService.IsLockDownMode)
                IsDrawerOpened = false;
            StateHasChanged();
        };
    }

    private void ToggleDrawer()
    {
        IsDrawerOpened = !IsDrawerOpened;
    }
}