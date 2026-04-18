\ Generated from forth-standard.org
\ Word: evaluate
\ Slug: evaluate
\ Wordset: core
\ Source: core/EVALUATE

\ T.6.1.1360 EVALUATE
\ : GE1 S" 123" ; IMMEDIATE
\ : GE2 S" 123 1+" ; IMMEDIATE
\ : GE3 S" : GE4 345 ;" ;
\ : GE5 EVALUATE ; IMMEDIATE
T{ GE1 EVALUATE -> 123 }T ( TEST EVALUATE IN INTERP. STATE )
T{ GE2 EVALUATE -> 124 }T
T{ GE3 EVALUATE -> }T
T{ GE4 -> 345 }T
T{ : GE6 GE1 GE5 ; -> }T ( TEST EVALUATE IN COMPILE STATE )
T{ GE6 -> 123 }T
T{ : GE7 GE2 GE5 ; -> }T
T{ GE7 -> 124 }T
\ See F.9.3.6 for additional test.
