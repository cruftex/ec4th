\ Generated from forth-standard.org
\ Word: d0=
\ Slug: op-d-zero-equal
\ Wordset: double
\ Source: double/DZeroEqual

T{ 1. D0= -> <FALSE> }T
T{ MIN-INT 0 D0= -> <FALSE> }T
T{ MAX-2INT D0= -> <FALSE> }T
T{ -1 MAX-INT D0= -> <FALSE> }T
T{ 0. D0= -> <TRUE> }T
T{ -1. D0= -> <FALSE> }T
T{ 0 MIN-INT D0= -> <FALSE> }T
