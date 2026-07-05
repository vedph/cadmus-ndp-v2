using Cadmus.Core;
using Cadmus.Seed.NdpFrac.Parts;
using System;
using System.Collections.Generic;

namespace Cadmus.NdpFrac.Parts.Test;

public sealed class CodFrRulingsPartTest
{
    private static CodFrRulingsPart GetPart()
    {
        CodFrRulingsPartSeeder seeder = new();
        IItem item = new Item
        {
            FacetId = "default",
            CreatorId = "zeus",
            UserId = "zeus",
            Description = "Test item",
            Title = "Test Item",
            SortKey = ""
        };
        return (CodFrRulingsPart)seeder.GetPart(item, null, null)!;
    }

    private static CodFrRulingsPart GetEmptyPart()
    {
        return new CodFrRulingsPart
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
        CodFrRulingsPart part = GetPart();

        string json = TestHelper.SerializePart(part);
        CodFrRulingsPart part2 =
            TestHelper.DeserializePart<CodFrRulingsPart>(json)!;

        Assert.Equal(part.Id, part2.Id);
        Assert.Equal(part.TypeId, part2.TypeId);
        Assert.Equal(part.ItemId, part2.ItemId);
        Assert.Equal(part.RoleId, part2.RoleId);
        Assert.Equal(part.CreatorId, part2.CreatorId);
        Assert.Equal(part.UserId, part2.UserId);

        Assert.Equal(part.Rulings.Count, part2.Rulings.Count);
    }

    [Fact]
    public void GetDataPins_NoEntries_Ok()
    {
        CodFrRulingsPart part = GetPart();
        part.Rulings.Clear();

        List<DataPin> pins = [.. part.GetDataPins(null)];

        Assert.Single(pins);
        DataPin pin = pins[0];
        Assert.Equal("tot-count", pin.Name);
        TestHelper.AssertPinIds(part, pin);
        Assert.Equal("0", pin.Value);
    }

    [Fact]
    public void GetDataPins_Entries_Ok()
    {
        CodFrRulingsPart part = GetEmptyPart();

        for (int n = 1; n <= 3; n++)
        {
            part.Rulings.Add(new CodFrRuling
            {
                System = n % 2 == 0 ? "latn" : "grek",
                Type = n % 2 == 0 ? "type1" : "type2",
                Features = n % 2 == 0 ? ["dry"] : ["lead"]
            });
        }

        List<DataPin> pins = [.. part.GetDataPins(null)];

        Assert.Equal(7, pins.Count);

        DataPin? pin = pins.Find(p => p.Name == "tot-count");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);
        Assert.Equal("3", pin!.Value);

        // system
        pin = pins.Find(p => p.Name == "system" && p.Value == "latn");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        pin = pins.Find(p => p.Name == "system" && p.Value == "grek");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        // type
        pin = pins.Find(p => p.Name == "type" && p.Value == "type1");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        pin = pins.Find(p => p.Name == "type" && p.Value == "type2");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        // feature
        pin = pins.Find(p => p.Name == "feature" && p.Value == "dry");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        pin = pins.Find(p => p.Name == "feature" && p.Value == "lead");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);
    }
}
