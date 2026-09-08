# Cadmus Naples Dante Project V2

This is the unified backend core for Cadmus NDP. It merges the following repositories:

- [Cadmus NDP FRAC](https://github.com/vedph/cadmus-ndp-frac)
- [Cadmus NDP Books](https://github.com/vedph/cadmus-ndp-books)
- [Cadmus NDP Drawings](https://github.com/vedph/cadmus-ndp-drawings)

These repositories are now archived and no longer maintained. Please use this repository instead, which provides:

- centralized codebase for all core Cadmus backend components.
- unified versioning and release management.

The projects were copied into this solution from the above repositories and then their configuration files were updated to use the new unified versioning (starting from 15.0.0) and release management. The code itself was not changed, so it is still the same as in the original repositories. Anyway, new development is done here only.

## Docker Images

🐋 Before creating Docker images, ensure you have a buildx builder instance running that supports multi-arch:

```sh
docker buildx create --use --name multi-arch-builder || docker buildx use multi-arch-builder
docker buildx inspect --bootstrap
```

> To run natively on Linux VMs, macOS (both Intel and Apple Silicon), and Windows (via WSL2 or Docker Desktop)—`linux/amd64` and `linux/arm64` are the only two targets we need. Note that `docker buildx` automatically injects variables like `TARGETARCH` and `TARGETOS` into the scope of your build. In `Dockerfile` we pass these directly to the .NET CLI commands.

These commands build for multiple platforms and push directly to Docker Hub:

- 🐋 **Cadmus.Ndp.Api**:

```sh
docker buildx build --platform linux/amd64,linux/arm64 -t vedph2020/cadmus-ndp-api:2.0.11 -t vedph2020/cadmus-ndp-api:latest --push .
```

## Facets

The list of facets is given here with their conventional groupings used in the editor UI. The 3-letters abbreviation after each part type name refers to Cadmus model spaces different from the generic one. Here we have `COD`=codicology, `FRA`=fragments, `BOK`=books, `DRW`=drawings, `ICO`=iconography. Also, 🔗 means a potential _internal_ link; where there is a links part without this indication, the usual implication is that it contains _external_ links.

> All items have a metadata part where the metadatum named `eid` can be used to provide a human-friendly identifier for the item as a whole, to be used when looking up items in internal linking. This is an opt-in mechanism: like in TEI you add an `xml:id` attribute to those elements you want to reference from another part of the document, here you add an `eid` to those items you want to link from another item's part.

- **manuscript**
  - _identity_
    - [metadata](https://github.com/vedph/cadmus-general/blob/master/docs/metadata.md): generic metadata.
    - [shelfmarks](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-shelfmarks.md) (COD): shelfmark code(s).
    - [links](https://github.com/vedph/cadmus-general/blob/master/docs/pin-links.md): generic links.
  - _history_
    - [chronotopes](https://github.com/vedph/cadmus-general/blob/master/docs/chronotopes.md): date and/or place pair(s).
    - [historical events](https://github.com/vedph/cadmus-general/blob/master/docs/historical-events.md):`ms` 🚩 historical events for granular history. This requires a specific ontology.
    - [note](https://github.com/vedph/cadmus-general/blob/master/docs/note.md):`hist` (history) 🚩 free text about the manuscript's history.
  - _material_
    - [bindings](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-bindings.md) (COD): binding(s).
    - [sheet labels](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-sheet-labels.md) (COD): sheet labels.
    - [material description](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-material-dsc.md) (COD): material description.
    - [watermarks](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-watermarks.md) (COD): watermark(s).
    - [preservation states](https://github.com/vedph/cadmus-general/blob/master/docs/physical-states.md): material preservation state(s)..
  - _content_
    - [contents](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-contents.md) (COD): textual contents.
    - [layouts](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-layouts.md) (COD): layout(s).
    - [decorations](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-decorations.md) (COD): decorations. 🔗 `person`. Each decoration element typically links to artist, topic, subject and descriptors, all from a taxonomies store.
    - [iconography instructions](https://github.com/vedph/cadmus-iconography/blob/master/docs/ico-instructions.md) (ICO): instructions to the artist.
    - [hands](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-hands.md) (COD): hands. 🔗 `person`
    - [edits](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-edits.md) (COD): edits after the manuscript composition. 🔗 `person`
    - [text passages](https://github.com/vedph/cadmus-ndp/blob/master/docs/text-passages.md) (NDP): relevant text passages.
  - _editorial_
    - [note](https://github.com/vedph/cadmus-general/blob/master/docs/note.md): editorial note.
  - _references_
    - [references](https://github.com/vedph/cadmus-general/blob/master/docs/doc-references.md): documental references, mostly used for Zotero bibliography.

- **fragment**
  - _identity_
    - [metadata](https://github.com/vedph/cadmus-general/blob/master/docs/metadata.md): generic metadata.
    - [shelfmarks](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-shelfmarks.md) (COD): shelfmark code(s) for the fragment.
    - [links](https://github.com/vedph/cadmus-general/blob/master/docs/pin-links.md): generic links. 🔗 `manuscript` (+group): the manuscript this fragment belonged to (when available).
  - _history_
    - [chronotopes](https://github.com/vedph/cadmus-general/blob/master/docs/chronotopes.md): date and/or place pair(s).
    - [historical events](https://github.com/vedph/cadmus-general/blob/master/docs/historical-events.md):`fr` 🚩 historical events for granular history. This requires a specific ontology.
    - [note](https://github.com/vedph/cadmus-general/blob/master/docs/note.md):`hist` (history) 🚩 free text about the manuscript's history.
  - _material_
    - [support](https://github.com/vedph/cadmus-ndp-frac/blob/master/docs/cod-fr-support-part.md) (FRA): material support.
    - [rulings](https://github.com/vedph/cadmus-ndp-frac/blob/master/docs/cod-fr-rulings-part.md) (FRA): rulings.
    - [labels](https://github.com/vedph/cadmus-ndp-frac/blob/master/docs/cod-fr-quire-labels-part.md):`catch` (FRA) 🚩 catchwords.
    - [labels](https://github.com/vedph/cadmus-ndp-frac/blob/master/docs/cod-fr-quire-labels-part.md):`sig` (FRA) 🚩 signatures.
    - [labels](https://github.com/vedph/cadmus-ndp-frac/blob/master/docs/cod-fr-quire-labels-part.md) (FRA): quires.
    - [decorated counts](https://github.com/vedph/cadmus-general/blob/master/docs/decorated-counts.md):`fr` 🚩 various counts.
    - [measurements](https://github.com/vedph/cadmus-general/blob/master/docs/physical-measurements.md):`fr` 🚩 various measurements.
    - [preservation states](https://github.com/vedph/cadmus-general/blob/master/docs/physical-states.md): material preservation state(s).
  - _content_
    - [contents](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-contents.md) (COD): textual contents.
    - [layout](https://github.com/vedph/cadmus-ndp-frac#codfrlayoutpart) (FRA): layout.
    - [decorations](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-decorations.md) (COD): decorations. 🔗 `person`. Each decoration element typically links to artist, topic, subject and descriptors, all from a taxonomies store.
    - [iconography instructions](https://github.com/vedph/cadmus-iconography/blob/master/docs/ico-instructions.md) (ICO): instructions to the artist.
    - [hands](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-hands.md) (COD): hands. 🔗 `person`
    - [edits](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-edits.md) (COD): edits after the manuscript composition. 🔗 `person`
    - [text passages](https://github.com/vedph/cadmus-ndp/blob/master/docs/text-passages.md) (NDP): relevant text passages.
  - _editorial_
    - [note](https://github.com/vedph/cadmus-general/blob/master/docs/note.md): editorial note.
  - references
    - [references](https://github.com/vedph/cadmus-general/blob/master/docs/doc-references.md): documental references, mostly used for Zotero bibliography.

- **print edition**
  - _identity_
    - [metadata](https://github.com/vedph/cadmus-general/blob/master/docs/metadata.md): generic metadata.
    - [links](https://github.com/vedph/cadmus-general/blob/master/docs/pin-links.md) (this can include also authors and editors) 🔗 `person`
  - _history_
    - [chronotopes](https://github.com/vedph/cadmus-general/blob/master/docs/chronotopes.md):`prn`  🚩 print date and/or place pair(s).
    - [chronotopes](https://github.com/vedph/cadmus-general/blob/master/docs/chronotopes.md):`pub` 🚩 publication date and/or place pair(s).
  - _content_
    - [fonts](https://github.com/vedph/cadmus-ndp-books/blob/master/docs/print-fonts-part.md) (BOK): fonts used in print.
    - [layouts](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-layouts.md):`prn` 🚩 (COD): layouts.
    - [watermarks](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-watermarks.md) (COD): watermarks.
    - [figurative plan](https://github.com/vedph/cadmus-ndp-books/blob/master/docs/figurative-plan-part.md) (BOK): figurative plan. 🔗 `person`
    - [note](https://github.com/vedph/cadmus-general/blob/master/docs/note.md):`inc` 🚩 incipit transcription.
    - [note](https://github.com/vedph/cadmus-general/blob/master/docs/note.md):`col` 🚩 colophon transcription.
    - [text passages](https://github.com/vedph/cadmus-ndp/blob/master/docs/text-passages.md) (NDP): relevant text passages.
  - _editorial_
    - [note](https://github.com/vedph/cadmus-general/blob/master/docs/note.md): editorial note.
  - references
    - [references](https://github.com/vedph/cadmus-general/blob/master/docs/doc-references.md): documental references, mostly used for Zotero bibliography.

- **print instance**
  - _identity_
    - [metadata](https://github.com/vedph/cadmus-general/blob/master/docs/metadata.md): generic metadata.
    - [links](https://github.com/vedph/cadmus-general/blob/master/docs/pin-links.md): generic links. 🔗 `print edition` (+group)
    - [shelfmarks](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-shelfmarks.md): shelfmarks code(s).
  - _history_
    - [historical events](https://github.com/vedph/cadmus-general/blob/master/docs/historical-events.md):`pri` 🚩 historical events for granular history. This requires a specific ontology.
    - [note](https://github.com/vedph/cadmus-general/blob/master/docs/note.md):`hist` 🚩 free text about the print's history.
  - _material_
    - [bindings](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-bindings.md) (COD): binding(s).
    - [sheet labels](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-sheet-labels.md) (COD): labels.
    - [measurements](https://github.com/vedph/cadmus-general/blob/master/docs/physical-measurements.md):`pri` 🚩 various measurements.
    - [preservation states](https://github.com/vedph/cadmus-general/blob/master/docs/physical-states.md): material preservation state(s).
  - _content_
    - [layouts](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-layouts.md):`prn` 🚩 (COD): layouts.
    - [figurative plan implementation](https://github.com/vedph/cadmus-ndp-books/blob/master/docs/figurative-plan-impl-part.md) (BOK): implementation of figurative plan.
    - [decorations](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-decorations.md) (COD) decorations. 🔗 `person`. Each decoration element typically links to artist, topic, subject and descriptors, all from a taxonomies store.
    - [edits](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-edits.md) (COD): text edits after print.
    - [text passages](https://github.com/vedph/cadmus-ndp/blob/master/docs/text-passages.md) (NDP): relevant text passages.
  - _editorial_
    - [note](https://github.com/vedph/cadmus-general/blob/master/docs/note.md): editorial note.
  - _references_:
    - [references](https://github.com/vedph/cadmus-general/blob/master/docs/doc-references.md): documental references, mostly used for Zotero bibliography.

- **drawings project**
  - _identity_
    - [metadata](https://github.com/vedph/cadmus-general/blob/master/docs/metadata.md): generic metadata.
    - [links](https://github.com/vedph/cadmus-general/blob/master/docs/pin-links.md): generic links. 🔗 `person`
    - [shelfmarks](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-shelfmarks.md) (COD)
  - _history_
    - [chronotopes](https://github.com/vedph/cadmus-general/blob/master/docs/chronotopes.md)
    - [historical events](https://github.com/vedph/cadmus-general/blob/master/docs/historical-events.md):`drp` 🚩
    - [note](https://github.com/vedph/cadmus-general/blob/master/docs/note.md):`hist` 🚩 (history)
  - _material_
    - [bindings](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-bindings.md):`drp` 🚩 (COD)
    - [preservation states](https://github.com/vedph/cadmus-general/blob/master/docs/physical-states.md): material preservation state(s).
    - [decorated counts](https://github.com/vedph/cadmus-general/blob/master/docs/decorated-counts.md):`drp` 🚩
  - _content_
    - [comment](https://github.com/vedph/cadmus-general/blob/master/docs/comment.md)
  - _editorial_
    - [note](https://github.com/vedph/cadmus-general/blob/master/docs/note.md): editorial note.
  - _references_:
    - [references](https://github.com/vedph/cadmus-general/blob/master/docs/doc-references.md): documental references, mostly used for Zotero bibliography.

- **drawing item**
  - _identity_
    - [metadata](https://github.com/vedph/cadmus-general/blob/master/docs/metadata.md): generic metadata.
    - [links](https://github.com/vedph/cadmus-general/blob/master/docs/pin-links.md) (also for authors) 🔗 `drawings project` (+group) 🔗 `person`
    - [shelfmarks](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-shelfmarks.md) (COD)
  - _history_
    - [chronotopes](https://github.com/vedph/cadmus-general/blob/master/docs/chronotopes.md)
    - [historical events](https://github.com/vedph/cadmus-general/blob/master/docs/historical-events.md):`dri` 🚩
    - [note](https://github.com/vedph/cadmus-general/blob/master/docs/note.md):`hist` 🚩 (history)
  - _material_
    - [drawing tech](https://github.com/vedph/cadmus-ndp-drawings?tab=readme-ov-file#drawingtechpart) (DRW)
    - [watermarks](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-watermarks.md) (COD)
    - [preservation states](https://github.com/vedph/cadmus-general/blob/master/docs/physical-states.md): material preservation state(s).
  - _content_
    - [flags](https://github.com/vedph/cadmus-general/blob/master/docs/flags.md):`dri` 🚩 boolean drawing features.
    - [edits](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-edits.md) (COD): edits after the drawing execution.
    - [iconography instructions](https://github.com/vedph/cadmus-iconography/blob/master/docs/ico-instructions.md) (ICO): instructions to the artist.
  - _editorial_
    - [note](https://github.com/vedph/cadmus-general/blob/master/docs/note.md): editorial note.
  - _references_:
    - [references](https://github.com/vedph/cadmus-general/blob/master/docs/doc-references.md): documental references, mostly used for Zotero bibliography.

- **person**: we will be using person items only if we need to supply people not found in authority lists and/or we need to tell more about a person.
  - _identity_
    - [metadata](https://github.com/vedph/cadmus-general/blob/master/docs/metadata.md): generic metadata.
    - [names](https://github.com/vedph/cadmus-general/blob/master/docs/names.md): person name(s).
    - [categories](https://github.com/vedph/cadmus-general/blob/master/docs/categories.md):`bio` 🚩 person categories, like poet, cardinal, jurist, miniaturist, etc.
    - [links](https://github.com/vedph/cadmus-general/blob/master/docs/pin-links.md): generic links.
  - _history_
    - [historical events](https://github.com/vedph/cadmus-general/blob/master/docs/historical-events.md):`bio` 🚩 biographic events. This requires an ontology.
    - [note](https://github.com/vedph/cadmus-general/blob/master/docs/note.md):`hist` 🚩 free text about the person's history.
  - _editorial_
    - [note](https://github.com/vedph/cadmus-general/blob/master/docs/note.md): editorial note.
  - _references_:
    - [references](https://github.com/vedph/cadmus-general/blob/master/docs/doc-references.md): documental references, mostly used for Zotero bibliography.

### Parts Matrix

In this parts matrix you find one column per entity. The cell at the intersection between each column and row contains `X` when the part is present, and/or one ore more role identifiers when that part is used with a specific role (indicated with a suffix after colon in the previous list).

This table clearly shows the modular modeling architecture of Cadmus: 32 parts (=reusable self-contained models with higher abstraction levels) are used more than 100 times, across 7 different items, covering very different knowledge domains and corresponding to material and immaterial entities.

This modular architecture is mirrored in the editor UI, as each part has its own editor, which greatly speeds up development and training for end users (once you learn how to enter data in a part, you will find it again and again in the editor for other entities). This means that even within the boundaries of this project two thirds of the models are reused.

Additionally, parts distribution shows cross-project reuse in action:

- generic: 14
- codicology (COD): 10
- fragments (FRA): 4
- books (BOK): 3
- drawings (DRW): 1
- iconography (ICO): 1

As you can see, almost half of the models come from the generic domain, and almost 3/4 of them are covered by the sum of generic and Codicology models. Only slightly more than 1/4 of the models used were specifically created for this project, while still keeping their design as much abstract and reusable as possible. This way, just like now we are using these parts for this specific project, anyone will be able to reuse them for its own.

![parts distribution](parts.png)

| part                                 | manuscript | fragment | print ed.      | print inst. | drawing prj. | drawing itm.       | person |
| ------------------------------------ | ---------- | -------- | -------------- | ----------- | ------------ | ------------------ | ------ |
| bindings (COD)                       | X          |          |                | X           | drp          |                    |        |
| categories                           |            |          |                |             |              |                    | bio    |
| chronotopes                          | X          | X        | prn pub        |             | X            | X                  |        |
| comment                              |            |          |                |             | X            |                    |        |
| contents (COD)                       | X          | X        |                |             |              |                    |        |
| decorated counts                     |            | X        |                |             | drp          |                    |        |
| decorations (COD)                    | X          | X        | X              | X           |              |                    |        |
| drawing tech (DRW)                   |            |          |                |             |              | X                  |        |
| edits (COD)                          | X          | X        |                | X           |              | X                  |        |
| flags                                |            |          |                |             |              | dri                |        |
| figurative plan (BOK)                |            |          | X              |             |              |                    |        |
| figurative plan implementation (BOK) |            |          |                | X           |              |                    |        |
| fonts (BOK)                          |            |          | X              |             |              |                    |        |
| hands (COD)                          | X          | X        |                | X           |              |                    |        |
| historical events                    | ms         | fr       |                | pri         | drp          | dri                | bio    |
| iconography instructions (ICO)       | X          | X        |                |             |              | X                  |        |
| labels (FRA)                         |            | X sig    |                |             |              |                    |        |
| layouts (COD)                        | X          |          | X              | X           |              |                    |        |
| layout (FRA)                         |            | X        |                |             |              |                    |        |
| links                                | X          | X        | X              | X           | X            | X                  | X      |
| material description (COD)           | X          |          |                |             |              |                    |        |
| measurements                         |            | fr       |                | pri         |              |                    |        |
| metadata                             | X          | X        | X              | X           | X            | X                  | X      |
| names                                |            |          |                |             |              |                    | X      |
| notable text passages (NDP)          | X          | X        | X              | X           |              |                    |        |
| note                                 | X hist     | X hist   | X hist inc col | X hist      | X hist       | X hist isd ift msc | X hist |
| preservation states                  | X          | X        |                | X           | X            | X                  |        |
| references                           | X          | X        | X              | X           | X            | X                  | X      |
| rulings (FRA)                        |            | X        |                |             |              |                    |        |
| shelfmarks (COD)                     | X          | X        |                | X           | X            | X                  |        |
| sheet labels (COD)                   | X          |          |                | X           |              |                    |        |
| support (FRA)                        |            | X        |                |             |              |                    |        |
| watermarks (COD)                     | X          |          | X              |             |              | X                  |        |

💡 Notes:

- `categories:txt` can be used to include the generic text classification which cross-references all the text-carrier entities.
- `categories:edits` in print instances is a special classification for "postille". We included it in `categories:txt`.
- `note:hist` is the narrative history of the entity. Usually its most relevant events are extracted and formalized into historical events.
- `note` is a generic note, mostly used for editorial purposes.
- bibliography is hosted in `references` via Zotero lookup. In this, the `citation` property of each reference (a string) will hold a combination of the Zotero library ID and item ID, uniquely identifying each bibliographic entry.

## Iconographic Links

Since version 16, the iconography facet has been removed and everything about it has been outsourced via links to authority lists, whether they are external or internal (taxonomies or items).

For instance, each element of a decoration in the decorations part can have links. These can target:

- the entry of an external authority list (e.g. Biblissima+, MOL, etc.);
- the entry of an internal taxostore list (e.g. descriptors, subjects, topics and possibly artists if we manage to get our maximum-coverage list);
- an internal item (e.g. a person, manuscript, print, fragment, drawing, etc.).

As for taxonomies, currently we will define the following:

- **descriptors** ("parole chiave"): to be revised. Suggested syntax is S(A) ("demonio", "veste", "bastone ricurvo"). These should ideally be the minimalist elements of a description in the most objective point of view: the "things" you see in a picture. In a resurrection scene you might see things like rock, sun, plant, shroud. How they are interpreted and connected together (usually with a "story") belongs to an higher abstraction layer, which is much more culturally denoted.
- **subjects** ("soggetti"): to be revised. Suggested syntax is SVO ("Dante vaga nella selva", "Virgilio cavalca la groppa di Gerione", etc). This is the "story", the higher level of description which usually introduces some action with a subject and a complement, whence the suggested minimalist SVO syntax.
- **topic** ("macrosoggetti": temi danteschi, e.g. "simoniaci", "spiriti magni").

>Taxonomies in link demo: <https://cadmus-bricks-v3.fusi-soft.com/refs/asserted-composite-id>.

Wherever an object contains links, it could contain these links too. This allows encoding this information with a cross-item spectrum.
