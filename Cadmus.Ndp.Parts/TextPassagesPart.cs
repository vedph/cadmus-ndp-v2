using System.Collections.Generic;
using System.Text;
using Cadmus.Core;
using Fusi.Tools.Configuration;

namespace Cadmus.Ndp.Parts;

/// <summary>
/// Text passages part. This contains a list of text passages.
/// <para>Tag: <c>it.vedph.ndp.text-passages</c>.</para>
/// </summary>
[Tag("it.vedph.ndp.text-passages")]
public sealed class TextPassagesPart : PartBase
{
    /// <summary>
    /// Gets or sets the passages.
    /// </summary>
    public List<TextPassage> Passages { get; set; } = [];

    /// <summary>
    /// Get all the key=value pairs (pins) exposed by the implementor.
    /// </summary>
    /// <param name="item">The optional item. The item with its parts
    /// can optionally be passed to this method for those parts requiring
    /// to access further data.</param>
    /// <returns>The pins: <c>tot-count</c> and a collection of pins.</returns>
    public override IEnumerable<DataPin> GetDataPins(IItem? item = null)
    {
        DataPinBuilder builder = new();

        builder.Set("tot", Passages?.Count ?? 0, false);

        if (Passages?.Count > 0)
        {
            foreach (TextPassage passage in Passages)
                builder.AddValue("citation", passage.Citation);
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
               "tot-count",
               "The total count of entries."),
            new DataPinDefinition(DataPinValueType.String,
                "location",
                "The passages citations.",
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

        sb.Append("[TextPassages]");

        if (Passages?.Count > 0)
        {
            sb.Append(' ');
            int n = 0;
            foreach (var entry in Passages)
            {
                if (++n > 3) break;
                if (n > 1) sb.Append("; ");
                sb.Append(entry);
            }
            if (Passages.Count > 3)
                sb.Append("...(").Append(Passages.Count).Append(')');
        }

        return sb.ToString();
    }
}
