from __future__ import annotations

import re
from typing import Iterator

from docutils import nodes
from docutils.parsers.rst import directives
from sphinx.domains import Domain, ObjType
from sphinx.roles import XRefRole
from sphinx.util import logging
from sphinx.util.docutils import SphinxDirective
from sphinx.util.nodes import make_refnode

logger = logging.getLogger(__name__)

PROFILE_RE = re.compile(r"^target/([^/]+)/profile/([^/]+)/")
TARGET_RE = re.compile(r"^target/([^/]+)/")


def location_from_docname(docname: str) -> tuple[str | None, str | None]:
    profile_match = PROFILE_RE.match(docname)
    if profile_match:
        return profile_match.group(1), profile_match.group(2)

    target_match = TARGET_RE.match(docname)
    if target_match:
        return target_match.group(1), None

    return None, None


class ForthWordDirective(SphinxDirective):
    required_arguments = 1
    has_content = False
    option_spec = {
        "word": directives.unchanged,
        "target": directives.unchanged,
        "profile": directives.unchanged,
    }

    def run(self) -> list[nodes.Node]:
        slug = self.arguments[0].strip()
        if not slug:
            raise self.error("forth:word requires a non-empty slug")

        word = (self.options.get("word") or slug).strip()
        current_target, current_profile = location_from_docname(self.env.docname)
        target = (self.options.get("target") or "").strip() or current_target
        profile = (self.options.get("profile") or "").strip() or current_profile

        anchor = f"forth-word-{slug}"
        target_node = nodes.target("", "", ids=[anchor])

        domain = self.env.get_domain("forth")
        assert isinstance(domain, ForthDomain)
        domain.register_word(
            slug=slug,
            word=word,
            target=target,
            profile=profile,
            docname=self.env.docname,
            anchor=anchor,
            location=(self.env.docname, self.lineno),
        )

        return [target_node]


class ForthXRefRole(XRefRole):
    def process_link(self, env, refnode, has_explicit_title, title, target):
        refnode["forth:has_explicit_title"] = has_explicit_title
        return title, target.strip()


class ForthDomain(Domain):
    name = "forth"
    label = "Forth"

    object_types = {
        "word": ObjType("word", "word", "op"),
    }

    directives = {
        "word": ForthWordDirective,
    }

    roles = {
        "word": ForthXRefRole(),
        "op": ForthXRefRole(),
    }

    initial_data = {
        # key: (target|None, profile|None, slug) -> record
        "objects": {},
    }

    @property
    def objects(self) -> dict[tuple[str | None, str | None, str], dict[str, str | None]]:
        return self.data["objects"]

    def lookup_by_word(
        self, target: str | None, profile: str | None, word_lower: str
    ) -> dict[str, str | None] | None:
        for (t, p, _slug), record in self.objects.items():
            if t != target or p != profile:
                continue
            rec_word = record.get("word")
            if rec_word and rec_word.lower() == word_lower:
                return record
        return None

    def register_word(
        self,
        slug: str,
        word: str,
        target: str | None,
        profile: str | None,
        docname: str,
        anchor: str,
        location=None,
    ) -> None:
        key = (target or None, profile or None, slug)
        previous = self.objects.get(key)

        if previous is not None:
            if previous["docname"] == docname and previous["anchor"] == anchor:
                self.objects[key] = {
                    "docname": docname,
                    "anchor": anchor,
                    "slug": slug,
                    "word": word,
                    "target": target or None,
                    "profile": profile or None,
                }
                return

            logger.warning(
                "duplicate forth word registration for slug=%r target=%r profile=%r; replacing %s with %s",
                slug,
                target,
                profile,
                previous["docname"],
                docname,
                location=location,
            )

        self.objects[key] = {
            "docname": docname,
            "anchor": anchor,
            "slug": slug,
            "word": word,
            "target": target or None,
            "profile": profile or None,
        }

    def resolve_slug(self, typ: str, target: str) -> str:
        if typ == "op":
            return f"op-{target}"
        return target

    def iter_candidates(
        self,
        fromdocname: str,
        slug: str,
    ) -> Iterator[tuple[str | None, str | None, str]]:
        current_target, current_profile = location_from_docname(fromdocname)
        if current_target is not None and current_profile is not None:
            yield (current_target, current_profile, slug)
        yield (None, None, slug)

    def clear_doc(self, docname: str) -> None:
        stale = [key for key, record in self.objects.items() if record.get("docname") == docname]
        for key in stale:
            del self.objects[key]

    def merge_domaindata(self, docnames, otherdata) -> None:
        for key, record in otherdata.get("objects", {}).items():
            if record.get("docname") in docnames:
                self.objects[key] = record

    def get_objects(self):
        for (target, profile, slug), obj in self.objects.items():
            if target and profile:
                fqname = f"{target}:{profile}:{slug}"
            elif target:
                fqname = f"{target}:{slug}"
            else:
                fqname = slug
            dispname = obj.get("word") or slug
            yield (
                fqname,
                dispname,
                "word",
                obj["docname"],
                obj["anchor"],
                1,
            )

    def resolve_xref(self, env, fromdocname, builder, typ, target, node, contnode):
        slug = self.resolve_slug(typ, target)

        obj = None
        for key in self.iter_candidates(fromdocname, slug):
            obj = self.objects.get(key)
            if obj is not None:
                break

        if obj is None:
            name_lower = target.strip().lower()
            current_target, current_profile = location_from_docname(fromdocname)
            if current_target is not None and current_profile is not None:
                obj = self.lookup_by_word(current_target, current_profile, name_lower)
            if obj is None:
                obj = self.lookup_by_word(None, None, name_lower)

        if obj is None:
            logger.warning(
                "unresolved forth:%s reference %r from %s",
                typ,
                target,
                fromdocname,
                location=node,
            )
            return None

        has_explicit_title = node.get("forth:has_explicit_title", False)
        if not has_explicit_title:
            contnode = nodes.literal(text=obj.get("word") or slug)

        return make_refnode(
            builder,
            fromdocname,
            obj["docname"],
            obj["anchor"],
            contnode,
            obj.get("word") or slug,
        )


def setup(app):
    app.add_domain(ForthDomain)

    return {
        "version": "0.1",
        "parallel_read_safe": True,
        "parallel_write_safe": True,
    }
