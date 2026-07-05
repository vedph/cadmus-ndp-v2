using Cadmus.Core;
using Cadmus.NdpBooks.Parts;
using Fusi.Tools.Configuration;
using System;
using System.Reflection;
using Xunit;

namespace Cadmus.Seed.NdpBooks.Parts.Test;

public sealed class PrintFontsPartSeederTest
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
        Type t = typeof(PrintFontsPartSeeder);
        TagAttribute? attr = t.GetTypeInfo().GetCustomAttribute<TagAttribute>();
        Assert.NotNull(attr);
        Assert.Equal("seed.it.vedph.ndp.print-fonts", attr!.Tag);
    }

    [Fact]
    public void Seed_Ok()
    {
        PrintFontsPartSeeder seeder = new();
        seeder.SetSeedOptions(_seedOptions);

        IPart? part = seeder.GetPart(_item, null, _factory);

        Assert.NotNull(part);

        PrintFontsPart? p = part as PrintFontsPart;
        Assert.NotNull(p);

        TestHelper.AssertPartMetadata(p!);

        // TODO: assert properties like:
        // Assert.NotEmpty(p!.Entries);
    }
}
