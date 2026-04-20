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
    f12_url: Optional[str] = None
    wordset: Optional[str] = None
    also_wordsets: Optional[List[str]] = None
    description: Optional[str] = None
    stack: Optional[str] = None
    return_stack: Optional[str] = None
    see: Optional[List[str]] = None
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
            f12_url=raw.get("f12-url"),
            wordset=raw.get("wordset"),
            also_wordsets=list(raw.get("also-wordsets") or []),
            description=raw.get("description"),
            stack=raw.get("stack"),
            return_stack=raw.get("return-stack"),
            see=list(raw.get("see") or []),
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


def md_code(text: str) -> str:
    return f"`{text.replace('`', '\\`')}`"


def md_display_text(text: str) -> str:
    if "\\" in text or "[" in text or "]" in text:
        return md_code(text)
    return text


def md_link_text(text: str) -> str:
    return (
        text.replace("&", "&amp;")
        .replace("\\", "&#92;")
        .replace("[", "&#91;")
        .replace("]", "&#93;")
    )


def md_word_link(label: str, target: str) -> str:
    return f"[{md_link_text(label)}]({target})"


def wordset_doc_link(wordset: str) -> str:
    return f"{{doc}}`{wordset} </wordset/{wordset}>`"


def profile_wordset_doc_link(target: str, profile: str, wordset: str) -> str:
    return f"{{doc}}`{wordset} </target/{target}/profile/{profile}/wordset/{wordset}>`"


def profile_word_doc_target(target: str, profile: str, slug: str) -> str:
    return f"/target/{target}/profile/{profile}/word/{slug}"


PROFILE_DOC_REF_RE = re.compile(r"\{doc\}`(?P<label>.*?) </word/(?P<slug>[^>]+)>`")
PROFILE_MD_REF_RE = re.compile(r"\[(?P<label>.+?)\]\(/word/(?P<slug>[^)]+)\.md\)")


def localize_profile_ref(
    ref: str,
    target: str,
    profile: str,
    available_slugs: set[str],
) -> str:
    def replace_doc(match: re.Match[str]) -> str:
        slug = match.group("slug")
        if slug not in available_slugs:
            return match.group(0)
        label = match.group("label")
        return f"{{doc}}`{label} <{profile_word_doc_target(target, profile, slug)}>`"

    ref = PROFILE_DOC_REF_RE.sub(replace_doc, ref)

    def replace_md(match: re.Match[str]) -> str:
        slug = match.group("slug")
        if slug not in available_slugs:
            return match.group(0)
        label = match.group("label")
        return f"[{label}]({profile_word_doc_target(target, profile, slug)}.md)"

    return PROFILE_MD_REF_RE.sub(replace_md, ref)


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


def standard_url(doc: Optional[WordDoc]) -> Optional[str]:
    if doc is None:
        return None
    if doc.f12_url:
        return doc.f12_url
    if not doc.wordset or not doc.f12_slug:
        return None
    return (
        f"https://forth-standard.org/standard/"
        f"{standard_wordset_path(doc.wordset)}/{doc.f12_slug}"
    )


SEE_REF_RE = re.compile(r"\{forth:word\}`([^`]+)`")


def extract_ref_name(ref: str) -> Optional[str]:
    m = SEE_REF_RE.search(ref)
    if m:
        return m.group(1).strip()
    cleaned = ref.strip().strip("`").strip()
    return cleaned or None


def render_see_ref(
    ref: str,
    docs_by_word: Dict[str, WordDoc],
    docs_by_slug: Dict[str, WordDoc],
) -> str:
    name = extract_ref_name(ref)
    if not name:
        return ref
    target_doc = docs_by_word.get(normalize_word_key(name))
    if target_doc is not None:
        if "\\" in target_doc.word:
            return md_word_link(target_doc.word, f"/word/{target_doc.slug}.md")
        return f"{{doc}}`{target_doc.word} </word/{target_doc.slug}>`"

    target_slug = resolve_slug(name, docs_by_word)
    if target_slug in docs_by_slug or SAFE_WORD_RE.match(target_slug) or target_slug.startswith("op-"):
        if "\\" in name:
            return md_word_link(name, f"/word/{target_slug}.md")
        return f"{{doc}}`{name} </word/{target_slug}>`"
    return ref


