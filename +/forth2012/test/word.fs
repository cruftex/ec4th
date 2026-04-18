\ Generated from forth-standard.org
\ Word: word
\ Slug: word
\ Wordset: core
\ Source: core/WORD]

\ T.6.1.2450 WORD
\ : GS3 WORD COUNT SWAP C@ ;
T{ BL GS3 HELLO -> 5 CHAR H }T
T{ CHAR " GS3 GOODBYE" -> 7 CHAR G }T
\ T{ BL GS3
DROP -> 0 }T \ Blank lines return zero-length strings
