.PHONY: all clean doc forth2012-report

SYM ?= output/ec4th-arduino-nano-regular.sym
OUT ?= doc/forth2012-core-wordset-coverage.md

all:
	bash build.sh

forth2012-report:
	python3 tools/forth2012_wordset_report.py "$(SYM)" "$(OUT)"

doc:
	PYTHONPYCACHEPREFIX=output/pycache python3 doc/build_word_docs.py
	PYTHONPYCACHEPREFIX=output/pycache sphinx-build -d output/doc-doctrees -b html -c doc output/doc output/doc-html

clean:
	rm -rf output
