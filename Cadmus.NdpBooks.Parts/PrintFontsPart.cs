using System;
using System.Collections.Generic;
using System.Text;
using Cadmus.Core;
using Fusi.Tools.Configuration;

namespace Cadmus.NdpBooks.Parts;

/// <summary>
/// Print fonts part.
/// <para>Tag: <c>it.vedph.ndp.print-fonts</c>.</para>
/// </summary>
[Tag("it.vedph.ndp.print-fonts")]
public sealed class PrintFontsPart : PartBase
{
    /// <summary>
    /// Gets or sets the fonts.
    /// </summary>
    public List<PrintFont> Fonts { get; set; } = [];

    /// <summary>
    /// Get all the key=value pairs (pins) exposed by the implementor.
    /// </summary>
    /// <param name="item">The optional item. The item with its parts
    /// can optionally be passed to this method for those parts requiring
    /// to access further data.</param>
    /// <returns>The pins: <c>tot-count</c> and a collection of pins.
    /// </returns>
    public override IEnumerable<DataPin> GetDataPins(IItem? item = null)
    {
        DataPinBuilder builder = new();

        builder.Set("tot", Fonts?.Count ?? 0, false);

        if (Fonts?.Count > 0)
        {
            HashSet<string> families = [];
            HashSet<string> sections = [];
            HashSet<string> features = [];

            foreach (PrintFont font in Fonts)
            {
                builder.AddValue("eid", font.Eid);

                if (!string.IsNullOrEmpty(font.Family)) families.Add(font.Family);

                if (font.Sections?.Count > 0)
                    sections.UnionWith(font.Sections);

                if (font.Features?.Count > 0)
                    features.UnionWith(font.Features);
            }

            if (families.Count > 0) builder.AddValues("family", families);
            if (sections.Count > 0) builder.AddValues("section", sections);
            if (features.Count > 0) builder.AddValues("feature", features);
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
               "The total count of fonts."),
            new DataPinDefinition(DataPinValueType.String,
                "eid",
                "The font's entity ID(s).",
                "M"),
            new DataPinDefinition(DataPinValueType.String,
                "family",
                "The font's family name(s).",
                "M"),
            new DataPinDefinition(DataPinValueType.String,
                "section",
                "The font's section(s).",
                "M"),
            new DataPinDefinition(DataPinValueType.String,
                "feature",
                "The font's feature(s).",
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

        sb.Append("[PrintFonts]");

        if (Fonts?.Count > 0)
        {
            sb.Append(' ');
            int n = 0;
            foreach (var entry in Fonts)
            {
                if (++n > 3) break;
                if (n > 1) sb.Append("; ");
                sb.Append(entry);
            }
            if (Fonts.Count > 3)
                sb.Append("...(").Append(Fonts.Count).Append(')');
        }

        return sb.ToString();
    }
}
