# PrintFontsPart

This part contains the fonts used in the printed book, each with its features and distribution across the book sections.

- `PrintFontsPart` (`it.vedph.ndp.print-fonts`):
  - `fonts` (`PrintFont[]`):
    - `eid` (`string`)
    - `family`\* (`string` 📚 `print-font-families`): a descriptive ID like "R5" for the font family.
    - `sections` (`string[]` 📚 `print-layout-sections`, fieldset: title, body, comment, proem, other): the section(s) where the font is used.
    - `features` (`string[]` 📚 `print-font-features` flags, features of the font, especially useful when the family can't be specified: uppercase, lowercase, Roman, Gothic, Italic, Hebrew, Greek, Glagolitic, etc.)
    - `ids` ([AssertedCompositeId[]](https://github.com/vedph/cadmus-bricks/blob/master/docs/asserted-composite-id.md)): external identifiers for the font.
    - `note` (`string`)

>[Asserted composite ID brick demo](https://cadmus-bricks-v3.fusi-soft.com/refs/asserted-composite-id) - [flags brick demo](https://cadmus-bricks-v3.fusi-soft.com/ui/flag-set).

- pricking is not applicable to printed books, but its thesaurus has a n/a entry for this.
- sheet formats (one or more can be selected) for printed books are defined in features using a corresponding thesaurus (`cod-fr-layout-features`), which is role-dependent. This allows to define features only for print (for sheet formats at least), if fragments do not require them.
