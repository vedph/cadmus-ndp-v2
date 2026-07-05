# Notable Word Forms Part

NDP-specific. This is a collection of linguistically or philologically notable word forms.

🔑 `it.vedph.ndp.notable-word-forms`

- `forms`\* (`NotableWordForm[]`):
  - `eid` (`string`): the entity ID of the word form, useful when cross-referencing it or projecting data into RDF.
  - `value`\* (`string`): the word form value.
  - `language` (`string` 📚 `notable-word-forms-languages`): the language of the word form.
  - `rank` (`short`): a frequency rank for the word form. Lower is more frequent, 0 is not specified.
  - `tags` (`string[]` 📚 `notable-word-forms-tags`): tags associated to the word form (e.g. POS, linguistic phenomena, etc.).
  - `note` (`string`): an optional note about the word form.
  - `referenceForm` (`string`): a reference form for the word form. For instance, if the word form is misspelled, this can contain the correct form.
  - `operations` (`string[]`): operations to be performed to transform the word form into the reference form, or vice-versa (according to `isValueTarget`). When using operations, the 📚 `notable-word-forms-op-tags` hierarchical thesaurus can be used.
  - `isValueTarget` (`bool`): true if the operations are to be applied starting from the reference form to get the word form; false to go from the word form to the reference form.
  - `references` (🧱 [DocReference[]](https://github.com/vedph/cadmus-bricks/blob/master/docs/doc-reference.md)):
    - `type` (`string` 📚 `doc-reference-types`)
    - `tag` (`string` 📚 `doc-reference-tags`)
    - `citation` (`string`)
    - `note` (`string`)
  - `links` (🧱 [AssertedCompositeId[]](https://github.com/vedph/cadmus-bricks/blob/master/docs/asserted-composite-id.md)):
    - `target` (`PinTarget`):
      - `gid`\* (`string`)
      - `label`\* (`string`)
      - `itemId` (`string`)
      - `partId` (`string`)
      - `partTypeId` (`string`)
      - `roleId` (`string`)
      - `name` (`string`)
      - `value` (`string`)
    - `scope` (`string` 📚 `pin-link-scopes`)
    - `tag` (`string` 📚 `pin-link-tags`)
    - `assertion` (🧱 [Assertion](https://github.com/vedph/cadmus-bricks/blob/master/docs/assertion.md)):
      - `tag` (`string` 📚 `pin-link-assertion-tags`)
      - `rank` (`short`)
      - `references` (🧱 [DocReference[]](https://github.com/vedph/cadmus-bricks/blob/master/docs/doc-reference.md)): 📚 `pin-link-docref-types`, `pin-link-docref-tags`.
