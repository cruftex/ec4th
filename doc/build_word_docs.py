#!/usr/bin/env python3
from __future__ import annotations

import argparse
import re
from dataclasses import dataclass
from pathlib import Path
from typing import Dict, List, Optional

try:
    import yaml
except ImportError as exc:
    raise SystemExit("PyYAML is required: pip install pyyaml") from exc


TAG_RE = re.compile(r'^(?P<word>\S+)\s+(?P<path>\S+)\s+(?P<line>\d+);"(?:\s.*)?$')
VARIANT_RE = re.compile(r"^ec4th-(?P<variant>.+)\.tags$")
SAFE_WORD_RE = re.compile(r"^[a-z0-9]+(?:[_-][a-z0-9]+)*$")
SYMBOL_NAME_MAP = {
    "!": "store",
    '"': "quote",
    "#": "number-sign",
    "$": "dollar",
    "%": "percent",
    "&": "and",
    "'": "tick",
    "(": "paren",
    ")": "close-paren",
    "*": "mul",
    "+": "plus",
    ",": "comma",
    "-": "minus",
    ".": "dot",
    "/": "div",
    ":": "colon",
    ";": "semicolon",
    "<": "less",
    "=": "equal",
    ">": "greater",
    "?": "question",
    "@": "fetch",
    "[": "left-bracket",
    "\\": "backslash",
    "]": "right-bracket",
    "^": "caret",
    "_": "underscore",
    "`": "backtick",
    "{": "left-brace",
    "|": "bar",
    "}": "right-brace",
    "~": "tilde",
}


@dataclass
class WordDoc:
    slug: str
    word: str
    f12_slug: Optional[str] = None
    wordset: Optional[str] = None
    also_wordsets: Optional[List[str]] = None
    description: Optional[str] = None
    stack: Optional[str] = None
    return_stack: Optional[str] = None
    source_file: Optional[Path] = None


@dataclass
class TagEntry:
    word: str
    source_path: str
    line: int


@dataclass(frozen=True)
class Variant:
    target: str
    profile: str
    tags_file: Path


@dataclass
class ProfileWord:
    target: str
    profile: str
    slug: str
    word: str
    source_path: str
    line: int
    doc: Optional[WordDoc] = None


def normalize_word_key(word: str) -> str:
    return word.strip().lower()


def load_word_docs(word_dir: Path) -> tuple[Dict[str, WordDoc], Dict[str, WordDoc]]:
    docs_by_slug: Dict[str, WordDoc] = {}
    docs_by_word: Dict[str, WordDoc] = {}

    for path in sorted(word_dir.glob("*.yml")) + sorted(word_dir.glob("*.yaml")):
        with path.open("r", encoding="utf-8") as f:
            raw = yaml.safe_load(f) or {}

        slug = path.stem
        word = str(raw.get("word") or slug)

        doc = WordDoc(
            slug=slug,
            word=word,
            f12_slug=raw.get("f12-slug"),
            wordset=raw.get("wordset"),
            also_wordsets=list(raw.get("also-wordsets") or []),
            description=raw.get("description"),
            stack=raw.get("stack"),
            return_stack=raw.get("return-stack"),
            source_file=path,
        )

        if slug in docs_by_slug:
            print(f"warning: duplicate slug {slug!r} in {path}")
        docs_by_slug[slug] = doc

        word_key = normalize_word_key(word)
        if word_key in docs_by_word:
            print(f"warning: duplicate word mapping {word!r} in {path}")
        docs_by_word[word_key] = doc

    return docs_by_slug, docs_by_word


def parse_tags_file(tags_file: Path) -> List[TagEntry]:
    entries: List[TagEntry] = []

    with tags_file.open("r", encoding="utf-8") as f:
        for lineno, raw in enumerate(f, start=1):
            line = raw.strip()
            if not line or line.startswith("!_TAG_"):
                continue

            m = TAG_RE.match(line)
            if not m:
                print(f"warning: could not parse {tags_file}:{lineno}: {line}")
                continue

            source_path = m.group("path")
            if source_path.startswith("+/"):
                source_path = source_path[2:]

            entries.append(
                TagEntry(
                    word=m.group("word"),
                    source_path=source_path,
                    line=int(m.group("line")),
                )
            )

    return entries


