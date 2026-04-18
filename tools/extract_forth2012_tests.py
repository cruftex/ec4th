#!/usr/bin/env python3
from __future__ import annotations

import html
import re
from html.parser import HTMLParser
from pathlib import Path

import yaml


ROOT = Path(__file__).resolve().parents[1]
WORD_DIR = ROOT / "doc" / "word"
TEST_DIR = ROOT / "+/forth2012/test"
SUITE_DIR = ROOT / "+/forth2012/suite"
STD_ROOT = ROOT / "tmp/forth-standard.org/standard"

WORDSET_DIR = {
    "core": "core",
    "core-ext": "core",
    "double": "double",
    "double-ext": "double",
    "exception": "exception",
    "exception-ext": "exception",
    "facility": "facility",
    "facility-ext": "facility",
    "floating": "float",
    "floating-ext": "float",
    "memory": "memory",
    "memory-ext": "memory",
    "tools": "tools",
    "tools-ext": "tools",
    "string": "string",
    "string-ext": "string",
}

F12_OVERRIDES = {
    "k-f1.yml": "K-FOne",
    "k-f2.yml": "K-FTwo",
    "op-backslash.yml": "bs",
    "op-c-quote.yml": "Cq",
    "op-cmove-greater.yml": "CMOVEtop",
    "op-d-dot.yml": "Dd",
    "op-d-two-slash.yml": "DTwoDiv",
    "op-d-two-star.yml": "DTwoTimes",
    "op-dot-paren.yml": "Dotp",
    "op-dot-quote.yml": "Dotq",
    "op-dot.yml": "d",
    "op-greater.yml": "more",
    "op-less-number-sign.yml": "num-start",
    "op-minus-trailing.yml": "MinusTRAILING",
    "op-m-star.yml": "MTimes",
    "op-not-equal.yml": "ne",
    "op-paren.yml": "p",
    "op-question-do.yml": "qDO",
    "op-s-backslash-quote.yml": "Seq",
    "op-semicolon.yml": "Semi",
    "op-slash.yml": "Div",
    "op-slash-string.yml": "DivSTRING",
    "op-star.yml": "Times",
    "op-two-slash.yml": "TwoDiv",
    "op-two-star.yml": "TwoTimes",
    "op-u-dot.yml": "Ud",
    "op-u-greater.yml": "Umore",
    "op-um-star.yml": "UMTimes",
    "op-zero-greater.yml": "Zeromore",
    "op-zero-not-equal.yml": "Zerone",
}

INLINE_GROUPS = {
    "if.fs": ["if.fs"],
    "begin.fs": ["while.fs", "until.fs"],
    "do.fs": ["loop.fs", "op-plus-loop.fs", "j.fs", "leave.fs", "unloop.fs"],
    "create.fs": ["op-to-body.fs", "op-does.fs"],
    "base.fs": ["base.fs"],
    "case.fs": ["case.fs"],
    "emit.fs": ["emit.fs"],
    "op-comma.fs": ["op-comma.fs"],
    "op-c-comma.fs": ["op-c-comma.fs"],
    "op-number-sign.fs": ["op-number-sign.fs"],
    "op-to-r.fs": ["op-to-r.fs"],
    "value.fs": ["value.fs"],
    "throw.fs": ["throw.fs"],
    "op-colon.fs": ["op-colon.fs"],
}

