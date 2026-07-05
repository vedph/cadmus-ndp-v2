using System;
using Cadmus.Core;
using System.Collections.Generic;
using Cadmus.Seed.NdpFrac.Parts;
using Cadmus.Mat.Bricks;
using Cadmus.Refs.Bricks;

namespace Cadmus.NdpFrac.Parts.Test;

public sealed class CodFrLayoutPartTest
{
    private static CodFrLayoutPart GetPart()
    {
        CodFrLayoutPartSeeder seeder = new();
        IItem item = new Item
        {
            FacetId = "default",
            CreatorId = "zeus",
            UserId = "zeus",
            Description = "Test item",
            Title = "Test Item",
            SortKey = ""
        };
        return (CodFrLayoutPart)seeder.GetPart(item, null, null)!;
    }

    private static CodFrLayoutPart GetEmptyPart()
    {
        return new CodFrLayoutPart
        {
            ItemId = Guid.NewGuid().ToString(),
            RoleId = "some-role",
            CreatorId = "zeus",
            UserId = "another",
        };
    }

    [Fact]
    public void Part_Is_Serializable()
    {
        CodFrLayoutPart part = GetPart();

        string json = TestHelper.SerializePart(part);
        CodFrLayoutPart part2 = TestHelper.DeserializePart<CodFrLayoutPart>(json)!;

        Assert.Equal(part.Id, part2.Id);
        Assert.Equal(part.TypeId, part2.TypeId);
        Assert.Equal(part.ItemId, part2.ItemId);
        Assert.Equal(part.RoleId, part2.RoleId);
        Assert.Equal(part.CreatorId, part2.CreatorId);
        Assert.Equal(part.UserId, part2.UserId);
    }

    [Fact]
    public void GetDataPins_AllPins()
    {
        CodFrLayoutPart part = GetEmptyPart();
        part.Formula = "formula";
        part.Dimensions.Add(new PhysicalDimension
        {
            Tag = "width",
            Value = 100,
            Unit = "mm"
        });
        part.Pricking = "pricking";
        part.ColumnCount = 2;
        part.Counts.Add(new DecoratedCount
        {
            Id = "column",
            Value = 2
        });

        List<DataPin> pins = [.. part.GetDataPins(null)];
        Assert.Equal(5, pins.Count);

        // formula
        DataPin? pin = pins.Find(p => p.Name == "formula" && p.Value == "formula");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        // d-width
        pin = pins.Find(p => p.Name == "d-width" && p.Value == "100");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        // pricking
        pin = pins.Find(p => p.Name == "pricking" && p.Value == "pricking");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        // column-count
        pin = pins.Find(p => p.Name == "column-count" && p.Value == "2");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        // c-column
        pin = pins.Find(p => p.Name == "c-column" && p.Value == "2");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);
    }
}