def split_variant_name(name: str) -> tuple[str, str]:
    if "-" not in name:
        raise SystemExit(
            f"invalid tags variant {name!r}: expected ec4th-<target>-<profile>.tags"
        )
    target, profile = name.rsplit("-", 1)
    return target, profile


def discover_variants(output_dir: Path) -> List[Variant]:
    variants: List[Variant] = []

    for path in sorted(output_dir.glob("ec4th-*.tags")):
        m = VARIANT_RE.match(path.name)
        if not m:
            continue
        target, profile = split_variant_name(m.group("variant"))
        variants.append(Variant(target=target, profile=profile, tags_file=path))

    return variants


def resolve_slug(word: str, docs_by_word: Dict[str, WordDoc]) -> str:
    doc = docs_by_word.get(normalize_word_key(word))
    if doc is not None:
        return doc.slug

    word = word.strip().lower()
    if not word:
        return "unnamed"

    if SAFE_WORD_RE.match(word):
        return word

    parts: List[str] = []
    named_parts: List[str] = []
    saw_symbol = False

    for index, char in enumerate(word):
        if char.isalnum():
            parts.append(char)
            named_parts.append(char)
            continue

        if char in ".-_":
            # Keep separator characters only when they separate alnum runs.
            prev_is_alnum = index > 0 and word[index - 1].isalnum()
            next_is_alnum = index + 1 < len(word) and word[index + 1].isalnum()
            if prev_is_alnum and next_is_alnum:
                parts.append(char)
                named_parts.append(char)
                continue

        symbol_name = SYMBOL_NAME_MAP.get(char)
        if symbol_name is not None:
            parts.append(f"-{symbol_name}-")
            named_parts.append(symbol_name)
            saw_symbol = True
        else:
            parts.append("-")
            named_parts.append("-")
            saw_symbol = True

    # Drop closing delimiters when they simply terminate a paired wrapper.
    if named_parts and named_parts[-1] == "close-paren" and "paren" in named_parts[:-1]:
        parts = parts[:-1]
    elif named_parts and named_parts[-1] == "right-bracket" and "left-bracket" in named_parts[:-1]:
        parts = parts[:-1]
        for index, part in enumerate(parts):
            if part == "-left-bracket-":
                parts[index] = "-bracket-"
                break
    elif named_parts and named_parts[-1] == "right-brace" and "left-brace" in named_parts[:-1]:
        parts = parts[:-1]

    slug = "".join(parts)
    slug = re.sub(r"[^a-z0-9._-]+", "-", slug)
    slug = re.sub(r"-{2,}", "-", slug)
    slug = re.sub(r"(^|-)m-mul(?=-|$)", r"\1m-times", slug)
    slug = re.sub(r"(^|-)um-mul(?=-|$)", r"\1um-times", slug)
    slug = re.sub(r"(^|-)two-mul(?=-|$)", r"\1two-times", slug)
    slug = slug.strip("-")

    if not slug:
        return "unnamed"
    if saw_symbol:
        return f"op-{slug}"
    return slug


def build_profile_words(
    variant: Variant,
    entries: List[TagEntry],
    docs_by_slug: Dict[str, WordDoc],
    docs_by_word: Dict[str, WordDoc],
) -> List[ProfileWord]:
    seen_words: set[str] = set()
    seen_slugs: set[str] = set()
    resolved: List[ProfileWord] = []

    for entry in entries:
        if entry.word in seen_words:
            print(
                f"warning: duplicate word {entry.word!r} in target={variant.target!r} "
                f"profile={variant.profile!r}; keeping first"
            )
            continue
        seen_words.add(entry.word)

        slug = resolve_slug(entry.word, docs_by_word)
        if slug in seen_slugs:
            print(
                f"warning: duplicate slug {slug!r} in target={variant.target!r} "
                f"profile={variant.profile!r}; keeping first"
            )
            continue
        seen_slugs.add(slug)

        doc = docs_by_slug.get(slug)

        resolved.append(
            ProfileWord(
                target=variant.target,
                profile=variant.profile,
                slug=slug,
                word=entry.word,
                source_path=entry.source_path,
                line=entry.line,
                doc=doc,
            )
        )

    resolved.sort(key=lambda x: (x.word.lower(), x.word, x.slug))
    return resolved


def ensure_dir(path: Path) -> None:
    path.mkdir(parents=True, exist_ok=True)


