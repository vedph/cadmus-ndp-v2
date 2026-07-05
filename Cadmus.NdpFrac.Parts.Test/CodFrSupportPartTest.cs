using Cadmus.Core;
using Cadmus.Seed.NdpFrac.Parts;
using System;
using System.Collections.Generic;

namespace Cadmus.NdpFrac.Parts.Test;

public sealed class CodFrSupportPartTest
{
    private static CodFrSupportPart GetPart()
    {
        CodFrSupportPartSeeder seeder = new();
        IItem item = new Item
        {
            FacetId = "default",
            CreatorId = "zeus",
            UserId = "zeus",
            Description = "Test item",
            Title = "Test Item",
            SortKey = ""
        };
        return (CodFrSupportPart)seeder.GetPart(item, null, null)!;
    }

    private static CodFrSupportPart GetEmptyPart()
    {
        return new CodFrSupportPart
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
        CodFrSupportPart part = GetPart();

        string json = TestHelper.SerializePart(part);
        CodFrSupportPart part2 = TestHelper.DeserializePart<CodFrSupportPart>(json)!;

        Assert.Equal(part.Id, part2.Id);
        Assert.Equal(part.TypeId, part2.TypeId);
        Assert.Equal(part.ItemId, part2.ItemId);
        Assert.Equal(part.RoleId, part2.RoleId);
        Assert.Equal(part.CreatorId, part2.CreatorId);
        Assert.Equal(part.UserId, part2.UserId);
    }

    [Fact]
    public void GetDataPins_NoTag_Empty()
    {
        CodFrSupportPart part = GetEmptyPart();

        Assert.Empty(part.GetDataPins());
    }

    [Fact]
    public void GetDataPins_Tag_1()
    {
        CodFrSupportPart part = GetEmptyPart();
        part.Material = "parchment";
        part.Location = "A1";
        part.Container = "BAV";

        List<DataPin> pins = [.. part.GetDataPins(null)];
        Assert.Equal(3, pins.Count);

        // material
        DataPin? pin = pins.Find(p => p.Name == "material" && p.Value == "parchment");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        // location
        pin = pins.Find(p => p.Name == "location" && p.Value == "A1");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        // container
        pin = pins.Find(p => p.Name == "container" && p.Value == "BAV");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);
    }
}
