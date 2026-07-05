using Cadmus.Refs.Bricks;
using System.Collections.Generic;
using System.Text;

namespace Cadmus.NdpFrac.Parts;

/// <summary>
/// A label for a codicological fragment quire.
/// </summary>
public class CodFrQuireLabel
{
    /// <summary>
    /// The positions of the label within the quire, like bottom margin,
    /// top margin, center, inner corner, outer corner, column A, etc.
    /// Usually from thesaurus <c>cod-fr-quire-label-positions</c>.
    /// </summary>
    public List<string> Positions { get; set; } = [];

    /// <summary>
    /// Label types like Latin alphabet, Greek alphabet, Arabic digits, etc.)
    /// Usually from thesaurus <c>cod-fr-quire-label-types</c>.
    /// </summary>
    public List<string> Types { get; set; } = [];

    /// <summary>
    /// The label's text.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// The hand's ID.
    /// </summary>
    public AssertedCompositeId? HandId { get; set; }

    /// <summary>
    /// Notes about the label's ink.
    /// </summary>
    public string? Ink { get; set; }

    /// <summary>
    /// A generic free text note.
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <returns>String.</returns>
    public override string ToString()
    {
        StringBuilder sb = new();

        // positions
        if (Positions.Count > 0)
            sb.AppendJoin(", ", Positions);

        // types
        if (Types?.Count > 0)
        {
            sb.Append(" (");
            sb.AppendJoin(", ", Types);
            sb.Append(')');
        }

        // text
        if (!string.IsNullOrEmpty(Text))
            sb.Append(": ").Append(Text);

        return sb.ToString();
    }
}
