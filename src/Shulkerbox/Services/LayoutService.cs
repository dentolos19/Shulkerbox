namespace Shulkerbox.Services;

public class LayoutService
{
    public bool IsLockdownMode { get; set; }
    public event EventHandler? StateChanged;

    public void TriggerStateChanged()
    {
        StateChanged?.Invoke(this, EventArgs.Empty);
    }
}