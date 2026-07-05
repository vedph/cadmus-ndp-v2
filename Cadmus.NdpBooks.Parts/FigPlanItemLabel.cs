using System.Collections.Generic;
using System.Text;

namespace Cadmus.NdpBooks.Parts;

/// <summary>
/// A label for a figurative plan item.
/// </summary>
public class FigPlanItemLabel
{
    /// <summary>
    /// The type of label, e.g. legend, topographic indication, etc.
    /// Usually from thesaurus <c>fig-plan-item-label-types</c>.
    /// </summary>
    public string Type { get; set; } = "";

    /// <summary>
    /// The language(s) used in the label.
    /// Usually from thesaurus <c>fig-plan-item-label-languages</c>.
    /// </summary>
    public List<string>? Languages { get; set; }

    /// <summary>
    /// The label value, e.g. the legend text, the character name, etc.
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// A free text note about the label.
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// The fonts used in the label.
    /// </summary>
    public List<PrintFont>? Fonts { get; set; }

    /// <summary>
    /// The label types found in the item: e.g. a legend for the whole image,
    /// or a character name on a character in the image, etc.
    /// </summary>
    public List<FigPlanItemLabel>? Labels { get; set; }

    /// <summary>
    /// Converts this label to a string.
    /// </summary>
    /// <returns>String.</returns>
    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append(Type);
        if (Languages?.Count > 0)
        {
            sb.Append(" [");
            sb.Append(string.Join(", ", Languages));
            sb.Append(']');
        }
        if (!string.IsNullOrEmpty(Value))
        {
            sb.Append(": ");
            sb.Append(Value);
        }
        return sb.ToString();
    }
}
