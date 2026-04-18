\ Generated from forth-standard.org
\ Word: d<
\ Slug: op-d-less
\ Wordset: double
\ Source: double/Dless

T{ 0. 1. D< -> <TRUE> }T
T{ 0. 0. D< -> <FALSE> }T
T{ 1. 0. D< -> <FALSE> }T
T{ -1. 1. D< -> <TRUE> }T
T{ -1. 0. D< -> <TRUE> }T
T{ -2. -1. D< -> <TRUE> }T
T{ -1. -2. D< -> <FALSE> }T
T{ -1. MAX-2INT D< -> <TRUE> }T
T{ MIN-2INT MAX-2INT D< -> <TRUE> }T
T{ MAX-2INT -1. D< -> <FALSE> }T
T{ MAX-2INT MIN-2INT D< -> <FALSE> }T
T{ MAX-2INT 2DUP -1. D+ D< -> <FALSE> }T
T{ MIN-2INT 2DUP 1. D+ D< -> <TRUE> }T