def compute_see_groups(
    docs_by_slug: Dict[str, WordDoc],
    docs_by_word: Dict[str, WordDoc],
) -> Dict[str, List[str]]:
    parent = {slug: slug for slug in docs_by_slug}

    def find(x: str) -> str:
        while parent[x] != x:
            parent[x] = parent[parent[x]]
            x = parent[x]
        return x

    def union(a: str, b: str) -> None:
        ra, rb = find(a), find(b)
        if ra != rb:
            parent[ra] = rb

    parsed: Dict[str, List[tuple[Optional[str], str]]] = {}
    for slug, doc in docs_by_slug.items():
        entries: List[tuple[Optional[str], str]] = []
        for ref in doc.see or []:
            name = extract_ref_name(ref)
            target_slug: Optional[str] = None
            rendered_ref = render_see_ref(ref, docs_by_word, docs_by_slug)
            if name:
                target_doc = docs_by_word.get(normalize_word_key(name))
                if target_doc is not None:
                    target_slug = target_doc.slug
                    union(slug, target_slug)
            entries.append((target_slug, rendered_ref))
        parsed[slug] = entries

    clusters: Dict[str, List[str]] = {}
    for slug in docs_by_slug:
        clusters.setdefault(find(slug), []).append(slug)

    result: Dict[str, List[str]] = {}
    for members in clusters.values():
        members_set = set(members)

        external_refs: List[str] = []
        seen_external: set[str] = set()
        for member in members:
            for target_slug, ref_str in parsed[member]:
                if target_slug is None or target_slug not in members_set:
                    if ref_str not in seen_external:
                        seen_external.add(ref_str)
                        external_refs.append(ref_str)

        if len(members) == 1 and not external_refs:
            result[members[0]] = []
            continue

        for member in members:
            member_refs: List[str] = []
            others_sorted = sorted(
                (s for s in members if s != member),
                key=lambda s: docs_by_slug[s].word.lower(),
            )
            for other_slug in others_sorted:
                other_word = docs_by_slug[other_slug].word
                if "\\" in other_word:
                    member_refs.append(md_word_link(other_word, f"/word/{other_slug}.md"))
                else:
                    member_refs.append(f"{{doc}}`{other_word} </word/{other_slug}>`")
            member_refs.extend(external_refs)
            result[member] = member_refs

    return result


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


def render_profile_word_page(
    pw: ProfileWord,
    see_also: Optional[List[str]] = None,
    profile_slugs: Optional[set[str]] = None,
) -> str:
    title = pw.doc.word if pw.doc is not None else pw.word
    desc = pw.doc.description if pw.doc is not None else None
    stack = pw.doc.stack if pw.doc is not None else None
    return_stack = pw.doc.return_stack if pw.doc is not None else None
    wordsets = iter_wordsets(pw.doc) if pw.doc is not None else []
    std_url = standard_url(pw.doc)
    profile_slugs = profile_slugs or set()

    parts: List[str] = []
    parts.append("---")
    parts.append("orphan: true")
    parts.append("---")
    parts.append("")
    parts.append(render_word_directive(pw.slug, pw.word, pw.target, pw.profile))
    parts.append("")
    parts.append(f"# {md_display_text(title)}")
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

    if see_also:
        parts.append("## See also")
        parts.append("")
        localized = [
            localize_profile_ref(ref, pw.target, pw.profile, profile_slugs)
            for ref in see_also
        ]
        parts.append(" ".join(localized))
        parts.append("")

    parts.append(f"**Target:** `{pw.target}`  ")
    parts.append(f"**Profile:** `{pw.profile}`  ")
    parts.append(f"**Defined in:** `{pw.source_path}:{pw.line}`  ")
    parts.append(f"**Source:** <{source_url(pw.source_path, pw.line)}>  ")
    if wordsets:
        parts.append(
            f"**Wordset:** {profile_wordset_doc_link(pw.target, pw.profile, wordsets[0])}  "
        )
    if len(wordsets) > 1:
        parts.append(
            f"**Also in:** {' '.join(profile_wordset_doc_link(pw.target, pw.profile, wordset) for wordset in wordsets[1:])}  "
        )
    if std_url:
        parts.append(f"**Standard:** <{std_url}>")
    parts.append("")

    parts.append("## Entry")
    parts.append("")
    parts.append(f"- slug: `{pw.slug}`")
    parts.append(f"- word: `{pw.word}`")

    return "\n".join(parts)


