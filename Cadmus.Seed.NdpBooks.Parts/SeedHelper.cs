using Bogus;
using Cadmus.Mat.Bricks;
using Cadmus.NdpBooks.Parts;
using Cadmus.Refs.Bricks;
using System;
using System.Collections.Generic;

namespace Cadmus.Seed.NdpBooks.Parts;

internal static class SeedHelper
{
    /// <summary>
    /// Truncates the specified value to the specified number of decimals.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="decimals">The decimals.</param>
    /// <returns>Truncated value.</returns>
    private static float Truncate(float value, int decimals)
    {
        double factor = Math.Pow(10, decimals);
        return (float)((float)Math.Truncate(factor * value) / factor);
    }

    public static List<PhysicalDimension> GetDimensions(int count)
    {
        List<PhysicalDimension> dimensions = [];

        for (int n = 1; n <= count; n++)
        {
            dimensions.Add(new Faker<PhysicalDimension>()
                .RuleFor(d => d.Tag, f => f.Lorem.Word())
                .RuleFor(d => d.Value, f => Truncate(f.Random.Float(2, 10), 2))
                .RuleFor(d => d.Unit, "cm")
                .Generate());
        }

        return dimensions;
    }

    public static PhysicalSize GetPhysicalSize()
    {
        List<PhysicalDimension> dimensions = GetDimensions(2);

        Faker faker = new();
        return new PhysicalSize
        {
            Tag = "tag",
            W = dimensions[0],
            H = dimensions[1],
            Note = faker.Random.Bool(0.25f) ? faker.Lorem.Sentence() : null
        };
    }

    /// <summary>
    /// Gets a random number of document references.
    /// </summary>
    /// <param name="count">The number of references to get.</param>
    /// <returns>References.</returns>
    public static List<DocReference> GetDocReferences(int count)
    {
        List<DocReference> refs = [];

        for (int n = 1; n <= count; n++)
        {
            refs.Add(new Faker<DocReference>()
                .RuleFor(r => r.Tag, f => f.PickRandom(null, "tag"))
                .RuleFor(r => r.Type, "biblio")
                .RuleFor(r => r.Citation,
                    f => f.Person.LastName + " " + f.Date.Past(10).Year)
                .RuleFor(r => r.Note, f => f.Lorem.Sentence())
                .Generate());
        }

        return refs;
    }

    public static List<AssertedCompositeId> GetAssertedCompositeIds(int count)
    {
        List<AssertedCompositeId> ids = [];

        for (int n = 1; n <= count; n++)
        {
            ids.Add(new Faker<AssertedCompositeId>()
                .RuleFor(r => r.Tag, f => f.PickRandom(null, "tag"))
                .RuleFor(r => r.Scope, f => f.Lorem.Word())
                .RuleFor(r => r.Target, f =>
                    new PinTarget
                    {
                        Gid = f.Internet.Url(),
                        Label = $"n{n}"
                    })
                .RuleFor(r => r.Assertion, GetAssertion())
                .Generate());
        }

        return ids;
    }

    public static Assertion GetAssertion()
    {
        return new Faker<Assertion>()
            .RuleFor(a => a.Tag, f => f.PickRandom("a", "b", null))
            .RuleFor(a => a.Rank, f => f.Random.Short(1, 3))
            .RuleFor(a => a.References, GetDocReferences(2))
            .RuleFor(a => a.Note, f => f.Lorem.Sentence().OrNull(f))
            .Generate();
    }

    public static List<FigPlanItem> GetFigPlanItems(int count)
    {
        List<FigPlanItem> items = [];
        Dictionary<string, int> types = new()
        {
            { "illustration", 0 },
            { "initial", 0 },
            { "ornament", 0 }
        };

        for (int i = 0; i < count; i++)
        {
            FigPlanItem item = new Faker<FigPlanItem>()
                .RuleFor(p => p.Type,
                    f => f.PickRandom("illustration", "initial", "ornament"))
                .RuleFor(p => p.Citation, f => f.Random.Bool(0.3f)
                    ? $"{f.Random.Number(1, 10)}.{f.Random.Number(1, 10)}"
                    : null)
                .Generate();
            types[item.Type!]++;
            item.Eid = $"{item.Type}-{types[item.Type!]}";
            items.Add(item);
        }

        return items;
    }

    private static List<FigPlanItemLabel> GetFigPlanItemLabels(int count)
    {
        List<FigPlanItemLabel> labels = [];
        for (int i = 0; i < count; i++)
        {
            labels.Add(new Faker<FigPlanItemLabel>()
                .RuleFor(l => l.Type, f => f.PickRandom("legend", "inscription"))
                .RuleFor(l => l.Languages, f=> [f.PickRandom("la", "it")]) // TODO thesaurus 
                .RuleFor(l => l.Value, f => f.Lorem.Sentence(3))
                .RuleFor(l => l.Note, f => f.Lorem.Sentence().OrNull(f))
                .Generate());
        }
        return labels;
    }

    public static List<FigPlanImplItem> GetFigPlanImplItems(int count)
    {
        List<FigPlanImplItem> items = [];
        Dictionary<string, int> types = new()
        {
            { "illustration", 0 },
            { "initial", 0 },
            { "ornament", 0 }
        };

        for (int i = 0; i < count; i++)
        {
            FigPlanImplItem item = new Faker<FigPlanImplItem>()
                .RuleFor(p => p.Type,
                    f => f.PickRandom("illustration", "initial", "ornament"))
                .RuleFor(p => p.Citation, f => f.Random.Bool(0.3f)
                    ? $"{f.Random.Number(1, 10)}.{f.Random.Number(1, 10)}"
                    : null)
                .RuleFor(p => p.Location, f =>
                    $"{f.Random.Number(1, 10)}{(f.Random.Bool()? 'v' : 'r')}")
                .RuleFor(p => p.ChangeType,
                    f => f.PickRandom("change", "replace"))
                .RuleFor(p => p.Features, f => [f.PickRandom(
                    "frame-add", "frame-del")])
                .RuleFor(p => p.MatrixType, "woodblock") // TODO thesaurus
                .RuleFor(p => p.MatrixState,
                    f => f.PickRandom("fair", "damaged"))
                .RuleFor(p => p.MatrixStateDsc, f => f.Lorem.Sentence())
                .RuleFor(p => p.Position,
                    f => f.PickRandom("margin-top", "margin-bottom"))
                .RuleFor(p => p.Size, GetPhysicalSize())
                .RuleFor(p => p.Labels,
                    f => GetFigPlanItemLabels(f.Random.Number(1, 3)))
                .Generate();
            types[item.Type!]++;
            item.Eid = $"{item.Type}-{types[item.Type!]}";
            items.Add(item);
        }

        return items;
    }
}
