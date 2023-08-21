using System.Runtime.InteropServices;

namespace Shulkerbox;

public static class Utilities
{
    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetPhysicallyInstalledSystemMemory(out long TotalMemoryInKilobytes);

    public static int GetTotalSystemMemory()
    {
        GetPhysicallyInstalledSystemMemory(out var memoryKb);
        return (int)(memoryKb / 1024);
    }
}