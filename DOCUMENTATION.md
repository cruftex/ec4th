# ec4th documentation system

This repository contains two kinds of documentation:

- hand-written project docs such as `README.md` and files in `doc/`
- generated word reference pages under `output/doc/`

The generated reference is driven by a small Sphinx pipeline tailored to ec4th.

## Concepts

- **word**: the Forth token as seen by the user, for example `swap`, `+`, or `'`
- **slug**: the stable documentation id used in filenames and cross references, for example `swap`, `op-plus`, or `op-tick`
- **profile**: a concrete built image described by a tag file such as `output/ec4th-arduino-nano-regular.tags`

Use slugs as the stable identifier. A word may be symbolic, but the slug should remain filesystem-safe.

## Source layout

```text
doc/
  build_word_docs.py
  conf.py
  forth_domain.py
  python-requirements.txt
  word/*.yml

output/
  ec4th-<profile>.tags
  doc/
```

The main inputs are:

- `doc/word/*.yml`: metadata for documented words
- `output/ec4th-*.tags`: generated symbol/tag files from the build

The main outputs are:

- `output/doc/index.md`
- `output/doc/profile/<profile>/index.md`
- `output/doc/profile/<profile>/word/<slug>.md`
- `output/doc/word/<slug>.md`
- `output/doc/_build/html/`

## Word metadata files

Each documented word is defined in a YAML file in `doc/word/`.

Rules:

- the filename stem is the slug
- use `word:` when the visible Forth word differs from the slug
- symbolic operators use `op-<name>` style slugs

Example:

```yaml
word: +
wordset: core
stack: "n1 n2 -- n3"
description: Add two single-cell numbers.
```

For the example above, the filename should be `doc/word/op-plus.yml`.

Supported fields today are:

- `word`
- `f12-slug`
- `wordset`
- `description`
- `stack`

## Profile discovery

Profiles are not configured manually. The generator discovers them from files matching:

```text
output/ec4th-*.tags
```

The profile name is the part between `ec4th-` and `.tags`.

Example:

```text
output/ec4th-arduino-nano-regular.tags
```

This produces the profile id `arduino-nano-regular`.

## How generation works

`doc/build_word_docs.py` performs the first stage:

1. Load YAML metadata from `doc/word/`.
2. Parse each discovered tags file.
3. Resolve a slug for every word.
4. Generate per-profile pages.
5. Generate global word pages and the root index.

Resolution rules:

- profile pages first resolve links within the same profile, then fall back to global pages
- global pages only resolve against global entries

If a word appears in a tags file but has no YAML entry yet, the generator creates a fallback slug from the visible word so the page can still be generated.

## Cross references

Always use the custom Forth domain roles, never raw relative links.

Examples:

```md
{forth:word}`swap`
{forth:op}`plus`
```

Use:

- `{forth:word}` for normal slugs
- `{forth:op}` for `op-...` slugs

The `forth:op` role maps `plus` to the slug `op-plus`.

## Build workflow

Build the firmware first so tag files exist:

```bash
make
```

Then install the Python documentation dependencies:

```bash
python3 -m pip install -r doc/python-requirements.txt
```

Generate and build the docs:

```bash
make doc
```

That currently runs:

```bash
python3 doc/build_word_docs.py
sphinx-build -b html -c doc output/doc output/doc/_build/html
```

## Maintainer constraints

- slugs are stable once published
- do not hardcode `.html` links in source documents
- prefer domain roles over raw links
- keep generated output under `output/doc/`
- Sphinx should build without warnings

The generator may recreate `output/doc/`, so do not hand-edit generated pages there. Edit YAML metadata or the generator instead.
