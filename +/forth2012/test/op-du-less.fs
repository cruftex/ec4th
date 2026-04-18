\ Generated from forth-standard.org
\ Word: du<
\ Slug: op-du-less
\ Wordset: double-ext
\ Source: double/DUless

T{ 1. 1. DU< -> <FALSE> }T
T{ 1. -1. DU< -> <TRUE> }T
T{ -1. 1. DU< -> <FALSE> }T
T{ -1. -2. DU< -> <FALSE> }T
T{ MAX-2INT HI-2INT DU< -> <FALSE> }T
T{ HI-2INT MAX-2INT DU< -> <TRUE> }T
T{ MAX-2INT MIN-2INT DU< -> <TRUE> }T
T{ MIN-2INT MAX-2INT DU< -> <FALSE> }T
T{ MIN-2INT LO-2INT DU< -> <TRUE> }T
