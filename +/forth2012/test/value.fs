\ Generated from forth-standard.org
\ Word: value
\ Slug: value
\ Wordset: core-ext
\ Source: core/VALUE

\ T.6.2.2405 VALUE (from value.fs)
T{ 111 VALUE v1 -> }T
T{ -999 VALUE v2 -> }T
T{ v1 -> 111 }T
T{ v2 -> -999 }T
T{ 222 TO v1 -> }T
T{ v1 -> 222 }T
T{ : vd1 v1 ; -> }T
T{ vd1 -> 222 }T
T{ : vd2 TO v2 ; -> }T
T{ v2 -> -999 }T
T{ -333 vd2 -> }T
T{ v2 -> -333 }T
T{ v1 -> 222 }T
