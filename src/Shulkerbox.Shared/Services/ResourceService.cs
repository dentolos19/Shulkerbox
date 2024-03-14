using System.Reflection;

namespace Shulkerbox.Shared.Services;

public sealed class ResourceService
{

    public async Task<string> GetResourceStringAsync(string resourceName)
    {
        await using var stream =
            Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream($"Shulkerbox.Shared.Resources.{resourceName}");
        using var reader = new StreamReader(stream!);
        return await reader.ReadToEndAsync();
    }

    public static ResourceService Initialize()
    {
        return new ResourceService();
    }
}