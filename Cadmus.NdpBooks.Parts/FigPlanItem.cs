namespace Cadmus.NdpBooks.Parts;

/// <summary>
/// An item in a figurative plan, such as an illustration, initial, etc.
/// </summary>
public class FigPlanItem
{
    /// <summary>
    /// The entity ID assigned to this item.
    /// </summary>
    public string Eid { get; set; } = "";

    /// <summary>
    /// The item type (e.g. illustration, initial, scheme, diagram, frieze).
    /// Usually from thesaurus <c>fig-plan-types</c>.
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// A cross-project citation created according to some convention to
    /// link the figurative item to a textual passage.
    /// </summary>
    public string? Citation { get; set; }

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <returns>String.</returns>
    public override string ToString()
    {
        return $"{Eid}: {Type}";
    }
}
