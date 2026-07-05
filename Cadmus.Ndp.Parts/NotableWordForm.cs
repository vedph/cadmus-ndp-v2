using Cadmus.Refs.Bricks;
using System.Collections.Generic;
using System.Text;

namespace Cadmus.Ndp.Parts;

/// <summary>
/// A notable word form.
/// </summary>
public class NotableWordForm
{
    /// <summary>
    /// The entity ID of the word form, useful when cross-referencing it or
    /// projecting data into RDF.
    /// </summary>
    public string? Eid { get; set; }

    /// <summary>
    /// The word form value.
    /// </summary>
    public string Value { get; set; } = "";

    /// <summary>
    /// The language of the word form. Usually from thesaurus
    /// <c>notable-word-forms-languages</c>.
    /// </summary>
    public string? Language { get; set; }

    /// <summary>
    /// A frequency rank for the word form. Lower is more frequent, 0 is not
    /// specified.
    /// </summary>
    public short Rank { get; set; }

    /// <summary>
    /// Tags associated to the word form. Usually from a hierarchical thesaurus
    /// <c>notable-word-forms-tags</c>.
    /// </summary>
    public List<string>? Tags { get; set; }

    /// <summary>
    /// An optional note about the word form.
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// A reference form for the word form. For instance, if the word form
    /// is misspelled, this can contain the correct form.
    /// </summary>
    public string? ReferenceForm { get; set; }

    /// <summary>
    /// Operations to be performed to transform the word form into the reference
    /// form, or vice-versa (according to <see cref="IsValueTarget"/>).
    /// </summary>
    public List<string>? Operations { get; set; }

    /// <summary>
    /// True if the operations are to be applied starting from the reference
    /// form to get the word form; false to go from the word form to the
    /// reference form.
    /// </summary>
    public bool IsValueTarget { get; set; }

    /// <summary>
    /// Documental references about the word form.
    /// </summary>
    public List<DocReference>? References { get; set; }

    /// <summary>
    /// Links to other entities.
    /// </summary>
    public List<AssertedCompositeId>? Links { get; set; }

    /// <summary>
    /// Represents the word form as a string.
    /// </summary>
    /// <returns>String.</returns>
    public override string ToString()
    {
        StringBuilder sb = new(Value);

        // language
        if (!string.IsNullOrEmpty(Language))
            sb.Append(" (").Append(Language).Append(')');

        // tags
        if (Tags?.Count > 0)
            sb.Append(" [").Append(string.Join(", ", Tags)).Append(']');

        // reference form
        if (!string.IsNullOrEmpty(ReferenceForm))
            sb.Append(IsValueTarget? " ← " : " → ").Append(ReferenceForm);

        // eid
        if (!string.IsNullOrEmpty(Eid))
            sb.Append(" #").Append(Eid);

        return sb.ToString();
    }
}
