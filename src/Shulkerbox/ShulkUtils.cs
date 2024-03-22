using System.Diagnostics;

namespace Shulkerbox;

public static class ShulkUtils
{
    public static void ExecuteShell(string fileName)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = fileName,
            UseShellExecute = true
        });
    }
}