\ Generated from forth-standard.org
\ Word: cs-pick
\ Slug: cs-pick
\ Wordset: tools-ext
\ Source: tools/CS-PICK

\ : ?repeat
\ 0 CS-PICK POSTPONE UNTIL
\ ; IMMEDIATE
\ VARIABLE pt4
\ : <= > 0= ;
\ T{ : pt5 ( n1 -- )
\ pt4 !
\ BEGIN
\ -1 pt4 +!
\ pt4 @ 4 <= ?repeat \ Back to BEGIN if false
\ 111
\ pt4 @ 3 <= ?repeat
\ 222
\ pt4 @ 2 <= ?repeat
\ 333
\ pt4 @ 1 =
\ UNTIL
; -> }T
T{ 6 pt5 -> 111 111 222 111 222 333 111 222 333 }T
