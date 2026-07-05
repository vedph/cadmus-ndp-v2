using Cadmus.Refs.Bricks;
using System.Collections.Generic;
using System.Text;

namespace Cadmus.NdpBooks.Parts;

/// <summary>
/// A font used for printing.
/// </summary>
public class PrintFont
{
    /// <summary>
    /// The optional font entity ID.
    /// </summary>
    public string? Eid { get; set; }

    /// <summary>
    /// The font's family. This is usually a descriptive conventional ID like
    /// "R13" form a Roman font using a specific size. Optionally it can use
    /// a <c>print-font-families</c> thesaurus.
    /// </summary>
    public string Family { get; set; } = "";

    /// <summary>
    /// The list of sections where the font is used (e.g. title, body, comment,
    /// proem). Usually from thesaurus <c>print-layout-sections</c>.
    /// </summary>
    public List<string>? Sections { get; set; }

    /// <summary>
    /// The features of the font, especially useful when the family can't be
    /// specified (e.g. uppercase, lowercase, etc.). Usually from thesaurus
    /// <c>print-font-features</c>
    /// </summary>
    public List<string>? Features { get; set; }

    /// <summary>
    /// External identifiers of the font.
    /// </summary>
    public List<AssertedCompositeId>? Ids { get; set; }

    /// <summary>
    /// Free text note.
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Convert this instance to a string representation.
    /// </summary>
    /// <returns>String.</returns>
    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append(Family);
        if (!string.IsNullOrEmpty(Eid)) sb.Append($" (#{Eid})");
        return sb.ToString();
    }
}
