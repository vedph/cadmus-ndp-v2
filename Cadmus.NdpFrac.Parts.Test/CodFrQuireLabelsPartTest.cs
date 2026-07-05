using Cadmus.Core;
using Cadmus.Refs.Bricks;
using Cadmus.Seed.NdpFrac.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Cadmus.NdpFrac.Parts.Test;

public sealed class CodFrQuireLabelsPartTest
{
    private static CodFrQuireLabelsPart GetPart()
    {
        CodFrQuireLabelsPartSeeder seeder = new();
        IItem item = new Item
        {
            FacetId = "default",
            CreatorId = "zeus",
            UserId = "zeus",
            Description = "Test item",
            Title = "Test Item",
            SortKey = ""
        };
        return (CodFrQuireLabelsPart)seeder.GetPart(item, null, null)!;
    }

    private static CodFrQuireLabelsPart GetEmptyPart()
    {
        return new CodFrQuireLabelsPart
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
        CodFrQuireLabelsPart part = GetPart();

        string json = TestHelper.SerializePart(part);
        CodFrQuireLabelsPart part2 =
            TestHelper.DeserializePart<CodFrQuireLabelsPart>(json)!;

        Assert.Equal(part.Id, part2.Id);
        Assert.Equal(part.TypeId, part2.TypeId);
        Assert.Equal(part.ItemId, part2.ItemId);
        Assert.Equal(part.RoleId, part2.RoleId);
        Assert.Equal(part.CreatorId, part2.CreatorId);
        Assert.Equal(part.UserId, part2.UserId);

        Assert.Equal(part.Labels.Count, part2.Labels.Count);
    }

    [Fact]
    public void GetDataPins_NoEntries_Ok()
    {
        CodFrQuireLabelsPart part = GetPart();
        part.Labels.Clear();

        List<DataPin> pins = part.GetDataPins(null).ToList();

        Assert.Single(pins);
        DataPin pin = pins[0];
        Assert.Equal("tot-count", pin.Name);
        TestHelper.AssertPinIds(part, pin);
        Assert.Equal("0", pin.Value);
    }

    [Fact]
    public void GetDataPins_Entries_Ok()
    {
        CodFrQuireLabelsPart part = GetEmptyPart();

        for (int n = 1; n <= 3; n++)
        {
            CodFrQuireLabel label = new()
            {
                Positions = (n & 1) == 1 ? ["mrg-top"] : ["mrg-bottom"],
                Types = ["latn"],
                HandId = new AssertedCompositeId
                {
                    Target = new()
                    {
                        Gid = $"hand-{n}",
                    }
                }
            };
            part.Labels.Add(label);
        }

        List<DataPin> pins = [.. part.GetDataPins(null)];

        Assert.Equal(7, pins.Count);

        DataPin? pin = pins.Find(p => p.Name == "tot-count");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);
        Assert.Equal("3", pin!.Value);

        // position=mrg-top
        pin = pins.Find(p => p.Name == "position" && p.Value == "mrg-top");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        // position=mrg-bottom
        pin = pins.Find(p => p.Name == "position" && p.Value == "mrg-bottom");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        // hand
        for (int n = 1; n <= 3; n++)
        {
            pin = pins.Find(p => p.Name == "hand-id" && p.Value == $"hand-{n}");
            Assert.NotNull(pin);
            TestHelper.AssertPinIds(part, pin!);
        }

        // type
        pin = pins.Find(p => p.Name == "type");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);
        Assert.Equal("latn", pin.Value);
    }
}
