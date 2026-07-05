using System;
using Cadmus.Core;
using System.Collections.Generic;
using Cadmus.Seed.NdpDrawings.Parts;

namespace Cadmus.NdpDrawings.Parts.Test;

public sealed class DrawingTechPartTest
{
    private static DrawingTechPart GetPart()
    {
        DrawingTechPartSeeder seeder = new();
        IItem item = new Item
        {
            FacetId = "default",
            CreatorId = "zeus",
            UserId = "zeus",
            Description = "Test item",
            Title = "Test Item",
            SortKey = ""
        };
        return (DrawingTechPart)seeder.GetPart(item, null, null)!;
    }

    private static DrawingTechPart GetEmptyPart()
    {
        return new DrawingTechPart
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
        DrawingTechPart part = GetPart();

        string json = TestHelper.SerializePart(part);
        DrawingTechPart part2 = TestHelper.DeserializePart<DrawingTechPart>(json)!;

        Assert.Equal(part.Id, part2.Id);
        Assert.Equal(part.TypeId, part2.TypeId);
        Assert.Equal(part.ItemId, part2.ItemId);
        Assert.Equal(part.RoleId, part2.RoleId);
        Assert.Equal(part.CreatorId, part2.CreatorId);
        Assert.Equal(part.UserId, part2.UserId);
    }

    [Fact]
    public void GetDataPins_Tag_1()
    {
        DrawingTechPart part = GetEmptyPart();
        part.Material = "paper";
        part.Features = ["f1", "f2"];
        part.Measurements =
        [
            new Mat.Bricks.PhysicalMeasurement
            {
                Name = "width",
                Value = 30,
                Unit = "cm"
            },
            new Mat.Bricks.PhysicalMeasurement
            {
                Name = "height",
                Value = 40,
                Unit = "cm"
            }
        ];
        part.Techniques = ["t1", "t2"];
        part.Colors = ["black", "red"];

        List<DataPin> pins = [.. part.GetDataPins(null)];
        Assert.Equal(9, pins.Count);

        // material
        DataPin? pin = pins.Find(p => p.Name == "material" && p.Value == "paper");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        // features
        pin = pins.Find(p => p.Name == "feature" && p.Value == "f1");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        pin = pins.Find(p => p.Name == "feature" && p.Value == "f2");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        // measurements
        pin = pins.Find(p => p.Name == "measure-width" && p.Value == "30");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        pin = pins.Find(p => p.Name == "measure-height" && p.Value == "40");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        // techniques
        pin = pins.Find(p => p.Name == "technique" && p.Value == "t1");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);
        
        pin = pins.Find(p => p.Name == "technique" && p.Value == "t2");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        // colors
        pin = pins.Find(p => p.Name == "color" && p.Value == "black");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);

        pin = pins.Find(p => p.Name == "color" && p.Value == "red");
        Assert.NotNull(pin);
        TestHelper.AssertPinIds(part, pin!);
    }
}
