import os
import sys

# allow importing forth_domain.py
sys.path.insert(0, os.path.abspath(os.path.dirname(__file__)))

extensions = [
    "myst_parser",
    "forth_domain",
]

source_suffix = {
    ".md": "myst",
}

master_doc = "index"
project = "ec4th"

exclude_patterns = ["_build"]

html_theme = "alabaster"
