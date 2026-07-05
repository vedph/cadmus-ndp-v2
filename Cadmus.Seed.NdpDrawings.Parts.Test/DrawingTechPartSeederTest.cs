using Cadmus.Core;
using Cadmus.NdpDrawings.Parts;
using Fusi.Tools.Configuration;
using System.Reflection;

namespace Cadmus.Seed.NdpDrawings.Parts.Test;

public sealed class DrawingTechPartSeederTest
{
    private static readonly PartSeederFactory _factory =
        TestHelper.GetFactory();
    private static readonly SeedOptions _seedOptions =
        _factory.GetSeedOptions();
    private static readonly IItem _item =
        _factory.GetItemSeeder().GetItem(1, "facet");

    [Fact]
    public void TypeHasTagAttribute()
    {
        Type t = typeof(DrawingTechPartSeeder);
        TagAttribute attr = t.GetTypeInfo().GetCustomAttribute<TagAttribute>();
        Assert.NotNull(attr);
        Assert.Equal("seed.it.vedph.ndp.drawing-tech", attr!.Tag);
    }

    [Fact]
    public void Seed_Ok()
    {
        DrawingTechPartSeeder seeder = new();
        seeder.SetSeedOptions(_seedOptions);

        IPart part = seeder.GetPart(_item, null, _factory);

        Assert.NotNull(part);

        DrawingTechPart p = part as DrawingTechPart;
        Assert.NotNull(p);

        TestHelper.AssertPartMetadata(p!);

        Assert.NotEmpty(p!.Material);
    }
}
