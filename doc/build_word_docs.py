#!/usr/bin/env python3
from __future__ import annotations

import argparse
import html
import re
import shutil
from dataclasses import dataclass, field
from pathlib import Path
from typing import Dict, List, Optional

try:
    import yaml
except ImportError as exc:
    raise SystemExit("PyYAML is required: pip install pyyaml") from exc


TAG_RE = re.compile(r'^(?P<word>\S+)\s+(?P<path>\S+)\s+(?P<line>\d+);"(?:\s.*)?$')
PROFILE_RE = re.compile(r"^ec4th-(?P<profile>.+)\.tags$")


@dataclass
class WordDoc:
    slug: str
    word: str
    f12_slug: Optional[str] = None
    wordset: Optional[str] = None
    description: Optional[str] = None
    stack: Optional[str] = None
    source_file: Optional[Path] = None


@dataclass
class TagEntry:
    word: str
    source_path: str
    line: int


@dataclass
class ProfileWord:
    profile: str
    slug: str
    word: str
    source_path: str
    line: int
    doc: Optional[WordDoc] = None


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
            description=raw.get("description"),
            stack=raw.get("stack"),
            source_file=path,
        )

        if slug in docs_by_slug:
            print(f"warning: duplicate slug {slug!r} in {path}")
        docs_by_slug[slug] = doc

        if word in docs_by_word:
            print(f"warning: duplicate word mapping {word!r} in {path}")
        docs_by_word[word] = doc

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


def discover_profiles(output_dir: Path) -> Dict[str, Path]:
    profiles: Dict[str, Path] = {}

    for path in sorted(output_dir.glob("ec4th-*.tags")):
        m = PROFILE_RE.match(path.name)
        if not m:
            continue
        profile = m.group("profile")
        profiles[profile] = path

    return profiles


def resolve_slug(word: str, docs_by_word: Dict[str, WordDoc]) -> str:
    doc = docs_by_word.get(word)
    if doc is not None:
        return doc.slug

    # fallback: safe-ish slug for undocumented words
    slug = word.strip().lower()
    slug = slug.replace(" ", "-")
    slug = re.sub(r"[^a-z0-9._-]+", "-", slug)
    slug = slug.strip("-")
    if not slug:
        slug = "unnamed"
    return slug


def build_profile_words(
    profile: str,
    entries: List[TagEntry],
    docs_by_slug: Dict[str, WordDoc],
    docs_by_word: Dict[str, WordDoc],
) -> List[ProfileWord]:
    seen_words: set[str] = set()
    resolved: List[ProfileWord] = []

    for entry in entries:
        if entry.word in seen_words:
            print(f"warning: duplicate word {entry.word!r} in profile {profile!r}; keeping first")
            continue
        seen_words.add(entry.word)

        slug = resolve_slug(entry.word, docs_by_word)
        doc = docs_by_slug.get(slug)

        resolved.append(
            ProfileWord(
                profile=profile,
                slug=slug,
                word=entry.word,
                source_path=entry.source_path,
                line=entry.line,
                doc=doc,
            )
        )

    resolved.sort(key=lambda x: (x.word.lower(), x.word, x.slug))
    return resolved


def ensure_clean_dir(path: Path) -> None:
    if path.exists():
        shutil.rmtree(path)
    path.mkdir(parents=True, exist_ok=True)


def write_text(path: Path, text: str) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(text.rstrip() + "\n", encoding="utf-8")


def md_escape_inline(text: str) -> str:
    return text.replace("\\", "\\\\").replace("`", "\\`")


def source_url(source_path: str, line: int) -> str:
    return f"/source/{source_path}.html#line-{line}"


def render_word_directive(slug: str, word: str, profile: Optional[str] = None) -> str:
    lines = [f"```{{forth:word}} {slug}", f":word: {word}"]
    if profile:
        lines.append(f":profile: {profile}")
    lines.append("```")
    return "\n".join(lines)


