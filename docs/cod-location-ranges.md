# CodLocationRangesPart

A list of codicological location ranges plus a generic note.

- ranges (CodLocationRange[]):
  - endleaf (`int`): 0=none 1=start 2=end.
  - s (`string`): system
  - n\* (`int`): sheet number
  - rmn (`boolean`): Roman system for `n`
  - sfx (`string`): arbitrary suffix
  - v (`boolean?`): verso or recto or unspecified/not-applicable
  - c (`string`): column
  - l (`string`): line
  - word (`string`): reference word
- note (`string`)
