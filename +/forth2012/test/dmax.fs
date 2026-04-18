\ Generated from forth-standard.org
\ Word: dmax
\ Slug: dmax
\ Wordset: double
\ Source: double/DMAX

T{ 1. 2. DMAX -> 2. }T
T{ 1. 0. DMAX -> 1. }T
T{ 1. -1. DMAX -> 1. }T
T{ 1. 1. DMAX -> 1. }T
T{ 0. 1. DMAX -> 1. }T
T{ 0. -1. DMAX -> 0. }T
T{ -1. 1. DMAX -> 1. }T
T{ -1. -2. DMAX -> -1. }T
T{ MAX-2INT HI-2INT DMAX -> MAX-2INT }T
T{ MAX-2INT MIN-2INT DMAX -> MAX-2INT }T
T{ MIN-2INT MAX-2INT DMAX -> MAX-2INT }T
T{ MIN-2INT LO-2INT DMAX -> LO-2INT }T
T{ MAX-2INT 1. DMAX -> MAX-2INT }T
T{ MAX-2INT -1. DMAX -> MAX-2INT }T
T{ MIN-2INT 1. DMAX -> 1. }T
T{ MIN-2INT -1. DMAX -> -1. }T
