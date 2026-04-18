\ Generated from forth-standard.org
\ Word: sliteral
\ Slug: sliteral
\ Wordset: string
\ Source: string/SLITERAL

T{ : s14 [ s1 ] SLITERAL ; -> }T
T{ s1 s14 COMPARE -> 0 }T
T{ s1 s14 ROT = ROT ROT = -> <TRUE> <FALSE> }T
