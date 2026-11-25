# Cadmus Naples Dante Project V2

This is the unified backend core for Cadmus NDP. It merges the following repositories:

- [Cadmus NDP FRAC](https://github.com/vedph/cadmus-ndp-frac)
- [Cadmus NDP Books](https://github.com/vedph/cadmus-ndp-books)
- [Cadmus NDP Drawings](https://github.com/vedph/cadmus-ndp-drawings)

These repositories are now archived and no longer maintained. Please use this repository instead, which provides:

- centralized codebase for all core Cadmus backend components.
- unified versioning and release management.

The projects were copied into this solution from the above repositories and then their configuration files were updated to use the new unified versioning (starting from 15.0.0) and release management. The code itself was not changed, so it is still the same as in the original repositories. Anyway, new development is done here only.

> Codicological terminology reference: <https://codicologia.irht.cnrs.fr/indexc>.

## Docker Images

🐋 Before creating Docker images, ensure you have a buildx builder instance running that supports multi-arch and that you have run `buildnpub.ps1` publishing NuGet packages:

```sh
docker buildx create --use --name multi-arch-builder || docker buildx use multi-arch-builder
docker buildx inspect --bootstrap
```

Also, be sure to have run `buildnpub.ps1` for both local and remote NuGet feeds, so that the .NET CLI can restore all dependencies.

> To run natively on Linux VMs, macOS (both Intel and Apple Silicon), and Windows (via WSL2 or Docker Desktop)—`linux/amd64` and `linux/arm64` are the only two targets we need. Note that `docker buildx` automatically injects variables like `TARGETARCH` and `TARGETOS` into the scope of your build. In `Dockerfile` we pass these directly to the .NET CLI commands.

These commands build for multiple platforms and push directly to Docker Hub:

- 🐋 **Cadmus.Ndp.Api**:

```sh
docker buildx build --platform linux/amd64,linux/arm64 -t vedph2020/cadmus-ndp-api:18.1.0 -t vedph2020/cadmus-ndp-api:latest --push .
```

> When there is no available ARM image, add `platform: linux/amd64` as a sibling of the `image` line in docker compose script to force Docker to pull and run the x64 image under emulation, e.g.:

- add `platform: linux/arm64` to MongoDB and PostgreSQL. Note that some images (especially for PostgreSQL) are not available for ARM, so you may need to add `platform: linux/amd64` to them too or downgrade to a version which supports ARM (e.g. `image: postgres:16`).
- add `platform: linux/amd64` to API and app.

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
    - [chronotopes](https://github.com/vedph/cadmus-general/blob/master/docs/chronotopes.md):`prn` 🚩 print date and/or place pair(s).
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

- **stemma**: a coat-of-arms found in manuscripts:
  - _identity_
    - [metadata](https://github.com/vedph/cadmus-general/blob/master/docs/metadata.md): generic metadata.
    - [links](https://github.com/vedph/cadmus-general/blob/master/docs/pin-links.md) 🔗 `manuscript`
  - _content_
    - [location](https://github.com/vedph/cadmus-codicology/blob/master/docs/cod-location-ranges.md) (COD): location(s) in manuscript.
    - [note](https://github.com/vedph/cadmus-general/blob/master/docs/note.md):`dsc` 🚩 (description)
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

| part                                 | manuscript | fragment | print ed.      | print inst. | drawing prj. | drawing itm.       | person | stemma |
| ------------------------------------ | ---------- | -------- | -------------- | ----------- | ------------ | ------------------ | ------ | ------ |
| bindings (COD)                       | X          |          |                | X           | drp          |                    |        |        |
| categories                           |            |          |                |             |              |                    | bio    |        |
| chronotopes                          | X          | X        | prn pub        |             | X            | X                  |        |        |
| comment                              |            |          |                |             | X            |                    |        |        |
| contents (COD)                       | X          | X        |                |             |              |                    |        |        |
| decorated counts                     |            | X        |                |             | drp          |                    |        |        |
| decorations (COD)                    | X          | X        | X              | X           |              |                    |        |        |
| drawing tech (DRW)                   |            |          |                |             |              | X                  |        |        |
| edits (COD)                          | X          | X        |                | X           |              | X                  |        |        |
| flags                                |            |          |                |             |              | dri                |        |        |
| figurative plan (BOK)                |            |          | X              |             |              |                    |        |        |
| figurative plan implementation (BOK) |            |          |                | X           |              |                    |        |        |
| fonts (BOK)                          |            |          | X              |             |              |                    |        |        |
| hands (COD)                          | X          | X        |                | X           |              |                    |        |        |
| historical events                    | ms         | fr       |                | pri         | drp          | dri                | bio    |        |
| iconography instructions (ICO)       | X          | X        |                |             |              | X                  |        |        |
| labels (FRA)                         |            | X sig    |                |             |              |                    |        |        |
| layouts (COD)                        | X          |          | X              | X           |              |                    |        |        |
| layout (FRA)                         |            | X        |                |             |              |                    |        |        |
| location (DOC)                       |            |          |                |             |              |                    |        | X      |
| links                                | X          | X        | X              | X           | X            | X                  | X      | X      |
| material description (COD)           | X          |          |                |             |              |                    |        |        |
| measurements                         |            | fr       |                | pri         |              |                    |        |        |
| metadata                             | X          | X        | X              | X           | X            | X                  | X      | X      |
| names                                |            |          |                |             |              |                    | X      |        |
| notable text passages (NDP)          | X          | X        | X              | X           |              |                    |        |        |
| note                                 | X hist     | X hist   | X hist inc col | X hist      | X hist       | X hist isd ift msc | X hist | X dsc  |
| preservation states                  | X          | X        |                | X           | X            | X                  |        |        |
| references                           | X          | X        | X              | X           | X            | X                  | X      | X      |
| rulings (FRA)                        |            | X        |                |             |              |                    |        |        |
| shelfmarks (COD)                     | X          | X        |                | X           | X            | X                  |        |        |
| sheet labels (COD)                   | X          |          |                | X           |              |                    |        |        |
| support (FRA)                        |            | X        |                |             |              |                    |        |        |
| watermarks (COD)                     | X          |          | X              |             |              | X                  |        |        |

💡 Notes:

- `categories:txt` can be used to include the generic text classification which cross-references all the text-carrier entities.
- `categories:edits` in print instances is a special classification for "postille". We included it in `categories:txt`.
- `note:hist` is the narrative history of the entity. Usually its most relevant events are extracted and formalized into historical events.
- `note` is a generic note, mostly used for editorial purposes.
- bibliography is hosted in `references` via Zotero lookup. In this, the `citation` property of each reference (a string) will hold a combination of the Zotero library ID and item ID, uniquely identifying each bibliographic entry.

## Iconographic Links

Since version 16, the iconography facet has been removed and everything about it has been outsourced via links to authority lists, whether they are external or internal (taxonomies or items). The following just reflect the practical guidelines used in this project.

For instance, each element of a decoration in the decorations part can have links. These can target:

- the entry of an external authority list (e.g. Biblissima+, MOL, etc.);
- the entry of an internal taxostore list (e.g. descriptors, subjects, topics and possibly artists if we manage to get our maximum-coverage list);
- an internal item (e.g. a person, manuscript, print, fragment, drawing, etc.).

As for **taxonomies**, currently we will define the following:

- 🟢 **descriptors** (IDP "parole chiave"): the lowest level of description, referring to the minimal elements within a depiction that are identified as relevant—at the most basic and objective level possible: the "things" one sees in an image. Consequently, their syntax typically follows the pattern N(A), where N stands for a noun and A for an adjective (e.g., "demon," "garment," "curved staff"). In a scene depicting a resurrection, one might identify elements such as "rock," "sun," "plant," or "shroud," even without knowing the underlying "story". How these elements are to be interpreted and connected belongs to a higher level of abstraction: the "subjects".
- 🟢 **subjects** (IDP "soggetti"): the narrative "glue" that binds objects together into a story; these are far more culturally loaded. Their typical syntax is (S)VO, where S stands for subject, V for a verb or a noun expressing an action (e.g., "vision"), and O for any complement (e.g., "Dante wanders through the dark forest," "Virgil rides on the back of Geryon," etc.). As these must function as entries in an authority list, they are kept as minimalist as possible.
- 🟢 **topics** (IDP "categorie"): specifically "Dantean themes"—for example, "exiting Hell," "explanation of the spots on the moon," or "Dante asleep." These are tailored to the _Divine Comedy_ and describe specific passages from the text from a thematic perspective. Therefore, they only make sense within the context of the poem—much like how a "dark forest" (_selva oscura_) in the context of Dante differs from a geographical location (e.g., "Constantinople").
- 🟢 **toponyms** (Dantean locations): place names from the geography of the _Divine Comedy_, organized in a hierarchical structure (e.g., "Inferno," "Inferno: Dark Forest," "Inferno: Acheron," "Inferno: First Circle – Limbo," etc.). This is a closed list.

> Additionally, when an object links to a specified text passage (e.g. "If. I 2-3") this is usually represented by citations in a set of document references.

**Suggested tests** for defining descriptors or subjects:

1. When the suggested syntax—such as N(A) [noun modified by an adjective/attribute] or SVO [subject-verb-object]—proves insufficient (for instance, if a subject requires two verbs), it is usually an indication that the entry should be broken down into several distinct entries.
2. When a proposed descriptor encompasses terms that could themselves function as descriptors, it is better to limit the descriptors to those individual terms and, if necessary, define a "subject" to represent their combination—unless that combination possesses a strong, reusable connection and significance.
   This second point is best clarified through a linguistic metaphor: in figurative language, think of descriptors as "words" and subjects as "propositions." If an image contains, among other elements, a dove, God the Father, and Christ, these are three separate descriptors. The descriptor "dove" can easily appear elsewhere to form different propositions — not only alongside God the Father and Christ, but also, for example, as a symbol of peace or love. Therefore, treating "dove + God the Father + Christ" as a single, indivisible "word" would be problematic, as one of its components can appear independently in other contexts. Instead, the Trinity—as represented here—is expressed through a proposition composed of these three "words," which are linked precisely because at least one of them can function independently within other propositions.

Thus, in this ascending scale from the simple to the complex, we have **three levels**:

1. **descriptors**: the "words."
2. **subjects**: the "propositions."
3. **free text**: the "sentences." By analogy, one might also consider the concept of compounding: _chemin-de-fer_ functions as a single ‘word’ because its three components, taken together, convey a unique, unified meaning; yet _chemin_ is frequently used elsewhere as an independent word in all sorts of contexts. Our objective is to establish two broad categories—one situated at the lower end of the spectrum of signifiers—comprising words that serve as ‘elements’ or building blocks for propositions and sentences; these are what we term ‘descriptors’. Consequently, we do not classify an expression as a descriptor if it is composed of a word that can also appear in isolation. Instead, we would distinguish between _chemin_ and _fer_ on one side (as descriptors) and _chemin-de-fer_ on the other (as a compound expression possessing its own distinct, unitary meaning—a subject). In any case, just as a compound term occupies a boundary zone between a full proposition and a single word—a distinction that can only be determined through philological and linguistic analysis of a given text—so too can descriptors occasionally be composite in nature.

By analogy, one might also consider the concept of compounding: _chemin-de-fer_ functions as a single ‘word’ because its three components, taken together, convey a unique, unified meaning; yet _chemin_ is frequently used elsewhere as an independent word in all sorts of contexts. Our objective is to establish two broad categories—one situated at the lower end of the spectrum of signifiers, comprising words that serve as ‘elements’ or building blocks for propositions and sentences; these are what we term ‘descriptors’. Consequently, we do not classify an expression as a descriptor if it is composed of a word that can also appear in isolation. Instead, we would distinguish between _chemin_ and _fer_ on one side (as descriptors) and _chemin-de-fer_ on the other (as a compound expression possessing its own distinct, unitary meaning—a subject). In any case, just as a compound term occupies a boundary zone between a full proposition and a single word—a distinction that can only be determined through philological and linguistic analysis of a given text—so too can descriptors occasionally be composite in nature.

### Descriptors: Number

As a general rule, descriptors are always used in the **singular**, regardless of the number of entities present in a scene. If there are two dogs in a scene, we still use the term "dog", because we are describing _classes_ rather than individual _instances_. We are only concerned with the fact that one or more instances of the "dog" class exist.

If we were to use the plural descriptor "dogs", we would also need a separate entry for "dog" to account for cases where only one is present; however, this is merely an incidental detail: it relates to specific, concrete instances, which vary depending on the scene, rather than to the classes themselves. Our goal is to create a generic, reusable classification system. Therefore, we simply use the form you would find as a dictionary headword (the singular "dog") regardless of the actual number of dogs, because our objective is to identify every instance where a dog appears, whether alone or in the company of others.

Exceptions occur in cases where a number is an integral part of the nature of a descriptor, for instance, "anime purganti", which represent an indistinct throng of multiple, unidentified individuals that collectively form a single entity. To use a linguistic metaphor, one can distinguish between a "singulative" plural (the sum of many separate, countable entities) and a "collective" plural, which functions as a single, unified entity (because counting the individual components is either impossible or irrelevant; compare Italian "folla" versus "persone," or "frutta" versus the countable "frutti"). In fact, the Italian word "frutta" derives from a Latin neuter form, which itself traces back to an Indo-European collective category. In Proto-Indo-European, a "collective" number existed alongside the singular, plural, and dual (marking the threshold between singularity and plurality). This is reflected in Ancient Greek, where a neuter plural subject takes a singular verb: for example, "tà zôa trékhei" (τὰ ζῷα τρέχει), where "trékhei" is the third-person singular of the verb "to run," while "zôa" is the neuter plural (or collective) form of "zóon" ("animal"). Literally, this translates to "the animals runs," though it is more accurately understood as "the collection of animals — such as a herd — runs" (the herd being a collective expressed at the lexical, rather than the morphological, level).

Thus, a plural form may be inherent to a word (if the word exists only in the plural, which is how it would appear as a dictionary headword), or it may arise from a specific cultural connotation: for example, "anime purganti" are perceived as an indistinct mass. However, "le tre fiere" refers to three specific beasts within the context of Dante's work, rather than just three arbitrary instances of a "fiera" (beast) in a setting where there could just as easily be one, two, four, or ten.

In general, therefore, _when any number of instances might occur, we use the singular form of the class name; conversely, we use the plural if the class itself is inherently collective, or if the specific number carries a particular connotative meaning_ (e.g., "le tre fiere", "i sette sapienti" = the seven sages, etc.).

### Descriptors: Adjective

An adjective may accompany the noun within a descriptor, typically when the adjective serves to complete or further specify the meaning of the noun (e.g., "bastone ricurvo" = curved staff to denote a crozier), or to add further iconographically significant connotations (e.g. "vecchio inginocchiato" = old man kneeling).

In hierarchical taxonomies such as Iconclass, the function of these adjectives is more effectively handled by the descendant nodes within the system’s single hierarchical tree. Each single character in an Iconclass ID is a node down this hierarchy (in other terms, this ID is a monohierarchical alphanumeric string, where length equals depth). For instance, under the node for "vecchio", one might find various postures such as "seduto," "inginocchiato," "disteso," etc. In our case, however, for the sake of simplicity and compatibility with IDP and other non-hierarchical authority lists, the taxonomy of descriptors is flat.

Nevertheless, adjectives can still be used to provide further specification. It's up to the scholar to determine which specifications are relevant, that is, to decide whether an adjective serves as an essential completion or refinement of the entity expressed by the noun. Posture, for example, is technically an incidental attribute of "vecchio", yet it is significant within the iconographic tradition because it carries specific cultural connotations.

In such instances, it is appropriate to include an adjective alongside the noun. This allows users to locate both the broader category (by searching for the noun alone: "vecchio" regardless of posture, as the search function can retrieve partial matches) and the more specific category (by searching for the combination of noun and adjective: "vecchio seduto", "vecchio inginocchiato", "vecchio disteso", etc.).

### UI

In the interface, there is no specific data format for iconography; instead, the iconographic description has been broken down into its constituent parts to achieve maximum granularity and cross-applicability. In general, with regard to the two methods mentioned above—taxonomy and free text—the interface elements are as follows:

1. **taxonomy**: links. Links can function either as a dedicated "links" section for an item (when we wish to connect an entire item directly to something else) or as a property of a specific part, as defined by its underlying model. A link is, in essence, a connection: it may point externally (e.g., to MOL, Biblissima+, Iconclass, etc.), internally (to an item or a portion thereof), or to a taxonomy. A taxonomy serves as an intermediate layer between the external and internal worlds, combining the strengths of both: it is authoritative, comprehensive, and granular, much like an external authority list; yet it remains editable, like an internal resource. Consequently, it can effectively abstract away the external layer; rather than providing separate links to entries in MOL, Biblissima+, and Iconclass and then resorting to our own entry when none of those sources suffice, we simply provide a link to a single taxonomy entry. Where possible, this entry is mapped as an equivalent to corresponding entries across all the reference lists we utilize. It functions, in effect, as a hub: the user interacts with a single, unified list that can be edited as needed, expanding organically as records are populated. At the same time, this entry points to various corresponding items within other reference lists—unless, of course, no suitable match exists. We are thus presented with a single, coherent, and homogeneous interface that connects seamlessly to other resources. The user need only search within this one space: if they find a relevant entry, they use it; if not, they create a new one. Newly created entries and those already present in other lists exist on the same level; and links to other authority lists, which ensure the greatest possible interoperability, need only be defined once, when an entry is added. Every time that same entry is reused, it will carry all those external links along with it.
2. **free text**: as mentioned above, this can be a specific part of an item (if it applies to the item as a whole) or a property of a part, as defined by its model. Essentially, it is simply a text box, typically using Markdown.

This architecture is, therefore, entirely modular and symmetrical:

|           | whole item | part                    |
| --------- | ---------- | ----------------------- |
| taxonomy  | links part | links in a part's model |
| free text | note part  | note in a part's model  |

> Taxonomies in link demo: <https://cadmus-bricks-v3.fusi-soft.com/refs/asserted-composite-id>.
