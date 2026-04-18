\ Generated from forth-standard.org
\ Word: 2constant
\ Slug: 2constant
\ Wordset: double
\ Source: double/TwoCONSTANT

T{ 1 2 2CONSTANT 2c1 -> }T
T{ 2c1 -> 1 2 }T
T{ : cd1 2c1 ; -> }T
T{ cd1 -> 1 2 }T
T{ : cd2 2CONSTANT ; -> }T
T{ -1 -2 cd2 2c2 -> }T
T{ 2c2 -> -1 -2 }T
T{ 4 5 2CONSTANT 2c3 IMMEDIATE 2c3 -> 4 5 }T
T{ : cd6 2c3 2LITERAL ; cd6 -> 4 5 }T
