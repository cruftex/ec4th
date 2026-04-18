\ Generated from forth-standard.org
\ Word: find
\ Slug: find
\ Wordset: core
\ Source: core/FIND

\ T.6.1.1550 FIND
\ HERE
\ 3 C,
\ CHAR G C,
\ CHAR T C,
\ CHAR 1 C,
\ CONSTANT GT1STRING
\ HERE
\ 3 C,
\ CHAR G C,
\ CHAR T C,
\ CHAR 2 C,
\ CONSTANT GT2STRING
T{ GT1STRING FIND -> ' GT1 -1 }T
T{ GT2STRING FIND -> ' GT2 1 }T
( HOW TO SEARCH FOR NON-EXISTENT WORD? )
