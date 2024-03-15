using CmlLib.Core.VersionMetadata;

namespace Shulkerbox.Shared.Models;

public class VersionModel(MVersionMetadata version)
{
    public string Name { get; } = version.Name;
    public MVersionMetadata Version { get; } = version;

    public override bool Equals(object? obj)
    {
        return
            obj is VersionModel model &&
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