using System;
using System.Collections.Generic;
using Cadmus.Core;
using Cadmus.Refs.Bricks;
using Cadmus.Seed.NdpBooks.Parts;

namespace Cadmus.NdpBooks.Parts.Test;

public sealed class FigurativePlanPartTest
{
    private static FigurativePlanPart GetPart()
    {
        FigurativePlanPartSeeder seeder = new();
        IItem item = new Item
        {
            FacetId = "default",
            CreatorId = "zeus",
            UserId = "zeus",
            Description = "Test item",
            Title = "Test Item",
            SortKey = ""
        };
        return (FigurativePlanPart)seeder.GetPart(item, null, null)!;
    }

    private static FigurativePlanPart GetEmptyPart()
    {
        return new FigurativePlanPart
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
        FigurativePlanPart part = GetPart();

        string json = TestHelper.SerializePart(part);
        FigurativePlanPart part2 =
            TestHelper.DeserializePart<FigurativePlanPart>(json)!;

        Assert.Equal(part.Id, part2.Id);
        Assert.Equal(part.TypeId, part2.TypeId);
        Assert.Equal(part.ItemId, part2.ItemId);
        Assert.Equal(part.RoleId, part2.RoleId);
        Assert.Equal(part.CreatorId, part2.CreatorId);
        Assert.Equal(part.UserId, part2.UserId);
    }

    [Fact]
    public void GetDataPins_Empty_Empty()
    {
        FigurativePlanPart part = GetEmptyPart();
        Assert.Empty(part.GetDataPins());
    }

    [Fact]
    public void GetDataPins_NonEmpty_Ok()
    {
        FigurativePlanPart part = GetEmptyPart();
        part.ArtistIds =
        [
            new()
            {
                Target = new PinTarget
                {
                    Gid = "http://alltheguys.edu/titius",
                    Label = "Titius"
                }
            }
        ];
        part.Techniques.Add("etching");
        part.Features = ["some-feature"];
        part.Items =
        [
            new FigPlanItem()
            {
                Eid = "initial1",
                Type = "initial",
            }
        ];

        List<DataPin> pins = [.. part.GetDataPins(null)];
        Assert.Equal(7, pins.Count);

        DataPin? pin = pins.Find(p => p.Name == "artist-count" && p.Value == "1");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        pin = pins.Find(p => p.Name == "artist-label" && p.Value == "titius");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        pin = pins.Find(p => p.Name == "technique" && p.Value == "etching");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        pin = pins.Find(p => p.Name == "feature" && p.Value == "some-feature");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        pin = pins.Find(p => p.Name == "item-count" && p.Value == "1");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        pin = pins.Find(p => p.Name == "item-eid" && p.Value == "initial1");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        pin = pins.Find(p => p.Name == "item-type" && p.Value == "initial");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);
    }
}
