\ Generated from forth-standard.org
\ Word: if
\ Slug: if
\ Wordset: core
\ Source: core/IF

\ T.6.1.1700 IF (from if.fs)
T{ : GI1 IF 123 THEN ; -> }T
T{ : GI2 IF 123 ELSE 234 THEN ; -> }T
T{ 0 GI1 -> }T
T{ 1 GI1 -> 123 }T
T{ -1 GI1 -> 123 }T
T{ 0 GI2 -> 234 }T
T{ 1 GI2 -> 123 }T
T{ -1 GI1 -> 123 }T
\ Multiple ELSEs in an IF statement
\ : melse IF 1 ELSE 2 ELSE 3 ELSE 4 ELSE 5 THEN ;
T{ <FALSE> melse -> 2 4 }T
T{ <TRUE> melse -> 1 3 5 }T
