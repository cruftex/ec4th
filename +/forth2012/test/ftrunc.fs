\ Generated from forth-standard.org
\ Word: ftrunc
\ Slug: ftrunc
\ Wordset: floating-ext
\ Source: float/FTRUNC

\ SET-EXACT
T{ -0E FTRUNC F0= -> <TRUE> }T
T{ -1E-9 FTRUNC F0= -> <TRUE> }T
T{ -0.9E FTRUNC F0= -> <TRUE> }T
T{ -1E 1E-5 F+ FTRUNC F0= -> <TRUE> }T
T{ 0E FTRUNC -> 0E R}T
T{ 1E-9 FTRUNC -> 0E R}T
T{ -1E -1E-5 F+ FTRUNC -> -1E R}T
T{ 3.14E FTRUNC -> 3E R}T
T{ 3.99E FTRUNC -> 3E R}T
T{ 4E FTRUNC -> 4E R}T
T{ -4E FTRUNC -> -4E R}T
T{ -4.1E FTRUNC -> -4E R}T
