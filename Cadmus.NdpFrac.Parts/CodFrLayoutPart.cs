using Cadmus.Core;
using Cadmus.Mat.Bricks;
using Cadmus.Refs.Bricks;
using Fusi.Tools.Configuration;
using System.Collections.Generic;

namespace Cadmus.NdpFrac.Parts;

/// <summary>
/// Codicological fragment layout part.
/// <para>Tag: <c>it.vedph.ndp.cod-fr-layout</c>.</para>
/// </summary>
[Tag("it.vedph.ndp.cod-fr-layout")]
public sealed class CodFrLayoutPart : PartBase
{
    /// <summary>
    /// The layout formula. Usually this follows the Bianconi-Orsini syntax
    /// (see https://github.com/vedph/cod-layout-view?tab=readme-ov-file#bianconi-orsini).
    /// </summary>
    public string Formula { get; set; } = "";

    /// <summary>
    /// Dimensions of any measurable elements in the layout, including those
    /// automatically derived from the layout formula.
    /// </summary>
    public List<PhysicalDimension> Dimensions { get; set; } = [];

    /// <summary>
    /// The pricking type (including no pricking), usually from thesaurus
    /// <c>cod-fr-layout-prickings</c>.
    /// </summary>
    public string Pricking { get; set; } = "";

    /// <summary>
    /// The count of columns in the layout, when it can be defined, or 0.
    /// </summary>
    public int ColumnCount { get; set; }

    /// <summary>
    /// Any type of countable elements in the layout with their value.
    /// </summary>
    public List<DecoratedCount> Counts { get; set; } = [];

    /// <summary>
    /// A list of thesaurus-defined features of the layout. Usually from
    /// thesaurus <c>cod-fr-layout-features</c>.
    /// </summary>
    public List<string>? Features { get; set; }

    /// <summary>
    /// A free text note about the layout.
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Get all the key=value pairs (pins) exposed by the implementor.
    /// </summary>
    /// <param name="item">The optional item. The item with its parts
    /// can optionally be passed to this method for those parts requiring
    /// to access further data.</param>
    /// <returns>The pins.</returns>
    public override IEnumerable<DataPin> GetDataPins(IItem? item = null)
    {
        DataPinBuilder builder = new();

        // formula
        if (!string.IsNullOrEmpty(Formula))
            builder.AddValue("formula", Formula);

        // dimensions
        if (Dimensions.Count > 0)
        {
            foreach (PhysicalDimension dimension in Dimensions)
                builder.AddValue($"d-{dimension.Tag}", dimension.Value);
        }

        // pricking
        if (!string.IsNullOrEmpty(Pricking))
            builder.AddValue("pricking", Pricking);

        // column-count
        if (ColumnCount > 0)
            builder.AddValue("column-count", ColumnCount);

        // counts
        if (Counts.Count > 0)
        {
            foreach (DecoratedCount count in Counts)
                builder.AddValue($"c-{count.Id}", count.Value);
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
            new DataPinDefinition(DataPinValueType.String,
               "formula",
               "The fragment layout formula."),
            new DataPinDefinition(DataPinValueType.Decimal,
               "d-{tag}",
               "The value of a dimension in the layout, identified by its tag.",
               "M"),
            new DataPinDefinition(DataPinValueType.String,
               "pricking",
               "The fragment pricking type."),
            new DataPinDefinition(DataPinValueType.Integer,
                "column-count",
                "The number of columns in the fragment layout."),
            new DataPinDefinition(DataPinValueType.Integer,
                "c-{ID}",
                "The value of a countable element in the layout, " +
                "identified by its ID.",
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
        return $"[CodFrLayout] {Formula}";
    }
}
