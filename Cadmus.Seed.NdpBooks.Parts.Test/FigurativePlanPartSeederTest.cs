using Cadmus.Core;
using Cadmus.NdpBooks.Parts;
using Fusi.Tools.Configuration;
using System;
using System.Reflection;
using Xunit;

namespace Cadmus.Seed.NdpBooks.Parts.Test;

public sealed class FigurativePlanPartSeederTest
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
        Type t = typeof(FigurativePlanPartSeeder);
        TagAttribute? attr = t.GetTypeInfo().GetCustomAttribute<TagAttribute>();
        Assert.NotNull(attr);
        Assert.Equal("seed.it.vedph.ndp.print-fig-plan", attr!.Tag);
    }

    [Fact]
    public void Seed_Ok()
    {
        FigurativePlanPartSeeder seeder = new();
        seeder.SetSeedOptions(_seedOptions);

        IPart? part = seeder.GetPart(_item, null, _factory);

        Assert.NotNull(part);

        FigurativePlanPart? p = part as FigurativePlanPart;
        Assert.NotNull(p);

        TestHelper.AssertPartMetadata(p!);

        Assert.NotEmpty(p.Techniques);

        Assert.NotNull(p.Items);
        Assert.NotEmpty(p.Items);

        Assert.NotNull(p.Description);
    }
}
