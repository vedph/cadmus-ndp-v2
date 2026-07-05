using Bogus;
using Cadmus.Core;
using Cadmus.NdpBooks.Parts;
using Fusi.Tools.Configuration;
using System;
using System.Collections.Generic;

namespace Cadmus.Seed.NdpBooks.Parts;

/// <summary>
/// Seeder for <see cref="PrintFontsPart"/>.
/// Tag: <c>seed.it.vedph.ndp.print-fonts</c>.
/// </summary>
/// <seealso cref="PartSeederBase" />
[Tag("seed.it.vedph.ndp.print-fonts")]
public sealed class PrintFontsPartSeeder : PartSeederBase
{
    private static List<PrintFont> GetFonts(int count)
    {
        Faker<PrintFont> faker = new();
        List<PrintFont> fonts = new(count);

        for (int n = 1; n <= count; n++)
        {
            fonts.Add(faker
                .RuleFor(font => font.Eid, f => $"{f.Random.Word()}{n}")
                .RuleFor(font => font.Family, f => f.PickRandom(
                    "R4", "R5", "G4", "G5"))
                .RuleFor(font => font.Sections, f => [f.PickRandom(
                    "title", "body", "comment")])
                .RuleFor(font => font.Features, f => [f.PickRandom(
                    "uppercase", "lowercase")])
                .Generate());
        }
        return fonts;
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

        PrintFontsPart part = new Faker<PrintFontsPart>()
           .RuleFor(p => p.Fonts, f => GetFonts(f.Random.Number(1, 3)))
           .Generate();
        SetPartMetadata(part, roleId, item);

        return part;
    }
}
