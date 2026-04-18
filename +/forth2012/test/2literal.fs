\ Generated from forth-standard.org
\ Word: 2literal
\ Slug: 2literal
\ Wordset: double
\ Source: double/TwoLITERAL

T{ : cd1 [ MAX-2INT ] 2LITERAL ; -> }T
T{ cd1 -> MAX-2INT }T
T{ 2VARIABLE 2v4 IMMEDIATE 5 6 2v4 2! -> }T
T{ : cd7 2v4 [ 2@ ] 2LITERAL ; cd7 -> 5 6 }T
T{ : cd8 [ 6 7 ] 2v4 [ 2! ] ; 2v4 2@ -> 6 7 }T
