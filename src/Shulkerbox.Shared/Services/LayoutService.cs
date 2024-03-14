namespace Shulkerbox.Shared.Services;

public sealed class LayoutService
{
    public bool IsLockDownMode { get; set; }
    public event EventHandler? StateChanged;

    public void TriggerStateChanged()
    {
        StateChanged?.Invoke(this, EventArgs.Empty);
    }

    public static LayoutService Initialize()
    {
        return new LayoutService();
    }
}