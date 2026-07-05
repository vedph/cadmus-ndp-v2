using Cadmus.Core;
using Cadmus.Seed.NdpBooks.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Cadmus.NdpBooks.Parts.Test;

public sealed class PrintFontsPartTest
{
    private static PrintFontsPart GetPart()
    {
        PrintFontsPartSeeder seeder = new();
        IItem item = new Item
        {
            FacetId = "default",
            CreatorId = "zeus",
            UserId = "zeus",
            Description = "Test item",
            Title = "Test Item",
            SortKey = ""
        };
        return (PrintFontsPart)seeder.GetPart(item, null, null)!;
    }

    private static PrintFontsPart GetEmptyPart()
    {
        return new PrintFontsPart
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
        PrintFontsPart part = GetPart();

        string json = TestHelper.SerializePart(part);
        PrintFontsPart part2 =
            TestHelper.DeserializePart<PrintFontsPart>(json)!;

        Assert.Equal(part.Id, part2.Id);
        Assert.Equal(part.TypeId, part2.TypeId);
        Assert.Equal(part.ItemId, part2.ItemId);
        Assert.Equal(part.RoleId, part2.RoleId);
        Assert.Equal(part.CreatorId, part2.CreatorId);
        Assert.Equal(part.UserId, part2.UserId);

        Assert.Equal(part.Fonts.Count, part2.Fonts.Count);
    }

    [Fact]
    public void GetDataPins_NoEntries_Ok()
    {
        PrintFontsPart part = GetPart();
        part.Fonts.Clear();

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
        PrintFontsPart part = GetEmptyPart();

        for (int n = 1; n <= 3; n++)
        {
            part.Fonts.Add(new()
            {
                Eid = $"font{n}",
                Family = n % 2 == 0 ? "R4" : "R5",
                Sections = n % 2 == 0 ? ["title", "body"] : ["comment"],
                Features = n % 2 == 0 ? ["uppercase"] : ["lowercase"]
            });
        }

        List<DataPin> pins = [.. part.GetDataPins(null)];

        Assert.Equal(11, pins.Count);

        DataPin? pin = pins.Find(p => p.Name == "tot-count");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);
        Assert.Equal("3", pin!.Value);

        // eid
        for (int n = 1; n <= 3; n++)
        {
            pin = pins.Find(p => p.Name == "eid" && p.Value == $"font{n}");
            Assert.NotNull(pin);
            TestHelper.AssertPinIds(part, pin!);
        }

        // family R4
        pin = pins.Find(p => p.Name == "family" && p.Value == "R4");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        // family R5
        pin = pins.Find(p => p.Name == "family" && p.Value == "R5");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        // section title
        pin = pins.Find(p => p.Name == "section" && p.Value == "title");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        // section body
        pin = pins.Find(p => p.Name == "section" && p.Value == "body");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        // section comment
        pin = pins.Find(p => p.Name == "section" && p.Value == "comment");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        // feature uppercase
        pin = pins.Find(p => p.Name == "feature" && p.Value == "uppercase");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        // feature lowercase
        pin = pins.Find(p => p.Name == "feature" && p.Value == "lowercase");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);
    }
}
