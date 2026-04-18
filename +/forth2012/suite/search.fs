\ Generated from forth-standard.org testsuite
\ Wordset: search

\ F.16.6.1.1180 DEFINITIONS (from testsuite#test:search:DEFINITIONS)
T{ ONLY FORTH DEFINITIONS -> }T
T{ GET-CURRENT -> FORTH-WORDLIST }T
\ T{ GET-ORDER wid2 @ SWAP 1+ SET-ORDER DEFINITIONS GET-CURRENT
-> wid2 @ }T
T{ GET-ORDER -> get-orderlist wid2 @ SWAP 1+ }T
T{ PREVIOUS GET-ORDER -> get-orderlist }T
T{ DEFINITIONS GET-CURRENT -> FORTH-WORDLIST }T
\ : alsowid2 ALSO GET-ORDER wid2 @ ROT DROP SWAP SET-ORDER ;
\ alsowid2
\ : w1 1234 ;
\ DEFINITIONS
\ : w1 -9876 ; IMMEDIATE
\ ONLY FORTH
T{ w1 -> 1234 }T
\ DEFINITIONS
T{ w1 -> 1234 }T
\ alsowid2
T{ w1 -> -9876 }T
\ DEFINITIONS
T{ w1 -> -9876 }T
\ ONLY FORTH DEFINITIONS
\ : so5 DUP IF SWAP EXECUTE THEN ;
T{ S" w1" wid1 @ SEARCH-WORDLIST so5 -> -1 1234 }T
T{ S" w1" wid2 @ SEARCH-WORDLIST so5 -> 1 -9876 }T
\ : c"w1" C" w1" ;
T{ alsowid2 c"w1" FIND so5 -> 1 -9876 }T
T{ PREVIOUS c"w1" FIND so5 -> -1 1234 }T

\ F.16.6.1.1550 FIND (from testsuite#test:search:FIND)
\ : c"dup" C" DUP" ;
\ : c".(" C" .(" ;
\ : c"x" C" unknown word" ;
T{ c"dup" FIND -> xt @ -1 }T
T{ c".(" FIND -> xti @ 1 }T
T{ c"x" FIND -> c"x" 0 }T

\ F.16.6.1.1595 FORTH-WORDLIST (from testsuite#test:search:FORTH-WORDLIST)
T{ FORTH-WORDLIST wid1 ! -> }T

\ F.16.6.1.2192 SEARCH-WORDLIST (from testsuite#test:search:SEARCH-WORDLIST)
\ ONLY FORTH DEFINITIONS
\ VARIABLE xt ' DUP xt !
\ VARIABLE xti ' .( xti ! \ Immediate word
T{ S" DUP" wid1 @ SEARCH-WORDLIST -> xt @ -1 }T
T{ S" .(" wid1 @ SEARCH-WORDLIST -> xti @ 1 }T
T{ S" DUP" wid2 @ SEARCH-WORDLIST -> 0 }T

\ F.16.6.1.2195 SET-CURRENT (from testsuite#test:search:SET-CURRENT)
T{ GET-CURRENT -> wid1 @ }T
T{ WORDLIST wid2 ! -> }T
T{ wid2 @ SET-CURRENT -> }T
T{ GET-CURRENT -> wid2 @ }T
T{ wid1 @ SET-CURRENT -> }T

\ F.16.6.1.2197 SET-ORDER (from testsuite#test:search:SET-ORDER)
T{ GET-ORDER OVER -> GET-ORDER wid1 @ }T
T{ GET-ORDER SET-ORDER -> }T
T{ GET-ORDER -> get-orderlist }T
\ \tab \word{bs} \textdf{Check nothing changed} \\
T{ get-orderlist DROP get-orderList 2* SET-ORDER -> }T
T{ GET-ORDER -> get-orderlist DROP get-orderList 2* }T
T{ get-orderlist SET-ORDER GET-ORDER -> get-orderlist }T
\ : so2a GET-ORDER get-orderlist SET-ORDER ;
\ : so2 0 SET-ORDER so2a ;
T{ so2 -> 0 }T \ 0 SET-ORDER leaves an empty search order
\ : so3 -1 SET-ORDER so2a ;
\ : so4 ONLY so2a ;
T{ so3 -> so4 }T \ -1 SET-ORDER is the same as ONLY

\ F.16.6.2.0715 ALSO (from testsuite#test:search:ALSO)
T{ ALSO GET-ORDER ONLY -> get-orderlist OVER SWAP 1+ }T

\ F.16.6.2.1965 ONLY (from testsuite#test:search:ONLY)
T{ ONLY FORTH GET-ORDER -> get-orderlist }T
\ : so1 SET-ORDER ; \ In case it is unavailable in the forth wordlist
T{ ONLY FORTH-WORDLIST 1 SET-ORDER get-orderlist so1 -> }T
T{ GET-ORDER -> get-orderlist }T

\ F.16.6.2.1985 ORDER (from testsuite#test:search:ORDER)
\ CR .( ONLY FORTH DEFINITIONS search order and compilation list) CR
T{ ONLY FORTH DEFINITIONS ORDER -> }T
\ CR .( Plus another unnamed wordlist at head of search order) CR
T{ alsowid2 DEFINITIONS ORDER -> }T
\ F.20 The optional String word set
\ T.17
\ Most of the tests in this wordlist require a known string which is
\ defined as:
T{ : s1 S" abcdefghijklmnopqrstuvwxyz" ; -> }T
\ The tests should be carried out in the order:
\ F.17.6.1.0245 /STRING,
\ F.17.6.1.2191 SEARCH,
\ F.17.6.1.0170 -TRAILING,
\ F.17.6.1.0935 COMPARE,
\ F.17.6.1.0780 BLANK and
\ F.17.6.1.2212 SLITERAL.
