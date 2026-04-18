\ Generated from forth-standard.org
\ Word: s"
\ Slug: op-s-quote
\ Wordset: core
\ Source: core/Sq

\ A.6.1.2165 S"
T{ : GC4 S" XY" ; -> }T
T{ GC4 SWAP DROP -> 2 }T
T{ GC4 DROP DUP C@ SWAP CHAR+ C@ -> 58 59 }T
\ : GC5 S" A String"2DROP ; \ There is no space between the " and 2DROP
T{ GC5 -> }T
