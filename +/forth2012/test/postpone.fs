\ Generated from forth-standard.org
\ Word: postpone
\ Slug: postpone
\ Wordset: core
\ Source: core/POSTPONE

\ T.6.1.2033 POSTPONE
T{ : GT4 POSTPONE GT1 ; IMMEDIATE -> }T
T{ : GT5 GT4 ; -> }T
T{ GT5 -> 123 }T
T{ : GT6 345 ; IMMEDIATE -> }T
T{ : GT7 POSTPONE GT6 ; -> }T
T{ GT7 -> 345 }T