BACKLINKS = {
    "else.fs": ["if.fs"],
    "then.fs": ["if.fs"],
    "while.fs": ["begin.fs"],
    "until.fs": ["begin.fs"],
    "repeat.fs": ["begin.fs"],
    "i.fs": ["do.fs"],
    "loop.fs": ["do.fs"],
    "op-plus-loop.fs": ["do.fs"],
    "j.fs": ["do.fs"],
    "leave.fs": ["do.fs"],
    "unloop.fs": ["do.fs"],
    "exit.fs": ["do.fs"],
    "op-does.fs": ["create.fs"],
    "op-to-body.fs": ["create.fs"],
    "decimal.fs": ["base.fs"],
    "hex.fs": ["base.fs"],
    "of.fs": ["case.fs"],
    "endof.fs": ["case.fs"],
    "endcase.fs": ["case.fs"],
    "cr.fs": ["emit.fs"],
    "op-dot.fs": ["emit.fs"],
    "op-dot-quote.fs": ["emit.fs"],
    "space.fs": ["emit.fs"],
    "spaces.fs": ["emit.fs"],
    "type.fs": ["emit.fs"],
    "op-u-dot.fs": ["emit.fs"],
    "here.fs": ["op-comma.fs"],
    "op-fetch.fs": ["op-comma.fs"],
    "op-store.fs": ["op-comma.fs"],
    "op-cell-plus.fs": ["op-comma.fs"],
    "op-two-fetch.fs": ["op-comma.fs"],
    "op-two-store.fs": ["op-comma.fs"],
    "op-c-fetch.fs": ["op-c-comma.fs"],
    "op-c-store.fs": ["op-c-comma.fs"],
    "op-char-plus.fs": ["op-c-comma.fs"],
    "op-less-number-sign.fs": ["op-number-sign.fs"],
    "op-number-sign-greater.fs": ["op-number-sign.fs"],
    "op-r-fetch.fs": ["op-to-r.fs"],
    "op-r-from.fs": ["op-to-r.fs"],
    "to.fs": ["value.fs"],
    "catch.fs": ["throw.fs"],
    "op-semicolon.fs": ["op-colon.fs"],
    "execute.fs": ["op-tick.fs", "op-bracket-tick.fs"],
    "immediate.fs": ["op-bracket-tick.fs"],
}


def normalize_name(text: str) -> str:
    return re.sub(r"[^a-z0-9]", "", text.lower())


def build_page_index() -> dict[str, dict[str, Path]]:
    index: dict[str, dict[str, Path]] = {}
    for std_dir in set(WORDSET_DIR.values()):
        dir_path = STD_ROOT / std_dir
        bucket: dict[str, Path] = {}
        for path in dir_path.iterdir():
            if path.is_file():
                bucket[normalize_name(path.name)] = path
        index[std_dir] = bucket
    return index


class TestingFragmentParser(HTMLParser):
    def __init__(self) -> None:
        super().__init__()
        self.parts: list[str] = []

    def handle_starttag(self, tag: str, attrs: list[tuple[str, str | None]]) -> None:
        if tag == "br":
            self.parts.append("\n")
        elif tag == "p":
            self.parts.append("\n")

    def handle_endtag(self, tag: str) -> None:
        if tag == "p":
            self.parts.append("\n")

    def handle_data(self, data: str) -> None:
        self.parts.append(data)

    def handle_entityref(self, name: str) -> None:
        self.parts.append(html.unescape(f"&{name};"))

    def handle_charref(self, name: str) -> None:
        self.parts.append(html.unescape(f"&#{name};"))

    def handle_comment(self, data: str) -> None:
        comment = data.strip()
        if comment:
            self.parts.append(f"\n\\ {comment}\n")


def extract_testing_fragment(page_text: str) -> str | None:
    start_marker = "<h2>Testing:</h2>"
    start = page_text.find(start_marker)
    if start == -1:
        return None

    start = page_text.find("<div>", start)
    end = page_text.find('<div class="contributions-wrapper">', start)
    if start == -1 or end == -1:
        return None

    depth = 0
    i = start
    while i < end:
        if page_text.startswith("<div", i):
            close = page_text.find(">", i)
            if close == -1:
                break
            depth += 1
            i = close + 1
            continue
        if page_text.startswith("</div>", i):
            depth -= 1
            i += len("</div>")
            if depth == 0:
                return page_text[start:i]
            continue
        i += 1

    return page_text[start:end]


def normalize_test_lines(fragment: str) -> list[str]:
    parser = TestingFragmentParser()
    parser.feed(fragment)
    text = "".join(parser.parts)
    text = text.replace("\xa0", " ")

    lines: list[str] = []
    for raw in text.splitlines():
        line = re.sub(r"[ \t]+", " ", raw).strip()
        if not line:
            continue
        if line.startswith("\\ "):
            lines.append(line)
            continue
        if "}T" in line:
            lines.append(line)
            continue
        if line.startswith("(") and line.endswith(")"):
            lines.append(line)
            continue
        lines.append(f"\\ {line}")

    return lines


