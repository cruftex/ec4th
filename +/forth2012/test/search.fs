\ Generated from forth-standard.org
\ Word: search
\ Slug: search
\ Wordset: string
\ Source: string/SEARCH

T{ : s2 S" abc" ; -> }T
T{ : s3 S" jklmn" ; -> }T
T{ : s4 S" z" ; -> }T
T{ : s5 S" mnoq" ; -> }T
T{ : s6 S" 12345" ; -> }T
T{ : s7 S" " ; -> }T
T{ s1 s2 SEARCH -> s1 <TRUE> }T
T{ s1 s3 SEARCH -> s1 9 /STRING <TRUE> }T
T{ s1 s4 SEARCH -> s1 25 /STRING <TRUE> }T
T{ s1 s5 SEARCH -> s1 <FALSE> }T
T{ s1 s6 SEARCH -> s1 <FALSE> }T
T{ s1 s7 SEARCH -> s1 <TRUE> }T
