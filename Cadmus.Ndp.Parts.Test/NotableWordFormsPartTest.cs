using Cadmus.Core;
using Cadmus.Ndp.Parts;
using Cadmus.Seed.Ndp.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Cadmus.Ndp.Parts.Test;

public sealed class NotableWordFormsPartTest
{
    private static NotableWordFormsPart GetPart()
    {
        NotableWordFormsPartSeeder seeder = new();
        IItem item = new Item
        {
            FacetId = "default",
            CreatorId = "zeus",
            UserId = "zeus",
            Description = "Test item",
            Title = "Test Item",
            SortKey = ""
        };
        return (NotableWordFormsPart)seeder.GetPart(item, null, null)!;
    }

    private static NotableWordFormsPart GetEmptyPart()
    {
        return new NotableWordFormsPart
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
        NotableWordFormsPart part = GetPart();

        string json = TestHelper.SerializePart(part);
        NotableWordFormsPart part2 =
            TestHelper.DeserializePart<NotableWordFormsPart>(json)!;

        Assert.Equal(part.Id, part2.Id);
        Assert.Equal(part.TypeId, part2.TypeId);
        Assert.Equal(part.ItemId, part2.ItemId);
        Assert.Equal(part.RoleId, part2.RoleId);
        Assert.Equal(part.CreatorId, part2.CreatorId);
        Assert.Equal(part.UserId, part2.UserId);

        Assert.Equal(part.Forms.Count, part2.Forms.Count);
    }

    [Fact]
    public void GetDataPins_NoEntries_Ok()
    {
        NotableWordFormsPart part = GetPart();
        part.Forms.Clear();

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
        NotableWordFormsPart part = GetEmptyPart();

        for (int n = 1; n <= 3; n++)
        {
            part.Forms.Add(new NotableWordForm
            {
                Eid = $"e{n}",
                Language = $"l{n}",
                Value = $"f{n}",
                Tags = [ $"t{n}" ],
                ReferenceForm = $"r{n}",
            });
        }

        List<DataPin> pins = [.. part.GetDataPins(null)];

        Assert.Equal(16, pins.Count);

        // tot-count
        DataPin? pin = pins.Find(p => p.Name == "tot-count");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);
        Assert.Equal("3", pin!.Value);

        for (int n = 1; n <= 3; n++)
        {
            // eid
            pin = pins.Find(p => p.Name == "eid" && p.Value == $"e{n}");
            Assert.NotNull(pin);
            TestHelper.AssertPinIds(part, pin!);

            // language
            pin = pins.Find(p => p.Name == "language" && p.Value == $"l{n}");
            Assert.NotNull(pin);
            TestHelper.AssertPinIds(part, pin!);

            // value
            pin = pins.Find(p => p.Name == "form" && p.Value == $"f{n}");
            Assert.NotNull(pin);
            TestHelper.AssertPinIds(part, pin!);

            // tags
            pin = pins.Find(p => p.Name == "tag" && p.Value == $"t{n}");
            Assert.NotNull(pin);
            TestHelper.AssertPinIds(part, pin!);

            // reference form
            pin = pins.Find(p => p.Name == "ref-form" && p.Value == $"r{n}");
            Assert.NotNull(pin);
            TestHelper.AssertPinIds(part, pin!);
        }
    }
}
