using System.Collections.Generic;

namespace Cadmus.NdpFrac.Parts;

/// <summary>
/// Data about ruling in a codicological fragment.
/// </summary>
public class CodFrRuling
{
    /// <summary>
    /// The features of the ruling, like "a secco", "a mina", "a inchiostro", etc.
    /// Usually from thesaurus <c>cod-fr-ruling-features</c>.
    /// </summary>
    public List<string> Features { get; set; } = [];

    /// <summary>
    /// The ruling type, usually from thesaurus <c>cod-fr-ruling-types</c>.
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// The ruling system. Usually from thesaurus <c>cod-fr-ruling-systems</c>.
    /// </summary>
    public string? System { get; set; }

    /// <summary>
    /// A general note about the ruling.
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Converts this instance to a string representation.
    /// </summary>
    /// <returns>String.</returns>
    public override string ToString()
    {
        return string.Join(", ", Features) +
            (Type != null ? ": " + Type : "");
    }
}
