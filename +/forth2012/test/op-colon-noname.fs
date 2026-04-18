\ Generated from forth-standard.org
\ Word: :noname
\ Slug: op-colon-noname
\ Wordset: core-ext
\ Source: core/ColonNONAME

\ VARIABLE nn1
\ VARIABLE nn2
T{ :NONAME 1234 ; nn1 ! -> }T
T{ :NONAME 9876 ; nn2 ! -> }T
T{ nn1 @ EXECUTE -> 1234 }T
T{ nn2 @ EXECUTE -> 9876 }T
