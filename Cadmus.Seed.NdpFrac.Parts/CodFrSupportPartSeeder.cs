using Bogus;
using Cadmus.Core;
using Cadmus.NdpFrac.Parts;
using Fusi.Tools.Configuration;
using System;

namespace Cadmus.Seed.NdpFrac.Parts;

/// <summary>
/// Seeder for <see cref="CodFrSupportPart"/>.
/// Tag: <c>seed.it.vedph.ndp.cod-fr-support</c>.
/// </summary>
/// <seealso cref="PartSeederBase" />
[Tag("seed.it.vedph.ndp.cod-fr-support")]
public sealed class CodFrSupportPartSeeder : PartSeederBase
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

        CodFrSupportPart part = new Faker<CodFrSupportPart>()
           .RuleFor(p => p.Material, f => f.PickRandom("paper", "parchment"))
           .RuleFor(p => p.Location, f => f.PickRandom("A1", "B2"))
           .RuleFor(p => p.Reuse, f => f.Random.Bool(0.5f)
                ? f.PickRandom("cover", "flyleave") : null)
           .RuleFor(p => p.Container, f => f.PickRandom("envelope", "host-codex"))
           .Generate();
        SetPartMetadata(part, roleId, item);

        return part;
    }
}
