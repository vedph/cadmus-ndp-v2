using Cadmus.Core;
using Cadmus.NdpFrac.Parts;
using Fusi.Tools.Configuration;
using System;
using System.Reflection;
using Xunit;

namespace Cadmus.Seed.NdpFrac.Parts.Test;

public sealed class CodFrQuireLabelsPartSeederTest
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
        Type t = typeof(CodFrQuireLabelsPartSeeder);
        TagAttribute? attr = t.GetTypeInfo().GetCustomAttribute<TagAttribute>();
        Assert.NotNull(attr);
        Assert.Equal("seed.it.vedph.ndp.cod-fr-quire-labels", attr!.Tag);
    }

    [Fact]
    public void Seed_Ok()
    {
        CodFrQuireLabelsPartSeeder seeder = new();
        seeder.SetSeedOptions(_seedOptions);

        IPart? part = seeder.GetPart(_item, null, _factory);

        Assert.NotNull(part);

        CodFrQuireLabelsPart? p = part as CodFrQuireLabelsPart;
        Assert.NotNull(p);

        TestHelper.AssertPartMetadata(p!);

        Assert.NotEmpty(p!.Labels);
    }
}
