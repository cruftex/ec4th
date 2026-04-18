\ Generated from forth-standard.org
\ Word: d=
\ Slug: op-d-equal
\ Wordset: double
\ Source: double/DEqual

T{ -1. -1. D= -> <TRUE> }T
T{ -1. 0. D= -> <FALSE> }T
T{ -1. 1. D= -> <FALSE> }T
T{ 0. -1. D= -> <FALSE> }T
T{ 0. 0. D= -> <TRUE> }T
T{ 0. 1. D= -> <FALSE> }T
T{ 1. -1. D= -> <FALSE> }T
T{ 1. 0. D= -> <FALSE> }T
T{ 1. 1. D= -> <TRUE> }T
T{ 0 -1 0 -1 D= -> <TRUE> }T
T{ 0 -1 0 0 D= -> <FALSE> }T
T{ 0 -1 0 1 D= -> <FALSE> }T
T{ 0 0 0 -1 D= -> <FALSE> }T
T{ 0 0 0 0 D= -> <TRUE> }T
T{ 0 0 0 1 D= -> <FALSE> }T
T{ 0 1 0 -1 D= -> <FALSE> }T
T{ 0 1 0 0 D= -> <FALSE> }T
T{ 0 1 0 1 D= -> <TRUE> }T
T{ MAX-2INT MIN-2INT D= -> <FALSE> }T
T{ MAX-2INT 0. D= -> <FALSE> }T
T{ MAX-2INT MAX-2INT D= -> <TRUE> }T
T{ MAX-2INT HI-2INT D= -> <FALSE> }T
T{ MAX-2INT MIN-2INT D= -> <FALSE> }T
T{ MIN-2INT MIN-2INT D= -> <TRUE> }T
T{ MIN-2INT LO-2INT D= -> <FALSE> }T
T{ MIN-2INT MAX-2INT D= -> <FALSE> }T
