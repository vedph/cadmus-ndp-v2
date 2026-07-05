using Bogus;
using Cadmus.Core;
using Cadmus.Ndp.Parts;
using Fusi.Tools;
using Fusi.Tools.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cadmus.Seed.Ndp.Parts;

/// <summary>
/// Seeder for <see cref="TextPassagesPart"/>.
/// Tag: <c>seed.it.vedph.ndp.text-passages</c>.
/// </summary>
/// <seealso cref="PartSeederBase" />
[Tag("seed.it.vedph.ndp.text-passages")]
public sealed class TextPassagesPartSeeder : PartSeederBase,
    IConfigurable<TextPassagesPartSeederOptions>
{
    private TextPassagesPartSeederOptions _options = new();

    /// <summary>
    /// Configures the object with the specified options.
    /// </summary>
    /// <param name="options">The options.</param>
    public void Configure(TextPassagesPartSeederOptions options)
    {
        _options = options;
    }

    private static string BuildLocationFromPattern(string pattern, Faker faker)
    {
        StringBuilder sb = new();
        int i = 0;
        while (i < pattern.Length)
        {
            char c = pattern[i];
            if (c == '{' && i + 2 < pattern.Length && pattern[i + 2] == '}')
            {
                char p = pattern[i + 1];
                switch (p)
                {
                    case 'R':
                        sb.Append(RomanNumber.ToRoman(faker.Random.Int(1, 10))
                            .ToUpperInvariant());
                        break;
                    case 'r':
                        sb.Append(RomanNumber.ToRoman(faker.Random.Int(1, 10))
                            .ToLowerInvariant());
                        break;
                    case 'N':
                        sb.Append(faker.Random.Int(1, 100));
                        break;
                    default:
                        sb.Append(p);
                        break;
                }
                i += 3;
            }
            else
            {
                sb.Append(c);
                i++;
            }
        }
        return sb.ToString();
    }

    private List<TextPassage> GetPassages(Faker faker)
    {
        int count = faker.Random.Int(1, 3);
        List<TextPassage> passages = [];

        for (int i = 0; i < count; i++)
        {
            TextPassage passage = new()
            {
                Citation = BuildLocationFromPattern(
                        _options.CitationPattern, faker),
                Text = faker.Lorem.Sentence(),
            };

            if (_options.Tags?.Count > 0)
                passage.Tag = faker.PickRandom(_options.Tags);

            if (_options.Features?.Count > 0)
                passage.Features = [faker.PickRandom(_options.Features)];

            passages.Add(passage);
        }

        return passages;
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

        TextPassagesPart part = new Faker<TextPassagesPart>()
           .RuleFor(p => p.Passages, f => GetPassages(f))
           .Generate();

        SetPartMetadata(part, roleId, item);

        return part;
    }
}

/// <summary>
/// Options for <see cref="TextPassagesPartSeeder"/>.
/// </summary>
public class TextPassagesPartSeederOptions
{
    /// <summary>
    /// The pattern for the citation. Placeholders are {R} for an uppercase
    /// Roman number, {r} for a lowercase Roman number, and {N} for an
    /// Arabic number.
    /// </summary>
    public string CitationPattern { get; set; } = "@dc:If. {R} {N}";

    /// <summary>
    /// IDs for text-passage-tags.
    /// </summary>
    public IList<string>? Tags { get; set; }

    /// <summary>
    /// IDs for text-passage-features.
    /// </summary>
    public IList<string>? Features { get; set; }
}
