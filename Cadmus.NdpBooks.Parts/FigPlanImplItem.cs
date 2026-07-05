using Cadmus.Mat.Bricks;
using Cadmus.Refs.Bricks;
using System.Collections.Generic;
using System.Text;

namespace Cadmus.NdpBooks.Parts;

/// <summary>
/// Implementation of a figurative plan item.
/// </summary>
public class FigPlanImplItem : FigPlanItem
{
    /// <summary>
    /// The page location (e.g. <c>1r</c>).
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// The type of change made to the item in this instance with respect to
    /// the plan. Usually from thesaurus <c>fig-plan-impl-change-types</c>. 
    /// </summary>
    public string? ChangeType { get; set; }

    /// <summary>
    /// Any relevant features of the implemented item (e.g. matrix change,
    /// frame removed, etc.). Usually from thesaurus
    /// <c>fig-plan-impl-item-features</c>.
    /// </summary>
    public List<string>? Features { get; set; }

    /// <summary>
    /// The type of matrix used for this item. Usually from thesaurus
    /// <c>fig-plan-impl-matrix-types</c>.
    /// </summary>
    public string? MatrixType { get; set; }

    /// <summary>
    /// The state of the matrix (e.g. a woodblock) used to print this item.
    /// Usually from thesaurus <c>fig-plan-impl-matrix-states</c>.
    /// </summary>
    public string? MatrixState { get; set; }

    /// <summary>
    /// A free textual description of the matrix state, if any.
    /// </summary>
    public string? MatrixStateDsc { get; set; }

    /// <summary>
    /// The relative position of the item in the page (e.g. in-text, upper
    /// margin, etc.). Usually from thesaurus <c>fig-plan-impl-positions</c>.
    /// </summary>
    public string? Position { get; set; }

    /// <summary>
    /// The size of the item.
    /// </summary>
    public PhysicalSize? Size { get; set; }

    /// <summary>
    /// Labels for this item, e.g. a legend, a character name, etc.
    /// </summary>
    public List<FigPlanItemLabel>? Labels { get; set; }

    /// <summary>
    /// A free text description about all the labels of this item.
    /// </summary>
    public string? LabelDsc { get; set; }

    /// <summary>
    /// The iconography of this item, if any.
    /// </summary>
    public AssertedCompositeId? IconographyId { get; set; }

    /// <summary>
    /// Converts this item to a string.
    /// </summary>
    /// <returns>String.</returns>
    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append('[').Append(ChangeType).Append("] ");
        sb.Append(Type);
        if (!string.IsNullOrEmpty(Location))
        {
            sb.Append(" @");
            sb.Append(Location);
        }
        return sb.ToString();
    }
}