def write_text(path: Path, text: str) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    rendered = text.rstrip() + "\n"
    if path.exists() and path.read_text(encoding="utf-8") == rendered:
        return
    path.write_text(rendered, encoding="utf-8")


def prune_stale_files(root: Path, wanted_files: set[Path]) -> None:
    if not root.exists():
        return

    for path in sorted(root.rglob("*.md"), reverse=True):
        if path not in wanted_files:
            path.unlink()

    for path in sorted((p for p in root.rglob("*") if p.is_dir()), reverse=True):
        try:
            path.rmdir()
        except OSError:
            pass


def md_escape_inline(text: str) -> str:
    return text.replace("\\", "\\\\").replace("`", "\\`")


def yaml_quote(text: str) -> str:
    escaped = text.replace("\\", "\\\\").replace('"', '\\"')
    return f'"{escaped}"'


def display_word(doc: Optional[WordDoc], fallback_word: str) -> str:
    if doc is not None:
        return doc.word
    return fallback_word.lower()


def iter_wordsets(doc: Optional[WordDoc]) -> List[str]:
    if doc is None:
        return ["other"]

    wordsets: List[str] = []
    if doc.wordset is not None:
        wordsets.append(doc.wordset)
    for wordset in doc.also_wordsets or []:
        if wordset not in wordsets:
            wordsets.append(wordset)
    if not wordsets:
        return ["other"]
    return wordsets


def source_url(source_path: str, line: int) -> str:
    return f"/source/{source_path}.html#line-{line}"


def standard_wordset_path(wordset: str) -> str:
    if wordset.startswith("core"):
        return "core"
    if wordset.startswith("double"):
        return "double"
    if wordset.startswith("exception"):
        return "exception"
    if wordset.startswith("facility"):
        return "facility"
    if wordset.startswith("floating"):
        return "float"
    if wordset.startswith("memory"):
        return "memory"
    if wordset.startswith("string"):
        return "string"
    if wordset.startswith("tools"):
        return "tools"
    return wordset


def standard_url(wordset: Optional[str], f12_slug: Optional[str]) -> Optional[str]:
    if not wordset or not f12_slug:
        return None
    return f"https://forth-standard.org/standard/{standard_wordset_path(wordset)}/{f12_slug}"


def render_word_directive(
    slug: str,
    word: str,
    target: Optional[str] = None,
    profile: Optional[str] = None,
) -> str:
    lines = [f"```{{forth:word}} {slug}", f":word: {yaml_quote(word)}"]
    if target:
        lines.append(f":target: {yaml_quote(target)}")
    if profile:
        lines.append(f":profile: {yaml_quote(profile)}")
    lines.append("```")
    return "\n".join(lines)


def render_profile_word_page(pw: ProfileWord) -> str:
    title = pw.doc.word if pw.doc is not None else pw.word
    desc = pw.doc.description if pw.doc is not None else None
    stack = pw.doc.stack if pw.doc is not None else None
    return_stack = pw.doc.return_stack if pw.doc is not None else None
    wordsets = iter_wordsets(pw.doc) if pw.doc is not None else []
    f12_slug = pw.doc.f12_slug if pw.doc is not None else None
    std_url = standard_url(wordsets[0] if wordsets else None, f12_slug)

    parts: List[str] = []
    parts.append("---")
    parts.append("orphan: true")
    parts.append("---")
    parts.append("")
    parts.append(render_word_directive(pw.slug, pw.word, pw.target, pw.profile))
    parts.append("")
    parts.append(f"# {title}")
    parts.append("")
    parts.append(f"**Target:** `{pw.target}`  ")
    parts.append(f"**Profile:** `{pw.profile}`  ")
    parts.append(f"**Defined in:** `{pw.source_path}:{pw.line}`  ")
    parts.append(f"**Source:** <{source_url(pw.source_path, pw.line)}>")
    if wordsets:
        parts.append(f"**Wordset:** `{wordsets[0]}`  ")
    if len(wordsets) > 1:
        parts.append(
            f"**Also in:** {' '.join(f'`{wordset}`' for wordset in wordsets[1:])}  "
        )
    if std_url:
        parts.append(f"**Standard:** <{std_url}>")
    parts.append("")

    if stack:
        parts.append(f"`{md_escape_inline(stack)}`")
        parts.append("")

    if return_stack:
        parts.append(f"`{md_escape_inline(return_stack)}`")
        parts.append("")

    if desc:
        parts.append(desc)
        parts.append("")

    parts.append("## Entry")
    parts.append("")
    parts.append(f"- slug: `{pw.slug}`")
    parts.append(f"- word: `{pw.word}`")

    return "\n".join(parts)


