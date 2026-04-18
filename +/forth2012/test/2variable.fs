\ Generated from forth-standard.org
\ Word: 2variable
\ Slug: 2variable
\ Wordset: double
\ Source: double/TwoVARIABLE

T{ 2VARIABLE 2v1 -> }T
T{ 0. 2v1 2! -> }T
T{ 2v1 2@ -> 0. }T
T{ -1 -2 2v1 2! -> }T
T{ 2v1 2@ -> -1 -2 }T
T{ : cd2 2VARIABLE ; -> }T
T{ cd2 2v2 -> }T
T{ : cd3 2v2 2! ; -> }T
T{ -2 -1 cd3 -> }T
T{ 2v2 2@ -> -2 -1 }T
T{ 2VARIABLE 2v3 IMMEDIATE 5 6 2v3 2! -> }T
T{ 2v3 2@ -> 5 6 }T
