using System;
using System.Collections.Generic;
using System.Text;
using Cadmus.Core;
using Fusi.Tools.Configuration;

namespace Cadmus.NdpBooks.Parts;

/// <summary>
/// Part representing the implementation of a figurative plan. This contains
/// some general data about the implementation, and specific data for each item
/// of the implementation which with reference to the plan was changed, removed,
/// or added.
/// <para>Tag: <c>it.vedph.ndp.print-fig-plan-impl</c>.</para>
/// </summary>
[Tag("it.vedph.ndp.print-fig-plan-impl")]
public sealed class FigurativePlanImplPart : PartBase
{
    /// <summary>
    /// True if the implementation is complete with reference to the plan.
    /// </summary>
    public bool IsComplete { get; set; }

    /// <summary>
    /// Techniques used in the figurative plan (e.g. copper engraving, woodcut,
    /// lithograph, etching). Usually from thesaurus <c>fig-plan-techniques</c>.
    /// Overrides the techniques of the plan, if any. If no technique is
    /// specified here, all the plan's techniques are implied. If any technique
    /// is specified, this implies that these techniques fully replace the
    /// plan's techniques.
    /// </summary>
    public List<string> Techniques { get; set; } = [];

    /// <summary>
    /// The items in the figurative plan implementation.
    /// </summary>
    public List<FigPlanImplItem>? Items { get; set; }

    /// <summary>
    /// Free text description of the figurative plan implementation.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Features about the implementation, e.g. frame, frieze, etc.
    /// Usually from thesaurus <c>fig-plan-impl-features</c>.
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
        DataPinBuilder builder = new();

        builder.AddValue("complete", IsComplete);
        builder.AddValue("item-count", Items?.Count ?? 0);
        if (Features?.Count > 0)
        {
            builder.AddValues("feature", Features);
        }

        return builder.Build(this);
    }

    /// <summary>
    /// Gets the definitions of data pins used by the implementor.
    /// </summary>
    /// <returns>Data pins definitions.</returns>
    public override IList<DataPinDefinition> GetDataPinDefinitions()
    {
        return
        [
            new DataPinDefinition(
                DataPinValueType.Boolean,
                "complete",
                "True if the implementation is complete with reference to the plan."
            ),
            new DataPinDefinition(
                DataPinValueType.Integer,
                "item-count",
                "The count of items in the figurative plan implementation."
            ),
            new DataPinDefinition(
                DataPinValueType.String,
                "feature",
                "Feature(s) about the implementation, e.g. frame, frieze, etc.",
                "M"
            )
        ];
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

        sb.Append("[FigurativePlanImpl]");

        if (!IsComplete) sb.Append('*');
        sb.Append(": ").Append(Items?.Count ?? 0);

        return sb.ToString();
    }
}
