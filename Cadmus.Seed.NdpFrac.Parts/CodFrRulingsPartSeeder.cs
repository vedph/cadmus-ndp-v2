using Bogus;
using Cadmus.Core;
using Cadmus.NdpFrac.Parts;
using Fusi.Tools.Configuration;
using System;
using System.Collections.Generic;

namespace Cadmus.Seed.NdpFrac.Parts;

/// <summary>
/// Seeder for <see cref="CodFrRulingsPart"/>.
/// Tag: <c>seed.it.vedph.ndp.cod-fr-rulings</c>.
/// </summary>
/// <seealso cref="PartSeederBase" />
[Tag("seed.it.vedph.ndp.cod-fr-rulings")]
public sealed class CodFrRulingsPartSeeder : PartSeederBase
{
    private static List<CodFrRuling> GetCodFrRulings(int count)
    {
        List<CodFrRuling> rulings = [];

        for (int i = 0; i < count; i++)
        {
            CodFrRuling ruling = new Faker<CodFrRuling>()
                .RuleFor(r => r.System, f => f.PickRandom("latn", "grek"))
                // TODO use thesaurus
                .RuleFor(r => r.Type, f => f.PickRandom("t1", "t2"))
                .RuleFor(r => r.Features, f => [f.PickRandom("drypoint", "plummet")])
                .RuleFor(r => r.Note,
                    f => f.Random.Bool(0.25f) ? f.Lorem.Sentence() : null)
                .Generate();
            rulings.Add(ruling);
        }

        return rulings;
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

        CodFrRulingsPart part = new Faker<CodFrRulingsPart>()
           .RuleFor(p => p.Rulings, f => GetCodFrRulings(f.Random.Number(1, 3)))
           .Generate();
        SetPartMetadata(part, roleId, item);

        return part;
    }
}
