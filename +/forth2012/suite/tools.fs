\ Generated from forth-standard.org testsuite
\ Wordset: tools

\ F.15.6.2.0702 AHEAD (from testsuite#test:tools:AHEAD)
T{ : pt1 AHEAD 1111 2222 THEN 3333 ; -> }T
T{ pt1 -> 3333 }T

\ F.15.6.2.1015 CS-PICK (from testsuite#test:tools:CS-PICK)
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

\ F.15.6.2.1020 CS-ROLL (from testsuite#test:tools:CS-ROLL)
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

\ F.15.6.2.1908 N&gt;R (from testsuite#test:tools:NtoR)
\ : TNR1 N>R SWAP NR> ;
T{ 1 2 10 20 30 3 TNR1 -> 2 1 10 20 30 3 }T
\ : TNR2 N>R N>R SWAP NR> NR> ;
T{ 1 2 10 20 30 3 40 50 2 TNR2 -> 2 1 10 20 30 3 40 50 2 }T

\ F.15.6.2.2533 [THEN] (from testsuite#test:tools:[THEN])
T{ <TRUE> [IF] 111 [ELSE] 222 [THEN] -> 111 }T
T{ <FALSE> [IF] 111 [ELSE] 222 [THEN] -> 222 }T
\ Check words are immediate
\ : tfind BL WORD FIND ;
T{ tfind [IF] NIP -> 1 }T
T{ tfind [ELSE] NIP -> 1 }T
T{ tfind [THEN] NIP -> 1 }T
T{ : pt2 [ 0 ] [IF] 1111 [ELSE] 2222 [THEN] ; pt2 -> 2222 }T
T{ : pt3 [ -1 ] [IF] 3333 [ELSE] 4444 [THEN] ; pt3 -> 3333 }T
\ Code spread over more than 1 line
\ T{ <TRUE> [IF] 1
\ 2
\ [ELSE]
\ 3
\ 4
[THEN] -> 1 2 }T
\ T{ <FALSE> [IF]
\ 1 2
\ [ELSE]
\ 3 4
[THEN] -> 3 4 }T
\ Nested
\ : <T> <TRUE> ;
\ : <F> <FALSE> :
T{ <T> [IF] 1 <T> [IF] 2 [ELSE] 3 [THEN] [ELSE] 4 [THEN] -> 1 2 }T
T{ <F> [IF] 1 <T> [IF] 2 [ELSE] 3 [THEN] [ELSE] 4 [THEN] -> 4 }T
T{ <T> [IF] 1 <F> [IF] 2 [ELSE] 3 [THEN] [ELSE] 4 [THEN] -> 1 3 }T
T{ <F> [IF] 1 <F> [IF] 2 [ELSE] 3 [THEN] [ELSE] 4 [THEN] -> 4 }T
\ T.15
\ F.19 The optional Search-Order word set
\ T.16
\ The search order is reset to a known state before the tests can be
\ run.
\ ONLY FORTH DEFINITIONS
\ Define two word list (wid) variables used by the tests.
\ VARIABLE wid1
\ VARIABLE wid2
\ In order to test the search order it in necessary to remember the
\ existing search order before modifying it. The existing search order
\ is saved and the get-orderlist defined to access it.
\ : save-orderlist ( widn ... wid1 n -- )
\ DUP , 0 ?DO , LOOP
\ ;
\ CREATE order-list
T{ GET-ORDER save-orderlist -> }T
\ : get-orderlist ( -- widn ... wid1 n )
\ order-list DUP @ CELLS ( -- ad n )
\ OVER + ( -- AD AD' )
\ ?DO I @ -1 CELLS +LOOP ( -- )
\ ;
\ Having obtained a copy of the current wordlist, the testing of the
\ wordlist can begin with test F.16.6.1.1595 FORTH-WORDLIST followed
\ by F.16.6.1.2197 SET-ORDER which also test GET-ORDER, then
\ F.16.6.2.0715 ALSO and F.16.6.2.1965 ONLY before moving on to
\ F.16.6.1.2195 SET-CURRENT which also test GET-CURRENT and
\ WORDLIST. This should be followed by the test
\ F.16.6.1.1180 DEFINITIONS which also tests PREVIOUS and the
\ F.16.6.1.2192 SEARCH-WORDLIST and F.16.6.1.1550 FIND tests.
\ Finally the F.16.6.2.1985 ORDER test can be performed.
