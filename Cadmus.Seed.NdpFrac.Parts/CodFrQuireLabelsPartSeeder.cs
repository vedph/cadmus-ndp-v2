using Bogus;
using Cadmus.Core;
using Fusi.Tools.Configuration;
using System;
using Cadmus.NdpFrac.Parts;
using System.Collections.Generic;

namespace Cadmus.Seed.NdpFrac.Parts;

/// <summary>
/// Seeder for <see cref="CodFrQuireLabelsPart"/>.
/// Tag: <c>seed.it.vedph.ndp.cod-fr-quire-labels</c>.
/// </summary>
/// <seealso cref="PartSeederBase" />
[Tag("seed.it.vedph.ndp.cod-fr-quire-labels")]
public sealed class CodFrQuireLabelsPartSeeder : PartSeederBase
{
    private static List<CodFrQuireLabel> GetQuireLabels(int count)
    {
        List<CodFrQuireLabel> labels = [];

        for (int i = 0; i < count; i++)
        {
            CodFrQuireLabel label = new Faker<CodFrQuireLabel>()
                .RuleFor(l => l.Positions, f => [
                    f.PickRandom("margin-top", "margin-bottom")])
                .RuleFor(l => l.Types, f =>
                    [f.PickRandom("latn", "n-grek")])
                .RuleFor(l => l.Text, f => f.Lorem.Word())
                .RuleFor(l => l.Ink, f => f.Lorem.Sentence())
                .RuleFor(l => l.Note, f =>
                    f.Random.Bool(0.25f)? f.Lorem.Sentence() : null)
                .Generate();
            labels.Add(label);
        }

        return labels;
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

        CodFrQuireLabelsPart part = new Faker<CodFrQuireLabelsPart>()
           .RuleFor(p => p.Labels, f => GetQuireLabels(f.Random.Number(1, 3)))
           .Generate();
        SetPartMetadata(part, roleId, item);

        return part;
    }
}
