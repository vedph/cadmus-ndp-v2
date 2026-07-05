using Bogus;
using Cadmus.Core;
using Cadmus.Mat.Bricks;
using Cadmus.NdpDrawings.Parts;
using Fusi.Tools.Configuration;
using System;
using System.Collections.Generic;

namespace Cadmus.Seed.NdpDrawings.Parts;

/// <summary>
/// Seeder for <see cref="DrawingTechPart"/>.
/// Tag: <c>seed.it.vedph.ndp.drawing-tech</c>.
/// </summary>
/// <seealso cref="PartSeederBase" />
[Tag("seed.it.vedph.ndp.drawing-tech")]
public sealed class DrawingTechPartSeeder : PartSeederBase
{
    private static List<PhysicalMeasurement> GetMeasurements(Faker f)
    {
        return
        [
            new()
            {
                Name = "width",
                Value = f.Random.Int(10, 50),
                Unit = "cm",
            },
            new()
            {
                Name = "height",
                Value = f.Random.Int(10, 50),
                Unit = "cm",
            }
        ];
    }

    /// <summary>
    /// Creates and seeds a new part.
    /// </summary>
    /// <param name="item">The item this part should belong to.</param>
    /// <param name="roleId">The optional part role ID.</param>
    /// <param name="factory">The part seeder factory. This is used
    /// for layer parts, which need to seed a set of fragments.</param>
    /// <returns>A new part or null.</returns>
    /// <exception cref="ArgumentNullException">item or factory</exception>
    public override IPart? GetPart(IItem item, string? roleId,
        PartSeederFactory? factory)
    {
        ArgumentNullException.ThrowIfNull(item);

        DrawingTechPart part = new Faker<DrawingTechPart>()
           // TODO thesauri
           .RuleFor(p => p.Material, f => f.PickRandom("paper", "cardboard"))
           .RuleFor(p => p.Features, f => [f.PickRandom("f1", "f2")])
           .RuleFor(p => p.Measurements, f => GetMeasurements(f))
           .RuleFor(p => p.Techniques, f => [f.PickRandom("t1", "t2")])
           .RuleFor(p => p.Colors, f => [f.PickRandom("black", "red")])
           .RuleFor(p => p.Note, f => f.Random.Bool(0.25F) ? f.Lorem.Sentence() : null)
           .Generate();
        SetPartMetadata(part, roleId, item);

        return part;
    }
}
