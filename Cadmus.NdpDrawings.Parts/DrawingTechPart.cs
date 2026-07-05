using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Cadmus.Core;
using Cadmus.Mat.Bricks;
using Fusi.Tools.Configuration;

namespace Cadmus.NdpDrawings.Parts;

/// <summary>
/// Drawing techniques part. This part contains the technical description of
/// a drawing: material and techniques.
/// <para>Tag: <c>it.vedph.ndp.drawing-tech</c>.</para>
/// </summary>
[Tag("it.vedph.ndp.drawing-tech")]
public sealed class DrawingTechPart : PartBase
{
    /// <summary>
    /// The support's material, usually from thesaurus
    /// <c>drawing-tech-materials</c>.
    /// </summary>
    public string Material { get; set; } = "";

    /// <summary>
    /// General binary features, from thesaurus <c>drawing-tech-features</c>.
    /// </summary>
    public List<string>? Features { get; set; }

    /// <summary>
    /// Various measurements. Usually from thesauri <c>drawing-tech-size-units</c>,
    /// <c>drawing-tech-dim-tags</c>, <c>drawing-tech-measure-names</c>.
    /// </summary>
    public List<PhysicalMeasurement>? Measurements { get; set; }

    /// <summary>
    /// Techniques used in the drawing, from thesaurus
    /// <c>drawing-tech-techniques</c>.
    /// </summary>
    public List<string>? Techniques { get; set; }

    /// <summary>
    /// Colors used in the drawing, from thesaurus <c>drawing-tech-colors</c>.
    /// </summary>
    public List<string>? Colors { get; set; }

    /// <summary>
    /// A generic note.
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

        // material
        if (!string.IsNullOrEmpty(Material))
            builder.AddValue("material", Material);

        // features
        if (Features?.Count > 0)
            builder.AddValues("feature", Features);

        // measurements
        if (Measurements?.Count > 0)
        {
            foreach (PhysicalMeasurement m in Measurements)
            {
                builder.AddValue($"measure-{m.Name}",
                    m.Value.ToString(CultureInfo.InvariantCulture));
            }
        }

        // techniques
        if (Techniques?.Count > 0)
            builder.AddValues("technique", Techniques);

        // colors
        if (Colors?.Count > 0)
            builder.AddValues("color", Colors);

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
                "material",
                "The material."),
            new DataPinDefinition(DataPinValueType.String,
                "feature",
                "The features.",
                "M"),
            new DataPinDefinition(DataPinValueType.Decimal,
                "measure-<NAME>",
                "The measurements by name.",
                "M"),
            new DataPinDefinition(DataPinValueType.String,
                "technique",
                "The techniques.",
                "M"),
            new DataPinDefinition(DataPinValueType.String,
                "color",
                "The colors.",
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

        sb.Append("[DrawingTech] ").Append(Material);

        if (Features?.Count > 0)
            sb.Append(": features: ").Append(string.Join(", ", Features));

        return sb.ToString();
    }
}
