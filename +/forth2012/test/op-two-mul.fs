\ Generated from forth-standard.org
\ Word: 2*
\ Slug: op-two-mul
\ Wordset: core
\ Source: core/TwoTimes

\ T.6.1.0320 2*
T{ 0S 2* -> 0S }T
T{ 1 2* -> 2 }T
T{ 4000 2* -> 8000 }T
T{ 1S 2* 1 XOR -> 1S }T
T{ MSB 2* -> 0S }T
