using Bogus;
using Cadmus.Core;
using Cadmus.Ndp.Parts;
using Fusi.Tools.Configuration;
using System;
using System.Collections.Generic;

namespace Cadmus.Seed.Ndp.Parts;

/// <summary>
/// Seeder for <see cref="NotableWordFormsPart"/>.
/// Tag: <c>seed.it.vedph.ndp.notable-word-forms</c>.
/// </summary>
/// <seealso cref="PartSeederBase" />
[Tag("seed.it.vedph.ndp.notable-word-forms")]
public sealed class NotableWordFormsPartSeeder : PartSeederBase,
    IConfigurable<NotableWordFormsPartSeederOptions>
{
    private NotableWordFormsPartSeederOptions? _options;

    /// <summary>
    /// Configures the object with the specified options.
    /// </summary>
    /// <param name="options">The options.</param>
    public void Configure(NotableWordFormsPartSeederOptions options)
    {
        _options = options;
    }

    private List<NotableWordForm> GetForms(Faker f)
    {
        int count = f.Random.Int(1, 3);
        List<NotableWordForm> forms = new(count);

        for (int i = 0; i < count; i++)
        {
            forms.Add(new NotableWordForm
            {
                Value = f.Lorem.Word(),
                Rank = (short)f.Random.Int(0, 3),
                Tags = _options?.Tags?.Count > 0 ?
                    [ f.PickRandom(_options.Tags) ] : null,
                Note = f.Random.Bool(0.25F)? f.Lorem.Sentence() : null,
                References = f.Random.Bool(0.5F)?
                    SeedHelper.GetDocReferences(1, 2) : null,
                Links = f.Random.Bool(0.5F) ?
                    SeedHelper.GetAssertedCompositeIds(1, 2) : null
            });
        }
        return forms;
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

        NotableWordFormsPart part = new Faker<NotableWordFormsPart>()
           .RuleFor(p => p.Forms, GetForms)
           .Generate();

        SetPartMetadata(part, roleId, item);

        return part;
    }
}

/// <summary>
/// Options for <see cref="NotableWordFormsPartSeeder"/>.
/// </summary>
public class NotableWordFormsPartSeederOptions
{
    /// <summary>
    /// The tags to use for seeding forms.
    /// </summary>
    public IList<string>? Tags { get; set; }
}
