using CmlLib.Core.VersionMetadata;

namespace Shulkerbox.Shared.Models;

public class VersionModel
{
    public string Name { get; }
    public MVersionMetadata Version { get; }

    public VersionModel(MVersionMetadata version)
    {
        Name = version.Name;
        Version = version;
    }

    public override bool Equals(object? @object)
    {
        return @object is VersionModel model && Name == model.Name;
    }

    public override string ToString()
    {
        return Name;
    }
}