def render_profile_word_page(pw: ProfileWord) -> str:
    title = pw.doc.word if pw.doc is not None else pw.word
    desc = pw.doc.description if pw.doc is not None else None
    stack = pw.doc.stack if pw.doc is not None else None
    wordset = pw.doc.wordset if pw.doc is not None else None
    f12_slug = pw.doc.f12_slug if pw.doc is not None else None

    parts: List[str] = []
    parts.append(render_word_directive(pw.slug, pw.word, pw.profile))
    parts.append("")
    parts.append(f"# {title}")
    parts.append("")
    parts.append(f"**Profile:** `{pw.profile}`  ")
    parts.append(f"**Defined in:** `{pw.source_path}:{pw.line}`  ")
    parts.append(f"**Source:** <{source_url(pw.source_path, pw.line)}>")
    if wordset:
        parts.append(f"**Wordset:** `{wordset}`  ")
    if f12_slug:
        parts.append(f"**F12 slug:** `{f12_slug}`")
    parts.append("")

    if desc:
        parts.append("## Description")
        parts.append("")
        parts.append(desc)
        parts.append("")

    if stack:
        parts.append("## Stack")
        parts.append("")
        parts.append(f"`{md_escape_inline(stack)}`")
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
    wordset = doc.wordset if doc is not None else None
    f12_slug = doc.f12_slug if doc is not None else None

    parts: List[str] = []
    parts.append(render_word_directive(slug, display_word))
    parts.append("")
    parts.append(f"# {display_word}")
    parts.append("")

    if wordset:
        parts.append(f"**Wordset:** `{wordset}`  ")
    if f12_slug:
        parts.append(f"**F12 slug:** `{f12_slug}`")
    if wordset or f12_slug:
        parts.append("")

    if desc:
        parts.append("## Description")
        parts.append("")
        parts.append(desc)
        parts.append("")

    if stack:
        parts.append("## Stack")
        parts.append("")
        parts.append(f"`{md_escape_inline(stack)}`")
        parts.append("")

    parts.append("## Available in profiles")
    parts.append("")
    for pw in sorted(occurrences, key=lambda x: x.profile):
        parts.append(f"- {{doc}}`{pw.profile} </profile/{pw.profile}/word/{pw.slug}>`")

    return "\n".join(parts)


def render_profile_index(profile: str, words: List[ProfileWord]) -> str:
    parts: List[str] = []
    parts.append(f"# Profile {profile}")
    parts.append("")
    parts.append(f"Available words: {len(words)}")
    parts.append("")
    parts.append("```{toctree}")
    parts.append(":maxdepth: 1")
    parts.append("")
    for pw in words:
        parts.append(f"word/{pw.slug}")
    parts.append("```")
    parts.append("")
    parts.append("## Words")
    parts.append("")
    for pw in words:
        parts.append(f"- {{doc}}`{pw.word} </profile/{profile}/word/{pw.slug}>`")
    return "\n".join(parts)


def render_root_index(profiles: List[str], global_slugs: List[str]) -> str:
    parts: List[str] = []
    parts.append("# ec4th Documentation")
    parts.append("")
    parts.append("## Profiles")
    parts.append("")
    for profile in profiles:
        parts.append(f"- {{doc}}`{profile} </profile/{profile}/index>`")
    parts.append("")

    parts.append("```{toctree}")
    parts.append(":maxdepth: 2")
    parts.append("")
    for profile in profiles:
        parts.append(f"profile/{profile}/index")
    for slug in global_slugs:
        parts.append(f"word/{slug}")
    parts.append("```")
    return "\n".join(parts)


def main() -> int:
    parser = argparse.ArgumentParser(description="Generate Sphinx/MyST docs from word YAML and ec4th tags.")
    parser.add_argument("--word-dir", type=Path, default=Path("doc/word"))
    parser.add_argument("--tags-dir", type=Path, default=Path("output"))
    parser.add_argument("--out-dir", type=Path, default=Path("output/doc"))
    args = parser.parse_args()

    docs_by_slug, docs_by_word = load_word_docs(args.word_dir)
    profiles = discover_profiles(args.tags_dir)

    if not profiles:
        raise SystemExit(f"no profile tags found in {args.tags_dir} matching ec4th-*.tags")

    ensure_clean_dir(args.out_dir)

    profile_words_map: Dict[str, List[ProfileWord]] = {}
    global_words: Dict[str, List[ProfileWord]] = {}

    for profile, tags_file in sorted(profiles.items()):
        entries = parse_tags_file(tags_file)
        pwords = build_profile_words(profile, entries, docs_by_slug, docs_by_word)
        profile_words_map[profile] = pwords

        for pw in pwords:
            global_words.setdefault(pw.slug, []).append(pw)

        profile_dir = args.out_dir / "profile" / profile
        write_text(profile_dir / "index.md", render_profile_index(profile, pwords))

        for pw in pwords:
            write_text(profile_dir / "word" / f"{pw.slug}.md", render_profile_word_page(pw))

    for slug, occurrences in sorted(global_words.items()):
        doc = docs_by_slug.get(slug)
        write_text(args.out_dir / "word" / f"{slug}.md", render_global_word_page(slug, occurrences, doc))

    write_text(
        args.out_dir / "index.md",
        render_root_index(sorted(profile_words_map.keys()), sorted(global_words.keys())),
    )

    print(f"generated docs in {args.out_dir}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
