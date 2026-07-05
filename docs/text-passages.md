# Text Passages Part

NDP-specific. This is a collection of text passages.

🔑 `it.vedph.ndp.text-passages`

- `passages`* (`TextPassage[]`):
  - `citation`\* (e.g. `@dc:If. I 3`): the citation representing the location of the passage in its work. It can be a single citation or a span of citations where citations are separated by ` - `.
  - `tag` (`string`, 📚 `text-passage-tags`): a generic tag used for grouping or classifying passages.
  - `features` (`string[]`, 📚 `text-passage-features`, hierarchical): a set of features attached to this passage.
  - `text` (`string`): the text passage.
  - `note` (`string`): an optional free text note.