def render_test_file(slug: str, word: str, wordset: str, source_page: str, lines: list[str]) -> str:
    out = [
        "\\ Generated from forth-standard.org",
        f"\\ Word: {word}",
        f"\\ Slug: {slug}",
        f"\\ Wordset: {wordset}",
        f"\\ Source: {source_page}",
        "",
    ]
    if lines:
        out.extend(lines)
    else:
        out.append("\\ No testing section found.")
    return "\n".join(out).rstrip() + "\n"


def write_text(path: Path, text: str) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    if path.exists() and path.read_text(encoding="utf-8") == text:
        return
    path.write_text(text, encoding="utf-8")


def split_rendered(text: str) -> tuple[list[str], list[str]]:
    lines = text.splitlines()
    for index, line in enumerate(lines):
        if line == "":
            return lines[:index], lines[index + 1 :]
    return lines, []


def source_comment(body: list[str]) -> str:
    for line in body:
        if line.startswith("\\ T.") or line.startswith("\\ F."):
            return line[2:]
    return "tests"


def render_inline_header(source: str, body: list[str]) -> str:
    return f"\\ {source_comment(body)} (from {source})"


def strip_leading_source_header(body: list[str]) -> list[str]:
    if body and (body[0].startswith("\\ T.") or body[0].startswith("\\ F.")):
        return body[1:]
    return body


def render_backlink(header: list[str], owners: list[str]) -> str:
    lines = list(header)
    lines.append("")
    if len(owners) == 1:
        lines.append(f"\\ Tested in {owners[0]}.")
    else:
        joined = ", ".join(owners)
        lines.append(f"\\ Tested in {joined}.")
    return "\n".join(lines).rstrip() + "\n"


def parse_testsuite_blocks() -> dict[str, list[tuple[str, str, str, list[str]]]]:
    testsuite_path = STD_ROOT / "testsuite"
    page_text = testsuite_path.read_text(encoding="utf-8")
    matches = list(re.finditer(r'<div class="wordHead" id="test:([^:]+):([^"]+)">', page_text))
    blocks: dict[str, list[tuple[str, str, str, list[str]]]] = {}

    for index, match in enumerate(matches):
        wordset = match.group(1)
        slug = match.group(2)
        start = match.start()
        end = matches[index + 1].start() if index + 1 < len(matches) else len(page_text)
        segment = page_text[start:end]

        number_match = re.search(r'<div class="wordNumber">([^<]+)</div>', segment)
        name_match = re.search(r'<div class="wordName">([^<]+)</div>', segment)
        testing_start = segment.find('<div class="testing">')
        if number_match is None or name_match is None or testing_start == -1:
            continue

        fragment = segment[testing_start + len('<div class="testing">') :]
        lines = normalize_test_lines(fragment)
        blocks.setdefault(wordset, []).append(
            (number_match.group(1).strip(), name_match.group(1).strip(), slug, lines)
        )

    return blocks


def canonical_header_number(number: str) -> str:
    if len(number) > 2 and number[1] == "." and number[0] in {"F", "T"}:
        return number[2:]
    return number


def parse_header_key(line: str) -> tuple[str, str] | None:
    match = re.match(r"^\\\s+([FT]\.[0-9.]+)\s+(.+?)(?:\s+\(from .+\))?$", line)
    if match is None:
        return None
    return (canonical_header_number(match.group(1)), match.group(2))


def collect_covered_testsuite_headers() -> set[tuple[str, str]]:
    covered: set[tuple[str, str]] = set()
    for path in TEST_DIR.glob("*.fs"):
        for line in path.read_text(encoding="utf-8").splitlines():
            key = parse_header_key(line)
            if key is not None:
                covered.add(key)
    return covered


def strip_leading_matching_header(lines: list[str], number: str, name: str) -> list[str]:
    if lines and parse_header_key(lines[0]) == (canonical_header_number(number), name):
        return lines[1:]
    return lines


def has_meaningful_testsuite_content(lines: list[str]) -> bool:
    if not lines:
        return False
    for line in lines:
        if "}T" in line:
            return True
        if line.startswith("\\ See "):
            continue
        if line.startswith("\\ F.") or line.startswith("\\ T."):
            continue
        if line.startswith("\\ ") and len(line) > 2:
            return True
        if not line.startswith("\\ "):
            return True
    return False


