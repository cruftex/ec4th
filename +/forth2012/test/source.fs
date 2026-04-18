\ Generated from forth-standard.org
\ Word: source
\ Slug: source
\ Wordset: core
\ Source: core/SOURCE

\ T.6.1.2216 SOURCE
\ : GS1 S" SOURCE" 2DUP EVALUATE
\ >R SWAP >R = R> R> = ;
T{ GS1 -> <TRUE> <TRUE> }T
\ : GS4 SOURCE >IN ! DROP ;
\ T{ GS4 123 456
-> }T
