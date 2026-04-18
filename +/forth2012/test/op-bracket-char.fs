\ Generated from forth-standard.org
\ Word: [char]
\ Slug: op-bracket-char
\ Wordset: core
\ Source: core/BracketCHAR

\ T.6.1.2520 [CHAR]
T{ : GC1 [CHAR] X ; -> }T
T{ : GC2 [CHAR] HELLO ; -> }T
T{ GC1 -> 58 }T
T{ GC2 -> 48 }T
