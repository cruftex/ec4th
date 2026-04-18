\ Generated from forth-standard.org
\ Word: cells
\ Slug: cells
\ Wordset: core
\ Source: core/CELLS

\ T.6.1.0890 CELLS
\ : BITS ( X -- U )
\ 0 SWAP BEGIN DUP WHILE
\ DUP MSB AND IF >R 1+ R> THEN 2*
\ REPEAT DROP
\ ;
( CELLS >= 1 AU, INTEGRAL MULTIPLE OF CHAR SIZE, >= 16 BITS )
T{ 1 CELLS 1 < -> <FALSE> }T
T{ 1 CELLS 1 CHARS MOD -> 0 }T
T{ 1S BITS 10 < -> <FALSE> }T