def render_global_word_page(slug: str, occurrences: List[ProfileWord], doc: Optional[WordDoc]) -> str:
    display_word = doc.word if doc is not None else occurrences[0].word
    desc = doc.description if doc is not None else None
    stack = doc.stack if doc is not None else None
    return_stack = doc.return_stack if doc is not None else None
    wordsets = iter_wordsets(doc) if doc is not None else []
    f12_slug = doc.f12_slug if doc is not None else None
    std_url = standard_url(wordsets[0] if wordsets else None, f12_slug)

    parts: List[str] = []
    parts.append("---")
    parts.append("orphan: true")
    parts.append("---")
    parts.append("")
    parts.append(render_word_directive(slug, display_word))
    parts.append("")
    parts.append(f"# {display_word}")
    parts.append("")

    if wordsets:
        parts.append(f"**Wordset:** `{wordsets[0]}`  ")
    if len(wordsets) > 1:
        parts.append(
            f"**Also in:** {' '.join(f'`{wordset}`' for wordset in wordsets[1:])}  "
        )
    if std_url:
        parts.append(f"**Standard:** <{std_url}>")
    if wordsets or std_url:
        parts.append("")

    if stack:
        parts.append(f"`{md_escape_inline(stack)}`")
        parts.append("")

    if return_stack:
        parts.append(f"`{md_escape_inline(return_stack)}`")
        parts.append("")

    if desc:
        parts.append(desc)
        parts.append("")

    parts.append("## Available in Targets")
    parts.append("")
    seen: set[tuple[str, str]] = set()
    for pw in sorted(occurrences, key=lambda x: (x.target, x.profile)):
        key = (pw.target, pw.profile)
        if key in seen:
            continue
        seen.add(key)
        parts.append(
            f"- {{doc}}`{pw.target}/{pw.profile} </target/{pw.target}/profile/{pw.profile}/word/{pw.slug}>`"
        )

    return "\n".join(parts)


def render_profile_index(
    target: str,
    profile: str,
    words: List[ProfileWord],
    docs_by_slug: Dict[str, WordDoc],
) -> str:
    parts: List[str] = []
    parts.append(f"# Profile {profile}")
    parts.append("")
    parts.append(f"Target: `{target}`")
    parts.append("")
    parts.append(f"Available words: {len(words)}")
    parts.append("")
    parts.append("## Indexes")
    parts.append("")
    parts.append(f"- {{doc}}`Word Index </target/{target}/profile/{profile}/word-index>`")
    parts.append("")
    parts.append("```{toctree}")
    parts.append(":hidden:")
    parts.append(":maxdepth: 1")
    parts.append("")
    parts.append("word-index")
    parts.append("```")
    parts.append("")
    words_by_wordset: Dict[str, List[ProfileWord]] = {}
    for pw in words:
        for wordset in iter_wordsets(pw.doc):
            words_by_wordset.setdefault(wordset, []).append(pw)

    documented_by_wordset: Dict[str, List[WordDoc]] = {}
    for doc in docs_by_slug.values():
        for wordset in iter_wordsets(doc):
            documented_by_wordset.setdefault(wordset, []).append(doc)

    parts.append("## Implemented Words")
    parts.append("")

    for wordset in sorted(documented_by_wordset):
        parts.append(f"### {wordset}")
        parts.append("")

        implemented = words_by_wordset.get(wordset, [])
        implemented_slugs = {pw.slug for pw in implemented}

        parts.append("#### Implemented Words")
        parts.append("")
        if implemented:
            entries = [
                f"[{display_word(pw.doc, pw.word)}](word/{pw.slug}.md)"
                for pw in implemented
            ]
            parts.append(" ".join(entries))
        else:
            parts.append("None")
        parts.append("")

        parts.append("#### Missing Words")
        parts.append("")
        missing = [
            doc.word
            for doc in sorted(documented_by_wordset[wordset], key=lambda x: (x.word.lower(), x.word, x.slug))
            if doc.slug not in implemented_slugs
        ]
        if missing:
            parts.append(" ".join(f"`{word}`" for word in missing))
        else:
            parts.append("None")
        parts.append("")

    return "\n".join(parts)


