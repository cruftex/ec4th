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

PROFILE_RE = re.compile(r"^profile/([^/]+)/")


def profile_from_docname(docname: str) -> str | None:
    match = PROFILE_RE.match(docname)
    return match.group(1) if match else None


class ForthWordDirective(SphinxDirective):
    """
    Register a documented Forth word page.

    Usage in MyST markdown:

    ```{forth:word} swap
    :word: swap
    :profile: avr
    ```

    Or for an operator-like word:

    ```{forth:word} op-plus
    :word: +
    :profile: avr
    ```
    """

    required_arguments = 1
    has_content = False
    option_spec = {
        "word": directives.unchanged,
        "profile": directives.unchanged,
    }

    def run(self) -> list[nodes.Node]:
        slug = self.arguments[0].strip()
        if not slug:
            raise self.error("forth:word requires a non-empty slug")

        word = (self.options.get("word") or slug).strip()
        profile = (self.options.get("profile") or "").strip() or profile_from_docname(self.env.docname)

        anchor = f"forth-word-{slug}"
        target = nodes.target("", "", ids=[anchor])

        domain = self.env.get_domain("forth")
        assert isinstance(domain, ForthDomain)
        domain.register_word(
            slug=slug,
            word=word,
            profile=profile,
            docname=self.env.docname,
            anchor=anchor,
            location=(self.env.docname, self.lineno),
        )

        return [target]


class ForthXRefRole(XRefRole):
    """
    Cross-reference role for Forth words.

    Supported roles:
      - forth:word  -> target is a slug, e.g. `swap`
      - forth:op    -> target is operator shorthand, e.g. `plus` -> `op-plus`

    Explicit titles are supported:
      {forth:word}`exchange <swap>`
      {forth:op}`+ <plus>`
    """

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
        # key: (profile|None, slug) -> record
        "objects": {},
    }

    @property
    def objects(self) -> dict[tuple[str | None, str], dict[str, str | None]]:
        return self.data["objects"]

    def register_word(
        self,
        slug: str,
        word: str,
        profile: str | None,
        docname: str,
        anchor: str,
        location=None,
    ) -> None:
        key = (profile or None, slug)
        previous = self.objects.get(key)

        if previous is not None:
            logger.warning(
                "duplicate forth word registration for slug=%r profile=%r; replacing %s with %s",
                slug,
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
            "profile": profile or None,
        }

    def resolve_slug(self, typ: str, target: str) -> str:
        if typ == "op":
            return f"op-{target}"
        return target

    def iter_candidates(self, fromdocname: str, slug: str) -> Iterator[tuple[str | None, str]]:
        current_profile = profile_from_docname(fromdocname)
        if current_profile is not None:
            yield (current_profile, slug)
        yield (None, slug)

    def get_objects(self):
        for (profile, slug), obj in self.objects.items():
            fqname = f"{profile}:{slug}" if profile else slug
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