def render_global_word_page(
    slug: str,
    occurrences: List[ProfileWord],
    doc: Optional[WordDoc],
    see_also: Optional[List[str]] = None,
) -> str:
    if doc is not None:
        display_word = doc.word
    elif occurrences:
        display_word = occurrences[0].word
    else:
        display_word = slug
    desc = doc.description if doc is not None else None
    stack = doc.stack if doc is not None else None
    return_stack = doc.return_stack if doc is not None else None
    wordsets = iter_wordsets(doc) if doc is not None else []
    std_url = standard_url(doc)

    parts: List[str] = []
    parts.append("---")
    parts.append("orphan: true")
    parts.append("---")
    parts.append("")
    parts.append(render_word_directive(slug, display_word))
    parts.append("")
    parts.append(f"# {md_display_text(display_word)}")
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

    if see_also:
        parts.append("## See also")
        parts.append("")
        parts.append(" ".join(see_also))
        parts.append("")

    if wordsets:
        parts.append(f"**Wordset:** {wordset_doc_link(wordsets[0])}  ")
    if len(wordsets) > 1:
        parts.append(
            f"**Also in:** {' '.join(wordset_doc_link(wordset) for wordset in wordsets[1:])}  "
        )
    if std_url:
        parts.append(f"**Standard:** <{std_url}>")
    if wordsets or std_url:
        parts.append("")

    parts.append("## Available in Targets")
    parts.append("")
    if not occurrences:
        parts.append("Not implemented in any target.")
    else:
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
    words_by_wordset: Dict[str, List[ProfileWord]] = {}
    for pw in words:
        for wordset in iter_wordsets(pw.doc):
            words_by_wordset.setdefault(wordset, []).append(pw)

    documented_by_wordset: Dict[str, List[WordDoc]] = {}
    for doc in docs_by_slug.values():
        for wordset in iter_wordsets(doc):
            documented_by_wordset.setdefault(wordset, []).append(doc)

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
    for wordset in sorted(documented_by_wordset):
        parts.append(f"wordset/{wordset}")
    parts.append("```")
    parts.append("")
    parts.append("## Implemented Words")
    parts.append("")

    for wordset in sorted(documented_by_wordset):
        parts.append(f"### {profile_wordset_doc_link(target, profile, wordset)}")
        parts.append("")

        implemented = words_by_wordset.get(wordset, [])
        implemented_slugs = {pw.slug for pw in implemented}

        parts.append("#### Implemented Words")
        parts.append("")
        if implemented:
            entries = [
                md_word_link(display_word(pw.doc, pw.word), f"word/{pw.slug}.md")
                for pw in implemented
            ]
            parts.append(" ".join(entries))
        else:
            parts.append("None")
        parts.append("")

        parts.append("#### Missing Words")
        parts.append("")
        missing = [
            doc
            for doc in sorted(documented_by_wordset[wordset], key=lambda x: (x.word.lower(), x.word, x.slug))
            if doc.slug not in implemented_slugs
        ]
        if missing:
            parts.append(
                " ".join(md_word_link(doc.word, f"/word/{doc.slug}.md") for doc in missing)
            )
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


def render_wordset_page(
    wordset: str,
    docs: List[WordDoc],
    occurrences: List[ProfileWord],
) -> str:
    parts: List[str] = []
    parts.append(f"# Wordset {wordset}")
    parts.append("")
    parts.append(f"Documented words: {len(docs)}")
    parts.append("")

    if occurrences:
        profiles: dict[tuple[str, str], int] = {}
        for pw in occurrences:
            profiles[(pw.target, pw.profile)] = profiles.get((pw.target, pw.profile), 0) + 1
        parts.append("## Implemented in Targets")
        parts.append("")
        for (target, profile), count in sorted(profiles.items()):
            parts.append(
                f"- {{doc}}`{target}/{profile} </target/{target}/profile/{profile}/index>`: {count} words"
            )
        parts.append("")

    parts.append("## Words")
    parts.append("")
    if docs:
        parts.append(" ".join(md_word_link(doc.word, f"/word/{doc.slug}.md") for doc in docs))
    else:
        parts.append("None")
    parts.append("")
    return "\n".join(parts)


def render_profile_wordset_page(
    target: str,
    profile: str,
    wordset: str,
    implemented: List[ProfileWord],
    documented: List[WordDoc],
) -> str:
    implemented = sorted(implemented, key=lambda x: (display_word(x.doc, x.word).lower(), display_word(x.doc, x.word), x.slug))
    implemented_slugs = {pw.slug for pw in implemented}
    missing = [
        doc for doc in documented if doc.slug not in implemented_slugs
    ]

    parts: List[str] = []
    parts.append(f"# Wordset {wordset}")
    parts.append("")
    parts.append(f"Target: `{target}`  ")
    parts.append(f"Profile: `{profile}`")
    parts.append("")
    parts.append(f"Documented words: {len(documented)}  ")
    parts.append(f"Implemented words: {len(implemented)}  ")
    parts.append(f"Missing words: {len(missing)}")
    parts.append("")
    parts.append("## Implemented Words")
    parts.append("")
    if implemented:
        parts.append(
            " ".join(
                md_word_link(display_word(pw.doc, pw.word), f"../word/{pw.slug}.md")
                for pw in implemented
            )
        )
    else:
        parts.append("None")
    parts.append("")
    parts.append("## Missing Words")
    parts.append("")
    if missing:
        parts.append(
            " ".join(md_word_link(doc.word, f"/word/{doc.slug}.md") for doc in missing)
        )
    else:
        parts.append("None")
    parts.append("")
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


