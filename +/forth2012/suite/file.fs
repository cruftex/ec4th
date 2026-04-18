\ Generated from forth-standard.org testsuite
\ Wordset: file

\ F.11.6.1.0080 ( (from testsuite#test:file:p)
\ T{ ( 1 2 3
\ 4 5 6
7 8 9 ) 11 22 33 -> 11 22 33 }T

\ F.11.6.1.1010 CREATE-FILE (from testsuite#test:file:CREATE-FILE)
\ : fn1 S" fatest1.txt" ;
\ VARIABLE fid1
T{ fn1 R/W CREATE-FILE SWAP fid1 ! -> 0 }T
T{ fid1 @ CLOSE-FILE -> 0 }T

\ F.11.6.1.1190 DELETE-FILE (from testsuite#test:file:DELETE-FILE)
T{ fn2 DELETE-FILE -> 0 }T
T{ fn2 R/W BIN OPEN-FILE SWAP DROP -> 0 }T
T{ fn2 DELETE-FILE -> 0 }T

\ F.11.6.1.1522 FILE-SIZE (from testsuite#test:file:FILE-SIZE)
\ : cbuf buf bsize 0 FILL ;
\ : fn2 S" fatest2.txt" ;
\ VARIABLE fid2
\ : setpad PAD 50 0 DO I OVER C! CHAR+ LOOP DROP ;
\ setpad
\ Note: If anything else is defined
\ setpad must be called again as the pad may move
T{ fn2 R/W BIN CREATE-FILE SWAP fid2 ! -> 0 }T
T{ PAD 50 fid2 @ WRITE-FILE fid2 @ FLUSH-FILE -> 0 0 }T
T{ fid2 @ FILE-SIZE -> 50. 0 }T
T{ 0. fid2 @ REPOSITION-FILE -> 0 }T
T{ cbuf buf 29 fid2 @ READ-FILE -> 29 0 }T
T{ PAD 29 buf 29 COMPARE -> 0 }T
T{ PAD 30 buf 30 COMPARE -> 1 }T
T{ cbuf buf 29 fid2 @ READ-FILE -> 21 0 }T
T{ PAD 29 + 21 buf 21 COMPARE -> 0 }T
T{ fid2 @ FILE-SIZE DROP fid2 @ FILE-POSITION DROP D= -> <TRUE> }T
T{ buf 10 fid2 @ READ-FILE -> 0 0 }T
T{ fid2 @ CLOSE-FILE -> 0 }T

\ F.11.6.1.2090 READ-LINE (from testsuite#test:file:READ-LINE)
\ 200 CONSTANT bsize
\ CREATE buf bsize ALLOT
\ VARIABLE #chars
T{ fn1 R/O OPEN-FILE SWAP fid1 ! -> 0 }T
T{ fid1 @ FILE-POSITION -> 0. 0 }T
\ T{ buf 100 fid1 @ READ-LINE ROT DUP #chars ! ->
<TRUE> 0 line1 SWAP DROP }T
T{ buf #chars @ line1 COMPARE -> 0 }T
T{ fid1 @ CLOSE-FILE -> 0 }T

\ F.11.6.1.2142 REPOSITION-FILE (from testsuite#test:file:REPOSITION-FILE)
\ : line2 S" Line 2 blah blah blah" ;
\ : rl1 buf 100 fid1 @ READ-LINE ;
\ 2VARIABLE fp
T{ fn1 R/W OPEN-FILE SWAP fid1 ! -> 0 }T
T{ fid1 @ FILE-SIZE DROP fid1 @ REPOSITION-FILE -> 0 }T
T{ fid1 @ FILE-SIZE -> fid1 @ FILE-POSITION }T
T{ line2 fid1 @ WRITE-FILE -> 0 }T
T{ 10. fid1 @ REPOSITION-FILE -> 0 }T
T{ fid1 @ FILE-POSITION -> 10. 0 }T
T{ 0. fid1 @ REPOSITION-FILE -> 0 }T
T{ rl1 -> line1 SWAP DROP <TRUE> 0 }T
T{ rl1 -> ROT DUP #chars ! }T<TRUE> 0 line2 SWAP DROP
T{ buf #chars @ line2 COMPARE -> 0 }T
T{ rl1 -> 0 <FALSE> 0 }T
T{ fid1 @ FILE-POSITION ROT ROT fp 2! -> 0 }T
T{ fp 2@ fid1 @ FILE-SIZE DROP D= -> <TRUE> }T
T{ S" " fid1 @ WRITE-LINE -> 0 }T
T{ S" " fid1 @ WRITE-LINE -> 0 }T
T{ fp 2@ fid1 @ REPOSITION-FILE -> 0 }T
T{ rl1 -> 0 <TRUE> 0 }T
T{ rl1 -> 0 <TRUE> 0 }T
T{ rl1 -> 0 <FALSE> 0 }T
T{ fid1 @ CLOSE-FILE -> 0 }T

\ F.11.6.1.2147 RESIZE-FILE (from testsuite#test:file:RESIZE-FILE)
\ setpad
T{ fn2 R/W BIN OPEN-FILE SWAP fid2 ! -> 0 }T
T{ 37. fid2 @ RESIZE-FILE -> 0 }T
T{ fid2 @ FILE-SIZE -> 37. 0 }T
T{ 0. fid2 @ REPOSITION-FILE -> 0 }T
T{ cbuf buf 100 fid2 @ READ-FILE -> 37 0 }T
T{ PAD 37 buf 37 COMPARE -> 0 }T
T{ PAD 38 buf 38 COMPARE -> 1 }T
T{ 500. fid2 @ RESIZE-FILE -> 0 }T
T{ fid2 @ FILE-SIZE -> 500. 0 }T
T{ 0. fid2 @ REPOSITION-FILE -> 0 }T
T{ cbuf buf 100 fid2 @ READ-FILE -> 100 0 }T
T{ PAD 37 buf 37 COMPARE -> 0 }T
T{ fid2 @ CLOSE-FILE -> 0 }T

\ F.11.6.1.2165 S" (from testsuite#test:file:Sq)
T{ S" A String"2DROP -> }T
\ There is no space between the " and 2DROP

\ F.11.6.1.2218 SOURCE-ID (from testsuite#test:file:SOURCE-ID)
T{ SOURCE-ID DUP -1 = SWAP 0= OR -> <FALSE> }T

\ F.11.6.1.2485 WRITE-LINE (from testsuite#test:file:WRITE-LINE)
\ : line1 S" Line 1" ;
T{ fn1 W/O OPEN-FILE SWAP fid1 ! -> 0 }T
T{ line1 fid1 @ WRITE-LINE -> 0 }T
T{ fid1 @ CLOSE-FILE -> 0 }T

\ F.11.6.2.2130 RENAME-FILE (from testsuite#test:file:RENAME-FILE)
\ : fn3 S" fatest3.txt" ;
\ : >end fid1 @ FILE-SIZE DROP fid1 @ REPOSITION-FILE ;
T{ fn3 DELETE-FILE DROP -> }T
T{ fn1 fn3 RENAME-FILE -> 0 }T
\ Return value is undefined
T{ fn1 FILE-STATUS SWAP DROP 0= -> <FALSE> }T
T{ fn3 FILE-STATUS SWAP DROP 0= -> <TRUE> }T
T{ fn3 R/W OPEN-FILE SWAP fid1 ! -> 0 }T
T{ >end -> 0 }T
T{ S" Final line" fid1 @ WRITE-LINE -> 0 }T
T{ fid1 @ FLUSH-FILE -> 0 }T \ Can only test FLUSH-FILE doesn't fail
T{ fid1 @ CLOSE-FILE -> 0 }T
\ Tidy the test folder
T{ fn3 DELETE-FILE DROP -> }T

\ F.11.6.2.2144.50 REQUIRED (from testsuite#test:file:REQUIRED)
\ This test requires two additional files:
\ required-helper1.fs and
\ required-helper2.fs.
\ Both of which hold the text:
\ 1+
\ As for the test themselves:
\ T{ 0
\ S" required-helper1.fs" REQUIRED \ Increment TOS
\ REQUIRE required-helper1.fs \ Ignore - already loaded
\ INCLUDE required-helper1.fs \ Increment TOS
-> 2 }T
\ T{ 0
\ INCLUDE required-helper2.fs \ Increment TOS
\ S" required-helper2.fs" REQUIRED \ Ignored - already loaded
\ REQUIRE required-helper2.fs \ Ignored - already loaded
\ S" required-helper2.fs" INCLUDED \ Increment TOS
-> 2 }T
