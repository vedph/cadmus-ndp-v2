using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cadmus.Core;
using Cadmus.Refs.Bricks;
using Fusi.Tools.Configuration;

namespace Cadmus.NdpBooks.Parts;

/// <summary>
/// Figurative plan part.
/// <para>Tag: <c>it.vedph.ndp.print-fig-plan</c>.</para>
/// </summary>
[Tag("it.vedph.ndp.print-fig-plan")]
public sealed class FigurativePlanPart : PartBase
{
    /// <summary>
    /// Artist IDs, if any.
    /// </summary>
    public List<AssertedCompositeId>? ArtistIds { get; set; }

    /// <summary>
    /// Techniques used in the figurative plan (e.g. copper engraving, woodcut,
    /// lithograph, etching). Usually from thesaurus <c>fig-plan-techniques</c>.
    /// </summary>
    public List<string> Techniques { get; set; } = [];

    /// <summary>
    /// An ordered list of items in the figurative plan (e.g. illustration,
    /// initial, etc.).
    /// </summary>
    public List<FigPlanItem>? Items { get; set; }

    /// <summary>
    /// A free text description of the figurative plan.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// A set of features of the figurative plan. Usually from thesaurus
    /// <c>fig-plan-features</c>.
    /// </summary>
    public List<string>? Features { get; set; }

    /// <summary>
    /// Get all the key=value pairs (pins) exposed by the implementor.
    /// </summary>
    /// <param name="item">The optional item. The item with its parts
    /// can optionally be passed to this method for those parts requiring
    /// to access further data.</param>
    /// <returns>The pins.</returns>
    public override IEnumerable<DataPin> GetDataPins(IItem? item = null)
    {
        DataPinBuilder builder = new(DataPinHelper.DefaultFilter);

        if (ArtistIds?.Count > 0)
        {
            builder.AddValue("artist-count", ArtistIds.Count);

            builder.AddValues("artist-label",
                ArtistIds.Where(id => id.Target?.Label != null)
                         .Select(id => id.Target!.Label), filter: true);
        }

        if (Techniques.Count > 0)
            builder.AddValues("technique", Techniques);

        if (Features?.Count > 0)
            builder.AddValues("feature", Features);

        if (Items?.Count > 0)
        {
            builder.AddValue("item-count", Items.Count);
            builder.AddValues("item-eid",
                Items.Where(i => !string.IsNullOrEmpty(i.Eid))
                     .Select(i => i.Eid));
            builder.AddValues("item-type", Items
                .Where(i => !string.IsNullOrEmpty(i.Type)).Select(i => i.Type!));
        }

        return builder.Build(this);
    }

    /// <summary>
    /// Gets the definitions of data pins used by the implementor.
    /// </summary>
    /// <returns>Data pins definitions.</returns>
    public override IList<DataPinDefinition> GetDataPinDefinitions()
    {
        return new List<DataPinDefinition>(
        [
            new DataPinDefinition(DataPinValueType.Integer,
                "artist-count",
                "The count of artists."),
            new DataPinDefinition(DataPinValueType.String,
                "artist-label",
                "The artist(s) filtered label(s).",
                "MF"),
            new DataPinDefinition(DataPinValueType.String,
                "technique",
                "The figurative plan technique(s).",
                "M"),
            new DataPinDefinition(DataPinValueType.String,
                "feature",
                "The figurative plan feature(s).",
                "M"),
            new DataPinDefinition(DataPinValueType.Integer,
                "item-count",
                "The count of items in the figurative plan."),
            new DataPinDefinition(DataPinValueType.String,
                "item-eid",
                "The EID(s) of items in the figurative plan.",
                "M"),
            new DataPinDefinition(DataPinValueType.String,
                "item-type",
                "The type(s) of items in the figurative plan.",
                "M")
        ]);
    }

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <returns>
    /// A <see cref="string" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        StringBuilder sb = new();

        sb.Append("[FigurativePlan]");

        if (ArtistIds?.Count > 0)
        {
            sb.AppendJoin(", ", ArtistIds
                .Where(id => !string.IsNullOrEmpty(id.Target?.Label))
                .Select(id => id.Target?.Label));
        }

        if (Items?.Count > 0)
            sb.Append(" (").Append(Items.Count).Append(')');

        return sb.ToString();
    }
}
