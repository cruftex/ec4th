\ Generated from forth-standard.org
\ Word: m*/
\ Slug: op-m-times-div
\ Wordset: double
\ Source: double/MTimesDiv

\ To correct the result if the division is floored,
\ only used when necessary, i.e., negative quotient and
\ remainder <>= 0.
\ : ?floored [ -3 2 / -2 = ] LITERAL IF 1. D- THEN ;
T{ 5. 7 11 M*/ -> 3. }T
T{ 5. -7 11 M*/ -> -3. ?floored }T
T{ -5. 7 11 M*/ -> -3. ?floored }T
T{ -5. -7 11 M*/ -> 3. }T
T{ MAX-2INT 8 16 M*/ -> HI-2INT }T
T{ MAX-2INT -8 16 M*/ -> HI-2INT DNEGATE ?floored }T
T{ MIN-2INT 8 16 M*/ -> LO-2INT }T
T{ MIN-2INT -8 16 M*/ -> LO-2INT DNEGATE }T
T{ MAX-2INT MAX-INT MAX-INT M*/ -> MAX-2INT }T
T{ MAX-2INT MAX-INT 2/ MAX-INT M*/ -> MAX-INT 1- HI-2INT NIP }T
T{ MIN-2INT LO-2INT NIP DUP NEGATE M*/ -> MIN-2INT }T
T{ MIN-2INT LO-2INT NIP 1- MAX-INT M*/ -> MIN-INT 3 + HI-2INT NIP 2 + }T
T{ MAX-2INT LO-2INT NIP DUP NEGATE M*/ -> MAX-2INT DNEGATE }T
T{ MIN-2INT MAX-INT DUP M*/ -> MIN-2INT }T
