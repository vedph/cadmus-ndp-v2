using System;
using System.Collections.Generic;
using System.Text;
using Cadmus.Core;
using Fusi.Tools.Configuration;

namespace Cadmus.Ndp.Parts;

/// <summary>
/// Notable word forms list part.
/// <para>Tag: <c>it.vedph.ndp.notable-word-forms</c>.</para>
/// </summary>
[Tag("it.vedph.ndp.notable-word-forms")]
public sealed class NotableWordFormsPart : PartBase
{
    /// <summary>
    /// Gets or sets the forms.
    /// </summary>
    public List<NotableWordForm> Forms { get; set; } = [];

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

        builder.Set("tot", Forms?.Count ?? 0, false);

        if (Forms?.Count > 0)
        {
            HashSet<string> languages = [];
            HashSet<string> values = [];
            HashSet<string> tags = [];
            HashSet<string> refs = [];

            foreach (NotableWordForm form in Forms)
            {
                // eid
                if (!string.IsNullOrEmpty(form.Eid))
                    builder.AddValue("eid", form.Eid);
                // language
                if (!string.IsNullOrEmpty(form.Language))
                    languages.Add(form.Language);
                // value
                values.Add(form.Value);
                // tags
                if (form.Tags?.Count > 0)
                {
                    foreach (string tag in form.Tags) tags.Add(tag);
                }
                // reference form
                if (!string.IsNullOrEmpty(form.ReferenceForm))
                    refs.Add(form.ReferenceForm);
            }

            // languages
            foreach (string lang in languages)
                builder.AddValue("language", lang);
            // values
            foreach (string value in values)
                builder.AddValue("form", value);
            // tags
            foreach (string tag in tags)
                builder.AddValue("tag", tag);
            // reference forms
            foreach (string rf in refs)
                builder.AddValue("ref-form", rf);
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
               "The total count of forms."),
            new DataPinDefinition(DataPinValueType.String,
                "eid",
                "The forms EIDs.",
                "M"),
            new DataPinDefinition(DataPinValueType.String,
                "language",
                "The forms languages.",
                "M"),
            new DataPinDefinition(DataPinValueType.String,
                "form",
                "The forms values.",
                "M"),
            new DataPinDefinition(DataPinValueType.String,
                "tag",
                "The form tags.",
                "M"),
            new DataPinDefinition(DataPinValueType.String,
                "ref-form",
                "The form reference forms.",
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

        sb.Append("[NotableWordForms]");

        if (Forms?.Count > 0)
        {
            sb.Append(' ');
            int n = 0;
            foreach (var entry in Forms)
            {
                if (++n > 3) break;
                if (n > 1) sb.Append("; ");
                sb.Append(entry);
            }
            if (Forms.Count > 3)
                sb.Append("...(").Append(Forms.Count).Append(')');
        }

        return sb.ToString();
    }
}