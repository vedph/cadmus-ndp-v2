using Bogus;
using Cadmus.Refs.Bricks;
using System.Collections.Generic;

namespace Cadmus.Seed.Ndp.Parts;

internal static class SeedHelper
{
    /// <summary>
    /// Gets a random number of document references.
    /// </summary>
    /// <param name="min">The min number of references to get.</param>
    /// <param name="max">The max number of references to get.</param>
    /// <returns>References.</returns>
    public static List<DocReference> GetDocReferences(int min, int max)
    {
        List<DocReference> refs = [];

        for (int n = 1; n <= Randomizer.Seed.Next(min, max + 1); n++)
        {
            refs.Add(new Faker<DocReference>()
                .RuleFor(r => r.Type, "book")
                .RuleFor(r => r.Tag, f => f.PickRandom(null, "tag"))
                .RuleFor(r => r.Type, "biblio")
                .RuleFor(r => r.Citation,
                    f => f.Person.LastName + " " + f.Date.Past(10).Year)
                .RuleFor(r => r.Note, f => f.Lorem.Sentence())
                .Generate());
        }

        return refs;
    }

    /// <summary>
    /// Gets a list of asserted composite IDs.
    /// </summary>
    /// <param name="min">The min number of IDs to get.</param>
    /// <param name="max">The max number of IDs to get.</param>
    /// <returns>IDs.</returns>
    public static List<AssertedCompositeId> GetAssertedCompositeIds(
        int min, int max)
    {
        List<AssertedCompositeId> ids = [];

        for (int n = 1; n <= Randomizer.Seed.Next(min, max + 1); n++)
        {
            ids.Add(new AssertedCompositeId
            {
                Target = new PinTarget
                {
                    Gid = $"http://www.resources.org/n{n}",
                    Label = $"res:n{n}"
                }
            });
        }

        return ids;
    }
}
