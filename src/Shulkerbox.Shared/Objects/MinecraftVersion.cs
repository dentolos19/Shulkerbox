using CmlLib.Core.VersionMetadata;

namespace Shulkerbox.Shared.Objects;

public class MinecraftVersion(MVersionMetadata version)
{
    public string Name { get; } = version.Name;
    public MVersionMetadata Version { get; } = version;

    public override bool Equals(object? obj)
    {
        return
            obj is MinecraftVersion model &&
            Name == model.Name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name);
    }

    public override string ToString()
    {
        return Name;
    }
}