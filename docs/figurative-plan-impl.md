# FigurativePlanImplPart

Implementation of a figurative plan. This contains some general data about the implementation, and specific data for each item of the implementation which with reference to the plan was changed, removed, or added.

- `FigurativePlanImplPart` (`it.vedph.ndp.print-fig-plan-impl`):
  - `isComplete`\* (`boolean`): true if the implementation is complete with reference to the plan.
  - `techniques` ⬆️ (`string[]` 📚 `fig-plan-techniques`): specified only to override the corresponding techniques in the plan. If no technique is specified, all the plan's techniques are implied. If any technique is specified, this implies that these techniques fully replace the plan's techniques.
  - `items` (`FigPlanImplItem[]`): ordered list of items (illustrations, initials, etc.). Only those items which override the plan's items are specified here. If an item is not specified, it is assumed that the implementation is the same as in the plan:
    - `eid`\* (`string`): the ID of the corresponding figurative plan item when overriding it, or a new one if added in this instance.
    - `type` (`string` 📚 `fig-plan-types`: illustration, initial, scheme, diagram, frieze): type. This overrides the plan's type.
    - `citation` (`string`): this is a cross-project citation created according to some convention to link the figurative item to a textual passage. This overides the plan's citation.
    - `location` (`string`): the page location.
    - `changeType`\* (`string`: 📚 `fig-plan-impl-change-types`: none, add, delete, replace, reuse, change, misuse): the type of change made to the item in this instance with respect to the plan. If an item is deleted, it will just include `eid` and change type. If it is replaced, it will have the same `eid` and different content, overriding the plan. If it is added, it will have a new `eid` and its own content, totally missing from the plan. The `none` change type is used when you want to add more details to an item.
    - `features` (`string[]` 📚 `fig-plan-impl-item-features`: matrix change, frame added, frame removed, frame changed, other): any relevant features of the implemented item, e.g. a frame.
    - `matrixType` (`string` 📚 `fig-plan-impl-matrix-types`): woodblock, etc.
    - `matrixState` (`string` 📚 `fig-plan-impl-matrix-states`: fine, good, fair, damaged): the state of the matrix (e.g. a woodblock) used to print this item.
    - `matrixStateDsc` (`string`): a free textual description of the matrix state.
    - `position` (`string` 📚 `fig-plan-impl-positions`: in-text, upper margin, lower margin, full page, antiporta/frontispiece, single-sheet prints (carta di tavola)): the relative position of the item in the page.
    - `size` ([PhysicalSize](https://github.com/vedph/cadmus-bricks/blob/master/docs/physical-size.md))
    - `labels` (`FigPlanItemLabel[]`): the label types found in the item: e.g. a legend for the whole image, or a character name on a character in the image, etc.:
      - `type`\* (`string` 📚 `fig-plan-item-label-types`: legend, topographic indication, character name, inscription): the label type.
      - `languages` (`string[]` 📚 `fig-plan-item-label-languages`): the language(s) used in the label.
      - `value` (`string`): the label value, e.g. the legend text, the character name, etc.
      - `note` (`string`): a free text note about the label.
      - `fonts` (`PrintFont[]`): the fonts used in the label.
    - `labelDsc` (`string`): a free textual description of label(s).
    - `iconographyId` (`AssertedCompositeId`): link to the corresponding iconography item if any.
  - `description` (`string`): free text description of the figurative plan implementation.
  - `features` ⬆️ (`string[]` 📚 `fig-plan-impl-features`: centre, frame, initial, frieze): this overrides the plan's features. If not specified, all the plan's features are implied. If any feature is specified, this implies that these features fully replace the plan's features.
