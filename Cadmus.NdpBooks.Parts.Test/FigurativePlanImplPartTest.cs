using System;
using Cadmus.Core;
using System.Collections.Generic;
using Cadmus.Seed.NdpBooks.Parts;

namespace Cadmus.NdpBooks.Parts.Test;

public sealed class FigurativePlanImplPartTest
{
    private static FigurativePlanImplPart GetPart()
    {
        FigurativePlanImplPartSeeder seeder = new();
        IItem item = new Item
        {
            FacetId = "default",
            CreatorId = "zeus",
            UserId = "zeus",
            Description = "Test item",
            Title = "Test Item",
            SortKey = ""
        };
        return (FigurativePlanImplPart)seeder.GetPart(item, null, null)!;
    }

    private static FigurativePlanImplPart GetEmptyPart()
    {
        return new FigurativePlanImplPart
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
        FigurativePlanImplPart part = GetPart();

        string json = TestHelper.SerializePart(part);
        FigurativePlanImplPart part2 =
            TestHelper.DeserializePart<FigurativePlanImplPart>(json)!;

        Assert.Equal(part.Id, part2.Id);
        Assert.Equal(part.TypeId, part2.TypeId);
        Assert.Equal(part.ItemId, part2.ItemId);
        Assert.Equal(part.RoleId, part2.RoleId);
        Assert.Equal(part.CreatorId, part2.CreatorId);
        Assert.Equal(part.UserId, part2.UserId);
    }

    [Fact]
    public void GetDataPins_Ok()
    {
        FigurativePlanImplPart part = GetEmptyPart();
        part.IsComplete = true;
        part.Items = [new FigPlanImplItem()];
        part.Features = ["a", "b"];

        List<DataPin> pins = [.. part.GetDataPins(null)];
        Assert.Equal(4, pins.Count);

        DataPin? pin = pins.Find(p => p.Name == "complete" && p.Value == "1");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        pin = pins.Find(p => p.Name == "item-count" && p.Value == "1");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        pin = pins.Find(p => p.Name == "feature" && p.Value == "a");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        pin = pins.Find(p => p.Name == "feature" && p.Value == "b");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);
    }
}
