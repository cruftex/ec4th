\ Generated from forth-standard.org
\ Word: dmin
\ Slug: dmin
\ Wordset: double
\ Source: double/DMIN

T{ 1. 2. DMIN -> 1. }T
T{ 1. 0. DMIN -> 0. }T
T{ 1. -1. DMIN -> -1. }T
T{ 1. 1. DMIN -> 1. }T
T{ 0. 1. DMIN -> 0. }T
T{ 0. -1. DMIN -> -1. }T
T{ -1. 1. DMIN -> -1. }T
T{ -1. -2. DMIN -> -2. }T
T{ MAX-2INT HI-2INT DMIN -> HI-2INT }T
T{ MAX-2INT MIN-2INT DMIN -> MIN-2INT }T
T{ MIN-2INT MAX-2INT DMIN -> MIN-2INT }T
T{ MIN-2INT LO-2INT DMIN -> MIN-2INT }T
T{ MAX-2INT 1. DMIN -> 1. }T
T{ MAX-2INT -1. DMIN -> -1. }T
T{ MIN-2INT 1. DMIN -> MIN-2INT }T
T{ MIN-2INT -1. DMIN -> MIN-2INT }T
