\ Generated from forth-standard.org
\ Word: >r
\ Slug: op-to-r
\ Wordset: core
\ Source: core/toR

\ T.6.1.0580 >R (from op-to-r.fs)
T{ : GR1 >R R> ; -> }T
T{ : GR2 >R R@ R> DROP ; -> }T
T{ 123 GR1 -> 123 }T
T{ 123 GR2 -> 123 }T
T{ 1S GR1 -> 1S }T ( Return stack holds cells )
