\ Generated from forth-standard.org
\ Word: 2/
\ Slug: op-two-slash
\ Wordset: core
\ Source: core/TwoDiv

\ T.6.1.0330 2/
T{ 0S 2/ -> 0S }T
T{ 1 2/ -> 0 }T
T{ 4000 2/ -> 2000 }T
T{ 1S 2/ -> 1S }T \ MSB PROPOGATED
T{ 1S 1 XOR 2/ -> 1S }T
T{ MSB 2/ MSB AND -> MSB }T
