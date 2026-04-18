\ Generated from forth-standard.org
\ Word: 2value
\ Slug: 2value
\ Wordset: double-ext
\ Source: double/TwoVALUE

T{ 1 2 2VALUE t2val -> }T
T{ t2val -> 1 2 }T
T{ 3 4 TO t2val -> }T
T{ t2val -> 3 4 }T
\ : sett2val t2val 2SWAP TO t2val ;
T{ 5 6 sett2val t2val -> 3 4 5 6 }T
