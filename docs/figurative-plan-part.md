# FigurativePlanPart

This part describes the figurative plan for the book, in general terms and for each item composing the figurative plan. As a plan, it is an _abstraction_ as well as the edition. Departures from this plan are described in the `FigurativePlanImplPart` part, which is an implementation of the plan for a specific print instance.

- `FigurativePlanPart` (`it.vedph.ndp.print-fig-plan`):
  - `artistIds` ([AssertedCompositeId[]](https://github.com/vedph/cadmus-bricks/blob/master/docs/asserted-composite-id.md)): artists identifiers, from external or internal resources, or even simple arbitrary names for unindentified artists.
  - `techniques`\* (`string[]` 📚 `fig-plan-techniques`: copper engraving, woodcut, lithograph, etching, other).
  - `items` (`FigPlanItem[]`): ordered list of items (illustrations, initials, etc.):
    - `eid`\* (`string`): a conventional human-friendly ID for the item.
    - `type`\* (`string` 📚 `fig-plan-types`: illustration, initial, scheme, diagram, frieze): type.
    - `citation` (`string`): this is a cross-project citation created according to some convention to link the figurative item to a textual passage.
  - `description` (`string`): free text description of the figurative plan.
  - `features` (`string[]` 📚 `fig-plan-features`): any set of relevant features tagged for the plan.