def render_root_index(targets: List[str], wordsets: List[str]) -> str:
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
    parts.append("")
    parts.append("## Wordsets")
    parts.append("")
    for wordset in wordsets:
        parts.append(f"- {wordset_doc_link(wordset)}")
    parts.append("")
    parts.append("```{toctree}")
    parts.append(":hidden:")
    parts.append(":maxdepth: 1")
    parts.append("")
    for wordset in wordsets:
        parts.append(f"wordset/{wordset}")
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
    see_groups = compute_see_groups(docs_by_slug, docs_by_word)
    variants = discover_variants(args.tags_dir)

    if not variants:
        raise SystemExit(f"no tags found in {args.tags_dir} matching ec4th-*.tags")

    ensure_dir(args.out_dir)

    profile_words_map: Dict[tuple[str, str], List[ProfileWord]] = {}
    global_words: Dict[str, List[ProfileWord]] = {}
    documented_by_wordset: Dict[str, List[WordDoc]] = {}
    wanted_files: set[Path] = set()

    for doc in docs_by_slug.values():
        for wordset in iter_wordsets(doc):
            documented_by_wordset.setdefault(wordset, []).append(doc)

    for variant in variants:
        entries = parse_tags_file(variant.tags_file)
        pwords = build_profile_words(variant, entries, docs_by_slug, docs_by_word)
        profile_words_map[(variant.target, variant.profile)] = pwords
        profile_slugs = {pw.slug for pw in pwords}

        for pw in pwords:
            global_words.setdefault(pw.slug, []).append(pw)

        profile_dir = args.out_dir / "target" / variant.target / "profile" / variant.profile
        profile_index = profile_dir / "index.md"
        wanted_files.add(profile_index)
        write_text(profile_index, render_profile_index(variant.target, variant.profile, pwords, docs_by_slug))
        word_index = profile_dir / "word-index.md"
        wanted_files.add(word_index)
        write_text(word_index, render_profile_word_index(variant.target, variant.profile, pwords))

        words_by_wordset: Dict[str, List[ProfileWord]] = {}
        for pw in pwords:
            for wordset in iter_wordsets(pw.doc):
                words_by_wordset.setdefault(wordset, []).append(pw)

        for wordset, documented in documented_by_wordset.items():
            wordset_path = profile_dir / "wordset" / f"{wordset}.md"
            wanted_files.add(wordset_path)
            write_text(
                wordset_path,
                render_profile_wordset_page(
                    variant.target,
                    variant.profile,
                    wordset,
                    words_by_wordset.get(wordset, []),
                    sorted(documented, key=lambda x: (x.word.lower(), x.word, x.slug)),
                ),
            )

        for pw in pwords:
            word_path = profile_dir / "word" / f"{pw.slug}.md"
            wanted_files.add(word_path)
            write_text(
                word_path,
                render_profile_word_page(
                    pw,
                    see_groups.get(pw.slug),
                    profile_slugs,
                ),
            )

    for target in sorted({variant.target for variant in variants}):
        target_dir = args.out_dir / "target" / target
        profiles = sorted(profile for current_target, profile in profile_words_map if current_target == target)
        target_index = target_dir / "index.md"
        wanted_files.add(target_index)
        write_text(target_index, render_target_index(target, profiles))

    all_slugs: set[str] = set(global_words.keys()) | set(docs_by_slug.keys())
    for slug in sorted(all_slugs):
        doc = docs_by_slug.get(slug)
        occurrences = global_words.get(slug, [])
        word_path = args.out_dir / "word" / f"{slug}.md"
        wanted_files.add(word_path)
        write_text(word_path, render_global_word_page(slug, occurrences, doc, see_groups.get(slug)))

    for wordset in sorted(documented_by_wordset):
        docs = sorted(
            documented_by_wordset[wordset],
            key=lambda x: (x.word.lower(), x.word, x.slug),
        )
        occurrences = [
            pw
            for pw_list in profile_words_map.values()
            for pw in pw_list
            if wordset in iter_wordsets(pw.doc)
        ]
        wordset_path = args.out_dir / "wordset" / f"{wordset}.md"
        wanted_files.add(wordset_path)
        write_text(wordset_path, render_wordset_page(wordset, docs, occurrences))

    root_index = args.out_dir / "index.md"
    wanted_files.add(root_index)
    write_text(
        root_index,
        render_root_index(
            sorted({variant.target for variant in variants}),
            sorted(documented_by_wordset),
        ),
    )
    prune_stale_files(args.out_dir, wanted_files)

    print(f"generated docs in {args.out_dir}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
