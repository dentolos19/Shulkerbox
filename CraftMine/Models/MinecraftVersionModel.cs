using CmlLib.Core.Version;

namespace CraftMine.Models;

public class MinecraftVersionModel
{

    public string Name { get; }
    public MVersionMetadata Metadata { get; }

    public MinecraftVersionModel(MVersionMetadata metadata)
    {
        Name = metadata.Name;
        Metadata = metadata;
    }
}