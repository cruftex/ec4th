\ Generated from forth-standard.org
\ Word: cs-roll
\ Slug: cs-roll
\ Wordset: tools-ext
\ Source: tools/CS-ROLL

\ T{ : ?DONE ( dest -- orig dest ) \ Same as WHILE
\ POSTPONE IF 1 CS-ROLL
; IMMEDIATE -> }T
\ T{ : pt6
\ >R
\ BEGIN
\ R@
\ ?DONE
\ R@
\ R> 1- >R
\ REPEAT
\ R> DROP
; -> }T
T{ 5 pt6 -> 5 4 3 2 1 }T
\ : mix_up 2 CS-ROLL ; IMMEDIATE \ cs-rot
\ : pt7 ( f3 f2 f1 -- ? )
\ IF 1111 ROT ROT ( -- 1111 f3 f2 ) ( cs: -- o1 )
\ IF 2222 SWAP ( -- 1111 2222 f3 ) ( cs: -- o1 o2 )
\ IF ( cs: -- o1 o2 o3 )
\ 3333 mix_up ( -- 1111 2222 3333 ) ( cs: -- o2 o3 o1 )
\ THEN ( cs: -- o2 o3 )
\ 4444 \ Hence failure of first IF comes here and falls through
\ THEN ( cs: -- o2 )
\ 5555 \ Failure of 3rd IF comes here
\ THEN ( cs: -- )
\ 6666 \ Failure of 2nd IF comes here
\ ;
T{ -1 -1 -1 pt7 -> 1111 2222 3333 4444 5555 6666 }T
T{ 0 -1 -1 pt7 -> 1111 2222 5555 6666 }T
T{ 0 0 -1 pt7 -> 1111 0 6666 }T
T{ 0 0 0 pt7 -> 0 0 4444 5555 6666 }T
\ : [1cs-roll] 1 CS-ROLL ; IMMEDIATE
\ T{ : pt8
\ >R
\ AHEAD 111
\ BEGIN 222
\ [1cs-roll]
\ THEN
\ 333
\ R> 1- >R
\ R@ 0<
\ UNTIL
\ R> DROP
; -> }T
T{ 1 pt8 -> 333 222 333 }T
