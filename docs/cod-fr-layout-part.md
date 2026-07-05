# CodFrLayoutPart

The layout the codicological fragment was part of.

- `CodFrLayoutPart` (`it.vedph.ndp.cod-fr-layout`):
  - `formula`\* (`string`): the layout formula. Usually this follows [Bianconi-Orsini](https://github.com/vedph/cod-layout-view?tab=readme-ov-file#bianconi-orsini): see D. Bianconi, _I Codices Graeci Antiquiores tra scavo e biblioteca_, in _Greek Manuscript Cataloguing: Past, Present, and Future_, edited by P. Degni, P. Eleuteri, M. Maniaci, Turnhout, Brepols, 2018 (Bibliologia, 48), 99-135, especially 110-111.
  - `dimensions` (`PhysicalDimension[]`): dimensions of any measurable elements in the layout, including those automatically derived from the layout formula.
  - `pricking`\* (`string`, 📚 `cod-fr-layout-prickings`): the pricking type (including no pricking), usually from thesaurus.
  - `columnCount`\* (`int`): the columns count, when it can be determined. Otherwise it is 0.
  - `counts` (`DecoratedCount[]`): any type of countable elements in the layout with their value.
  - `features` (`string[]` 📚 `cod-fr-layout-features`): custom binary features.
  - `note` (`string`): free text note.