def strip_testsuite_noise(lines: list[str]) -> list[str]:
    cleaned = list(lines)
    while cleaned and re.match(r"^\\ F\.[0-9.]+ The optional .+$", cleaned[-1]):
        cleaned.pop()
    return cleaned


def render_suite_file(wordset: str, blocks: list[tuple[str, str, str, list[str]]]) -> str:
    out = [
        "\\ Generated from forth-standard.org testsuite",
        f"\\ Wordset: {wordset}",
        "",
    ]
    first = True
    for number, name, slug, lines in blocks:
        body = strip_testsuite_noise(strip_leading_matching_header(lines, number, name))
        if not first:
            out.append("")
        first = False
        out.append(f"\\ {number} {name} (from testsuite#test:{wordset}:{slug})")
        out.extend(body)
    return "\n".join(out).rstrip() + "\n"


def generate_suite_files() -> None:
    covered = collect_covered_testsuite_headers()
    blocks_by_wordset = parse_testsuite_blocks()

    for wordset, blocks in blocks_by_wordset.items():
        remaining: list[tuple[str, str, str, list[str]]] = []
        for number, name, slug, lines in blocks:
            if (canonical_header_number(number), name) in covered:
                continue
            if not has_meaningful_testsuite_content(lines):
                continue
            remaining.append((number, name, slug, lines))

        if not remaining:
            continue

        write_text(SUITE_DIR / f"{wordset}.fs", render_suite_file(wordset, remaining))


def postprocess_generated_files() -> None:
    rendered_by_name: dict[str, str] = {}
    for path in TEST_DIR.glob("*.fs"):
        rendered_by_name[path.name] = path.read_text(encoding="utf-8")

    parsed: dict[str, tuple[list[str], list[str]]] = {
        name: split_rendered(text) for name, text in rendered_by_name.items()
    }

    for owner, sources in INLINE_GROUPS.items():
        if owner not in parsed:
            continue
        header, _ = parsed[owner]
        body: list[str] = []
        first = True
        for source in sources:
            _, source_body = parsed[source]
            stripped_body = strip_leading_source_header(source_body)
            if not first:
                body.append("")
            first = False
            body.append(render_inline_header(source, source_body))
            body.extend(stripped_body)
        write_text(TEST_DIR / owner, ("\n".join(header + [""] + body)).rstrip() + "\n")

    for name, owners in BACKLINKS.items():
        if name not in parsed:
            continue
        header, _ = parsed[name]
        write_text(TEST_DIR / name, render_backlink(header, owners))


def main() -> None:
    page_index = build_page_index()
    written = 0
    missing_pages: list[str] = []

    for path in sorted(WORD_DIR.glob("*.yml")):
        raw = yaml.safe_load(path.read_text(encoding="utf-8")) or {}
        wordset = raw.get("wordset")
        f12_slug = raw.get("f12-slug")
        if wordset not in WORDSET_DIR or f12_slug is None:
            continue

        f12_slug = str(f12_slug)
        lookup_name = F12_OVERRIDES.get(path.name, f12_slug)
        std_dir = WORDSET_DIR[wordset]
        page = page_index[std_dir].get(normalize_name(lookup_name))
        if page is None:
            missing_pages.append(f"{path.name}: {std_dir}/{f12_slug}")
            continue

        page_text = page.read_text(encoding="utf-8")
        fragment = extract_testing_fragment(page_text)
        lines = normalize_test_lines(fragment) if fragment is not None else []
        word = str(raw.get("word") or path.stem)
        rendered = render_test_file(path.stem, word, str(wordset), f"{std_dir}/{page.name}", lines)
        target = TEST_DIR / f"{path.stem}.fs"
        before = target.read_text(encoding="utf-8") if target.exists() else None
        write_text(target, rendered)
        if before != rendered:
            written += 1

    print(f"updated {written} test files")
    if missing_pages:
        print("missing pages:")
        for item in missing_pages:
            print(f"  {item}")

    postprocess_generated_files()
    generate_suite_files()


if __name__ == "__main__":
    main()
