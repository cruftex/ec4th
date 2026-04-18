\ Generated from forth-standard.org
\ Word: lshift
\ Slug: lshift
\ Wordset: core
\ Source: core/LSHIFT

\ T.6.1.1805 LSHIFT
T{ 1 0 LSHIFT -> 1 }T
T{ 1 1 LSHIFT -> 2 }T
T{ 1 2 LSHIFT -> 4 }T
T{ 1 F LSHIFT -> 8000 }T \ BIGGEST GUARANTEED SHIFT
T{ 1S 1 LSHIFT 1 XOR -> 1S }T
T{ MSB 1 LSHIFT -> 0 }T
