\ Generated from forth-standard.org
\ Word: rshift
\ Slug: rshift
\ Wordset: core
\ Source: core/RSHIFT

\ T.6.1.2162 RSHIFT
T{ 1 0 RSHIFT -> 1 }T
T{ 1 1 RSHIFT -> 0 }T
T{ 2 1 RSHIFT -> 1 }T
T{ 4 2 RSHIFT -> 1 }T
T{ 8000 F RSHIFT -> 1 }T \ Biggest
T{ MSB 1 RSHIFT MSB AND -> 0 }T \ RSHIFT zero fills MSBs
T{ MSB 1 RSHIFT 2* -> MSB }T
