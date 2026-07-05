using Bogus;
using Cadmus.Core;
using Cadmus.NdpFrac.Parts;
using Fusi.Tools.Configuration;
using System;

namespace Cadmus.Seed.NdpFrac.Parts;

/// <summary>
/// Seeder for <see cref="CodFrLayoutPart"/>.
/// Tag: <c>seed.it.vedph.ndp.cod-fr-layout</c>.
/// </summary>
/// <seealso cref="PartSeederBase" />
[Tag("seed.it.vedph.ndp.cod-fr-layout")]
public sealed class CodFrLayoutPartSeeder : PartSeederBase
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

        CodFrLayoutPart part = new Faker<CodFrLayoutPart>()
            .RuleFor(p => p.Formula, f => f.PickRandom(
                "mm (57) [175] x (145) [150] = (22) // (35) [115] // - x 10 // 115 // (20)",
                "mm 336 x 240 = 18 // 282 // 36 x 25 / 4 // 174 // 4 / 33"))
            .Generate();
        SetPartMetadata(part, roleId, item);

        return part;
    }
}