def render_profile_word_index(
    target: str,
    profile: str,
    words: List[ProfileWord],
) -> str:
    parts: List[str] = []
    parts.append("# Word Index")
    parts.append("")
    parts.append(f"Target: `{target}`  ")
    parts.append(f"Profile: `{profile}`")
    parts.append("")
    parts.append("```{toctree}")
    parts.append(":maxdepth: 1")
    parts.append("")
    for pw in words:
        parts.append(f"word/{pw.slug}")
    parts.append("```")
    return "\n".join(parts)


def render_target_index(target: str, profiles: List[str]) -> str:
    parts: List[str] = []
    parts.append(f"# Target {target}")
    parts.append("")
    parts.append("## Profiles")
    parts.append("")
    for profile in profiles:
        parts.append(f"- {{doc}}`{profile} </target/{target}/profile/{profile}/index>`")
    parts.append("")

    parts.append("```{toctree}")
    parts.append(":maxdepth: 2")
    parts.append("")
    for profile in profiles:
        parts.append(f"profile/{profile}/index")
    parts.append("```")
    return "\n".join(parts)


def render_root_index(targets: List[str], global_slugs: List[str]) -> str:
    parts: List[str] = []
    parts.append("# ec4th Documentation")
    parts.append("")
    parts.append("## Targets")
    parts.append("")
    for target in targets:
        parts.append(f"- {{doc}}`{target} </target/{target}/index>`")
    parts.append("")

    parts.append("```{toctree}")
    parts.append(":maxdepth: 2")
    parts.append("")
    for target in targets:
        parts.append(f"target/{target}/index")
    parts.append("```")
    return "\n".join(parts)


def main() -> int:
    parser = argparse.ArgumentParser(
        description="Generate Sphinx/MyST docs from word YAML and ec4th tags."
    )
    parser.add_argument("--word-dir", type=Path, default=Path("doc/word"))
    parser.add_argument("--tags-dir", type=Path, default=Path("output"))
    parser.add_argument("--out-dir", type=Path, default=Path("output/doc"))
    args = parser.parse_args()

    docs_by_slug, docs_by_word = load_word_docs(args.word_dir)
    variants = discover_variants(args.tags_dir)

    if not variants:
        raise SystemExit(f"no tags found in {args.tags_dir} matching ec4th-*.tags")

    ensure_dir(args.out_dir)

    profile_words_map: Dict[tuple[str, str], List[ProfileWord]] = {}
    global_words: Dict[str, List[ProfileWord]] = {}
    wanted_files: set[Path] = set()

    for variant in variants:
        entries = parse_tags_file(variant.tags_file)
        pwords = build_profile_words(variant, entries, docs_by_slug, docs_by_word)
        profile_words_map[(variant.target, variant.profile)] = pwords

        for pw in pwords:
            global_words.setdefault(pw.slug, []).append(pw)

        profile_dir = args.out_dir / "target" / variant.target / "profile" / variant.profile
        profile_index = profile_dir / "index.md"
        wanted_files.add(profile_index)
        write_text(profile_index, render_profile_index(variant.target, variant.profile, pwords, docs_by_slug))
        word_index = profile_dir / "word-index.md"
        wanted_files.add(word_index)
        write_text(word_index, render_profile_word_index(variant.target, variant.profile, pwords))

        for pw in pwords:
            word_path = profile_dir / "word" / f"{pw.slug}.md"
            wanted_files.add(word_path)
            write_text(word_path, render_profile_word_page(pw))

    for target in sorted({variant.target for variant in variants}):
        target_dir = args.out_dir / "target" / target
        profiles = sorted(profile for current_target, profile in profile_words_map if current_target == target)
        target_index = target_dir / "index.md"
        wanted_files.add(target_index)
        write_text(target_index, render_target_index(target, profiles))

    for slug, occurrences in sorted(global_words.items()):
        doc = docs_by_slug.get(slug)
        word_path = args.out_dir / "word" / f"{slug}.md"
        wanted_files.add(word_path)
        write_text(word_path, render_global_word_page(slug, occurrences, doc))

    root_index = args.out_dir / "index.md"
    wanted_files.add(root_index)
    write_text(
        root_index,
        render_root_index(sorted({variant.target for variant in variants}), sorted(global_words.keys())),
    )
    prune_stale_files(args.out_dir, wanted_files)

    print(f"generated docs in {args.out_dir}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
