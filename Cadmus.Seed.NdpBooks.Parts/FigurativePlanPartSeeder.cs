using Bogus;
using Cadmus.Core;
using Cadmus.NdpBooks.Parts;
using Fusi.Tools.Configuration;
using System;

namespace Cadmus.Seed.NdpBooks.Parts;

/// <summary>
/// Seeder for <see cref="FigurativePlanPart"/>.
/// Tag: <c>seed.it.vedph.ndp.print-fig-plan</c>.
/// </summary>
/// <seealso cref="PartSeederBase" />
[Tag("seed.it.vedph.ndp.print-fig-plan")]
public sealed class FigurativePlanPartSeeder : PartSeederBase
{
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

        FigurativePlanPart part = new Faker<FigurativePlanPart>()
           .RuleFor(p => p.Techniques, f => [f.PickRandom("lithograph", "etching")])
           .RuleFor(p => p.Items, f => SeedHelper.GetFigPlanItems(f.Random.Number(1, 3)))
           .RuleFor(p => p.Description, f => f.Lorem.Sentence())
           //.RuleFor(p => p.Features, f => [f.PickRandom("")]) TODO: add thesaurus
           .Generate();
        SetPartMetadata(part, roleId, item);

        return part;
    }
}
