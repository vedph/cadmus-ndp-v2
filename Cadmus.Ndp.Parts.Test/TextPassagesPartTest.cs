using Cadmus.Core;
using Cadmus.Seed.Ndp.Parts;
using System;
using System.Collections.Generic;

namespace Cadmus.Ndp.Parts.Test;

public sealed class TextPassagesPartTest
{
    private static TextPassagesPart GetPart()
    {
        TextPassagesPartSeeder seeder = new();
        IItem item = new Item
        {
            FacetId = "default",
            CreatorId = "zeus",
            UserId = "zeus",
            Description = "Test item",
            Title = "Test Item",
            SortKey = ""
        };
        return (TextPassagesPart)seeder.GetPart(item, null, null)!;
    }

    private static TextPassagesPart GetEmptyPart()
    {
        return new TextPassagesPart
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
        TextPassagesPart part = GetPart();

        string json = TestHelper.SerializePart(part);
        TextPassagesPart part2 =
            TestHelper.DeserializePart<TextPassagesPart>(json)!;

        Assert.Equal(part.Id, part2.Id);
        Assert.Equal(part.TypeId, part2.TypeId);
        Assert.Equal(part.ItemId, part2.ItemId);
        Assert.Equal(part.RoleId, part2.RoleId);
        Assert.Equal(part.CreatorId, part2.CreatorId);
        Assert.Equal(part.UserId, part2.UserId);

        Assert.Equal(part.Passages.Count, part2.Passages.Count);
    }

    [Fact]
    public void GetDataPins_NoEntries_Ok()
    {
        TextPassagesPart part = GetPart();
        part.Passages.Clear();

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
        TextPassagesPart part = GetEmptyPart();

        for (int n = 1; n <= 3; n++)
        {
            part.Passages.Add(new TextPassage
            {
                Citation = $"Il.{n}"
            });
        }

        List<DataPin> pins = [.. part.GetDataPins(null)];

        Assert.Equal(4, pins.Count);

        DataPin? pin = pins.Find(p => p.Name == "tot-count");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);
        Assert.Equal("3", pin!.Value);

        for (int n = 1; n <= 3; n++)
        {
            pin = pins.Find(p => p.Name == "citation" && p.Value == $"Il.{n}");
            Assert.NotNull(pin);
            TestHelper.AssertPinIds(part, pin!);
        }
    }
}
