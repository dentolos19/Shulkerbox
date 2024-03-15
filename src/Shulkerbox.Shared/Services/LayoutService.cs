namespace Shulkerbox.Shared.Services;

public class LayoutService
{
    public bool IsDebugMode { get; set; }
    public bool IsLockDownMode { get; set; }
    public event EventHandler? StateChanged;

    public void TriggerStateChanged()
    {
        StateChanged?.Invoke(this, EventArgs.Empty);
    }

    public static LayoutService Initialize()
    {
        var service = new LayoutService();
#if DEBUG
        service.IsDebugMode = true;
#endif
        return service;
    }
}