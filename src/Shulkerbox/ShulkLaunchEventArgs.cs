using CmlLib.Core.Downloader;

namespace Shulkerbox;

public class ShulkLaunchEventArgs : EventArgs
{
    public string? FileName { get; internal set; }
    public MFile FileKind { get; internal set; }
    public int ProgressedFileCount { get; internal set; }
    public int TotalFileCount { get; internal set; }

    public int OverallProgressPercentage { get; internal set; }

    public int FileProgressPercentage => ProgressedFileCount / TotalFileCount * 100;
}