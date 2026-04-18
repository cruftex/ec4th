\ Generated from forth-standard.org testsuite
\ Wordset: core

\ F.6.1.0080 ( (from testsuite#test:core:p)
\ There is no space either side of the ).
T{ ( A comment)1234 -> }T
T{ : pc1 ( A comment)1234 ; pc1 -> 1234 }T

\ F.6.1.0190 ." (from testsuite#test:core:.q)
T{ : pb1 CR ." You should see 2345: "." 2345"; pb1 -> }T
\ See F.6.1.1320 EMIT.

\ F.6.1.0250 0&lt; (from testsuite#test:core:0less)
T{ 0 0< -> <FALSE> }T
T{ -1 0< -> <TRUE> }T
T{ MIN-INT 0< -> <TRUE> }T
T{ 1 0< -> <FALSE> }T
T{ MAX-INT 0< -> <FALSE> }T

\ F.6.1.0480 &lt; (from testsuite#test:core:less)
T{ 0 1 < -> <TRUE> }T
T{ 1 2 < -> <TRUE> }T
T{ -1 0 < -> <TRUE> }T
T{ -1 1 < -> <TRUE> }T
T{ MIN-INT 0 < -> <TRUE> }T
T{ MIN-INT MAX-INT < -> <TRUE> }T
T{ 0 MAX-INT < -> <TRUE> }T
T{ 0 0 < -> <FALSE> }T
T{ 1 1 < -> <FALSE> }T
T{ 1 0 < -> <FALSE> }T
T{ 2 1 < -> <FALSE> }T
T{ 0 -1 < -> <FALSE> }T
T{ 1 -1 < -> <FALSE> }T
T{ 0 MIN-INT < -> <FALSE> }T
T{ MAX-INT MIN-INT < -> <FALSE> }T
T{ MAX-INT 0 < -> <FALSE> }T

\ F.6.1.0540 &gt; (from testsuite#test:core:more)
T{ 0 1 > -> <FALSE> }T
T{ 1 2 > -> <FALSE> }T
T{ -1 0 > -> <FALSE> }T
T{ -1 1 > -> <FALSE> }T
T{ MIN-INT 0 > -> <FALSE> }T
T{ MIN-INT MAX-INT > -> <FALSE> }T
T{ 0 MAX-INT > -> <FALSE> }T
T{ 0 0 > -> <FALSE> }T
T{ 1 1 > -> <FALSE> }T
T{ 1 0 > -> <TRUE> }T
T{ 2 1 > -> <TRUE> }T
T{ 0 -1 > -> <TRUE> }T
T{ 1 -1 > -> <TRUE> }T
T{ 0 MIN-INT > -> <TRUE> }T
T{ MAX-INT MIN-INT > -> <TRUE> }T
T{ MAX-INT 0 > -> <TRUE> }T

\ F.6.1.0550 &gt;BODY (from testsuite#test:core:toBODY)
T{ CREATE CR0 -> }T
T{ ' CR0 >BODY -> HERE }T

\ F.6.1.0560 &gt;IN (from testsuite#test:core:toIN)
\ VARIABLE SCANS
\ : RESCAN? -1 SCANS +! SCANS @ IF 0 >IN ! THEN ;
\ T{ 2 SCANS !
\ 345 RESCAN?
-> 345 345 }T
\ : GS2 5 SCANS ! S" 123 RESCAN?" EVALUATE ;
T{ GS2 -> 123 123 123 123 123 }T
\ These tests must start on a new line
\ DECIMAL
\ T{ 123456 DEPTH OVER 9 < 35 AND + 3 + >IN !
-> 123456 23456 3456 456 56 6 }T
\ T{ 14145 8115 ?DUP 0= 34 AND >IN +! TUCK MOD 14 >IN ! GCD calculation
-> 15 }T

\ F.6.1.0570 &gt;NUMBER (from testsuite#test:core:toNUMBER)
\ CREATE GN-BUF 0 C,
\ : GN-STRING GN-BUF 1 ;
\ : GN-CONSUMED GN-BUF CHAR+ 0 ;
\ : GN' [CHAR] ' WORD CHAR+ C@ GN-BUF C! GN-STRING ;
T{ 0 0 GN' 0' >NUMBER -> 0 0 GN-CONSUMED }T
T{ 0 0 GN' 1' >NUMBER -> 1 0 GN-CONSUMED }T
T{ 1 0 GN' 1' >NUMBER -> BASE @ 1+ 0 GN-CONSUMED }T
\ FOLLOWING SHOULD FAIL TO CONVERT
T{ 0 0 GN' -' >NUMBER -> 0 0 GN-STRING }T
T{ 0 0 GN' +' >NUMBER -> 0 0 GN-STRING }T
T{ 0 0 GN' .' >NUMBER -> 0 0 GN-STRING }T
\ : >NUMBER-BASED
\ BASE @ >R BASE ! >NUMBER R> BASE ! ;
T{ 0 0 GN' 2' 10 >NUMBER-BASED -> 2 0 GN-CONSUMED }T
T{ 0 0 GN' 2' 2 >NUMBER-BASED -> 0 0 GN-STRING }T
T{ 0 0 GN' F' 10 >NUMBER-BASED -> F 0 GN-CONSUMED }T
T{ 0 0 GN' G' 10 >NUMBER-BASED -> 0 0 GN-STRING }T
T{ 0 0 GN' G' MAX-BASE >NUMBER-BASED -> 10 0 GN-CONSUMED }T
T{ 0 0 GN' Z' MAX-BASE >NUMBER-BASED -> 23 0 GN-CONSUMED }T
\ : GN1 ( UD BASE -- UD' LEN )
\ UD SHOULD EQUAL UD' AND LEN SHOULD BE ZERO.
\ BASE @ >R BASE !
\ <# #S #>
\ 0 0 2SWAP >NUMBER SWAP DROP \ RETURN LENGTH ONLY
\ R> BASE ! ;
T{ 0 0 2 GN1 -> 0 0 0 }T
T{ MAX-UINT 0 2 GN1 -> MAX-UINT 0 0 }T
T{ MAX-UINT DUP 2 GN1 -> MAX-UINT DUP 0 }T
T{ 0 0 MAX-BASE GN1 -> 0 0 0 }T
T{ MAX-UINT 0 MAX-BASE GN1 -> MAX-UINT 0 0 }T
T{ MAX-UINT DUP MAX-BASE GN1 -> MAX-UINT DUP 0 }T

\ F.6.1.0580 &gt;R (from testsuite#test:core:toR)
T{ : GR1 >R R> ; -> }T
T{ : GR2 >R R@ R> DROP ; -> }T
T{ 123 GR1 -> 123 }T
T{ 123 GR2 -> 123 }T
T{ 1S GR1 -> 1S }T ( Return stack holds cells )

\ F.6.1.1250 DOES&gt; (from testsuite#test:core:DOES)
T{ : DOES1 DOES> @ 1 + ; -> }T
T{ : DOES2 DOES> @ 2 + ; -> }T
T{ CREATE CR1 -> }T
T{ CR1 -> HERE }T
T{ 1 , -> }T
T{ CR1 @ -> 1 }T
T{ DOES1 -> }T
T{ CR1 -> 2 }T
T{ DOES2 -> }T
T{ CR1 -> 3 }T
T{ : WEIRD: CREATE DOES> 1 + DOES> 2 + ; -> }T
T{ WEIRD: W1 -> }T
T{ ' W1 >BODY -> HERE }T
T{ W1 -> HERE 1 + }T
T{ W1 -> HERE 2 + }T

\ F.6.1.1710 IMMEDIATE (from testsuite#test:core:IMMEDIATE)
T{ 123 CONSTANT iw1 IMMEDIATE iw1 -> 123 }T
T{ : iw2 iw1 LITERAL ; iw2 -> 123 }T
T{ VARIABLE iw3 IMMEDIATE 234 iw3 ! iw3 @ -> 234 }T
T{ : iw4 iw3 [ @ ] LITERAL ; iw4 -> 234 }T
T{ :NONAME [ 345 ] iw3 [ ! ] ; DROP iw3 @ -> 345 }T
T{ CREATE iw5 456 , IMMEDIATE -> }T
T{ :NONAME iw5 [ @ iw3 ! ] ; DROP iw3 @ -> 456 }T
T{ : iw6 CREATE , IMMEDIATE DOES> @ 1+ ; -> }T
T{ 111 iw6 iw7 iw7 -> 112 }T
T{ : iw8 iw7 LITERAL 1+ ; iw8 -> 113 }T
T{ : iw9 CREATE , DOES> @ 2 + IMMEDIATE ; -> }T
\ : find-iw BL WORD FIND NIP ;
T{ 222 iw9 iw10 find-iw iw10 -> -1 }T \ iw10 is not immediate
T{ iw10 find-iw iw10 -> 224 1 }T \ iw10 becomes immediate
\ See F.6.1.2510 ['],
\ F.6.1.2033 POSTPONE,
\ F.6.1.2250 STATE,
\ F.6.1.2165 S".

\ F.6.1.2165 S" (from testsuite#test:core:Sq)
T{ : GC4 S" XY" ; -> }T
T{ GC4 SWAP DROP -> 2 }T
T{ GC4 DROP DUP C@ SWAP CHAR+ C@ -> 58 59 }T
\ : GC5 S" A String"2DROP ; \ There is no space between the " and 2DROP
T{ GC5 -> }T

\ F.6.1.2170 S&gt;D (from testsuite#test:core:StoD)
T{ 0 S>D -> 0 0 }T
T{ 1 S>D -> 1 0 }T
T{ 2 S>D -> 2 0 }T
T{ -1 S>D -> -1 -1 }T
T{ -2 S>D -> -2 -1 }T
T{ MIN-INT S>D -> MIN-INT -1 }T
T{ MAX-INT S>D -> MAX-INT 0 }T

\ F.6.1.2340 U&lt; (from testsuite#test:core:Uless)
T{ 0 1 U< -> <TRUE> }T
T{ 1 2 U< -> <TRUE> }T
T{ 0 MID-UINT U< -> <TRUE> }T
T{ 0 MAX-UINT U< -> <TRUE> }T
T{ MID-UINT MAX-UINT U< -> <TRUE> }T
T{ 0 0 U< -> <FALSE> }T
T{ 1 1 U< -> <FALSE> }T
T{ 1 0 U< -> <FALSE> }T
T{ 2 1 U< -> <FALSE> }T
T{ MID-UINT 0 U< -> <FALSE> }T
T{ MAX-UINT 0 U< -> <FALSE> }T
T{ MAX-UINT MID-UINT U< -> <FALSE> }T

\ F.6.2.0455 :NONAME (from testsuite#test:core::NONAME)
\ VARIABLE nn1
\ VARIABLE nn2
T{ :NONAME 1234 ; nn1 ! -> }T
T{ :NONAME 9876 ; nn2 ! -> }T
T{ nn1 @ EXECUTE -> 1234 }T
T{ nn2 @ EXECUTE -> 9876 }T

\ F.6.2.0620 ?DO (from testsuite#test:core:qDO)
\ DECIMAL
\ : qd ?DO I LOOP ;
T{ 789 789 qd -> }T
T{ -9876 -9876 qd -> }T
T{ 5 0 qd -> 0 1 2 3 4 }T
\ : qd1 ?DO I 10 +LOOP ;
T{ 50 1 qd1 -> 1 11 21 31 41 }T
T{ 50 0 qd1 -> 0 10 20 30 40 }T
\ : qd2 ?DO I 3 > IF LEAVE ELSE I THEN LOOP ;
T{ 5 -1 qd2 -> -1 0 1 2 3 }T
\ : qd3 ?DO I 1 +LOOP ;
T{ 4 4 qd3 -> }T
T{ 4 1 qd3 -> 1 2 3 }T
T{ 2 -1 qd3 -> -1 0 1 }T
\ : qd4 ?DO I -1 +LOOP ;
T{ 4 4 qd4 -> }T
T{ 1 4 qd4 -> 4 3 2 1 }T
T{ -1 2 qd4 -> 2 1 0 -1 }T
\ : qd5 ?DO I -10 +LOOP ;
T{ 1 50 qd5 -> 50 40 30 20 10 }T
T{ 0 50 qd5 -> 50 40 30 20 10 0 }T
T{ -25 10 qd5 -> 10 0 -10 -20 }T
\ VARIABLE qditerations
\ VARIABLE qdincrement
\ : qd6 ( limit start increment -- )
\ qdincrement !
\ 0 qditerations !
\ ?DO
\ 1 qditerations +!
\ I
\ qditerations @ 6 = IF LEAVE THEN
\ qdincrement @
\ +LOOP qditerations @
\ ;
T{ 4 4 -1 qd6 -> 0 }T
T{ 1 4 -1 qd6 -> 4 3 2 1 4 }T
T{ 4 1 -1 qd6 -> 1 0 -1 -2 -3 -4 6 }T
T{ 4 1 0 qd6 -> 1 1 1 1 1 1 6 }T
T{ 0 0 0 qd6 -> 0 }T
T{ 1 4 0 qd6 -> 4 4 4 4 4 4 6 }T
T{ 1 4 1 qd6 -> 4 5 6 7 8 9 6 }T
T{ 4 1 1 qd6 -> 1 2 3 3 }T
T{ 4 4 1 qd6 -> 0 }T
T{ 2 -1 -1 qd6 -> -1 -2 -3 -4 -5 -6 6 }T
T{ -1 2 -1 qd6 -> 2 1 0 -1 4 }T
T{ 2 -1 0 qd6 -> -1 -1 -1 -1 -1 -1 6 }T
T{ -1 2 0 qd6 -> 2 2 2 2 2 2 6 }T
T{ -1 2 1 qd6 -> 2 3 4 5 6 7 6 }T
T{ 2 -1 1 qd6 -> -1 0 1 3 }T

\ F.6.2.0698 ACTION-OF (from testsuite#test:core:ACTION-OF)
T{ DEFER defer1 -> }T
T{ : action-defer1 ACTION-OF defer1 ; -> }T
T{ ' * ' defer1 DEFER! -> }T
T{ 2 3 defer1 -> 6 }T
T{ ACTION-OF defer1 -> ' * }T
T{ action-defer1 -> ' * }T
T{ ' + IS defer1 -> }T
T{ 1 2 defer1 -> 3 }T
T{ ACTION-OF defer1 -> ' + }T
T{ action-defer1 -> ' + }T

\ F.6.2.0825 BUFFER: (from testsuite#test:core:BUFFER:)
\ DECIMAL
T{ 127 CHARS BUFFER: TBUF1 -> }T
T{ 127 CHARS BUFFER: TBUF2 -> }T
\ Buffer is aligned
T{ TBUF1 ALIGNED -> TBUF1 }T
\ Buffers do not overlap
T{ TBUF2 TBUF1 - ABS 127 CHARS < -> <FALSE> }T
\ Buffer can be written to
\ 1 CHARS CONSTANT /CHAR
\ : TFULL? ( c-addr n char -- flag )
\ TRUE 2SWAP CHARS OVER + SWAP ?DO
\ OVER I C@ = AND
\ /CHAR +LOOP NIP
\ ;
T{ TBUF1 127 CHAR * FILL -> }T
T{ TBUF1 127 CHAR * TFULL? -> <TRUE> }T
T{ TBUF1 127 0 FILL -> }T
T{ TBUF1 127 0 TFULL? -> <TRUE> }T

\ F.6.2.0855 C" (from testsuite#test:core:Cq)
T{ : cq1 C" 123" ; -> }T
T{ : cq2 C" " ; -> }T
T{ cq1 COUNT EVALUATE -> 123 }T
T{ cq2 COUNT EVALUATE -> }T

\ F.6.2.0873 CASE (from testsuite#test:core:CASE)
\ : cs1 CASE 1 OF 111 ENDOF
\ 2 OF 222 ENDOF
\ 3 OF 333 ENDOF
\ >R 999 R>
\ ENDCASE
\ ;
T{ 1 cs1 -> 111 }T
T{ 2 cs1 -> 222 }T
T{ 3 cs1 -> 333 }T
T{ 4 cs1 -> 999 }T
\ : cs2 >R CASE
\ -1 OF CASE R@ 1 OF 100 ENDOF
\ 2 OF 200 ENDOF
\ >R -300 R>
\ ENDCASE
\ ENDOF
\ -2 OF CASE R@ 1 OF -99 ENDOF
\ >R -199 R>
\ ENDCASE
\ ENDOF
\ >R 299 R>
\ ENDCASE R> DROP
\ ;
T{ -1 1 cs2 -> 100 }T
T{ -1 2 cs2 -> 200 }T
T{ -1 3 cs2 -> -300 }T
T{ -2 1 cs2 -> -99 }T
T{ -2 2 cs2 -> -199 }T
T{ 0 2 cs2 -> 299 }T

\ F.6.2.0945 COMPILE, (from testsuite#test:core:COMPILE,)
\ :NONAME DUP + ; CONSTANT dup+
T{ : q dup+ COMPILE, ; -> }T
T{ : as [ q ] ; -> }T
T{ 123 as -> 246 }T

\ F.6.2.1173 DEFER (from testsuite#test:core:DEFER)
T{ DEFER defer2 -> }T
T{ ' * ' defer2 DEFER! -> }T
T{ 2 3 defer2 -> 6 }T
T{ ' + IS defer2 -> }T
T{ 1 2 defer2 -> 3 }T

\ F.6.2.1175 DEFER! (from testsuite#test:core:DEFER!)
T{ DEFER defer3 -> }T
T{ ' * ' defer3 DEFER! -> }T
T{ 2 3 defer3 -> 6 }T
T{ ' + ' defer3 DEFER! -> }T
T{ 1 2 defer3 -> 3 }T

\ F.6.2.1177 DEFER@ (from testsuite#test:core:DEFER@)
T{ DEFER defer4 -> }T
T{ ' * ' defer4 DEFER! -> }T
T{ 2 3 defer4 -> 6 }T
T{ ' defer4 DEFER@ -> ' * }T
T{ ' + IS defer4 -> }T
T{ 1 2 defer4 -> 3 }T
T{ ' defer4 DEFER@ -> ' + }T

\ F.6.2.1485 FALSE (from testsuite#test:core:FALSE)
T{ FALSE -> 0 }T
T{ FALSE -> <FALSE> }T

\ F.6.2.1675 HOLDS (from testsuite#test:core:HOLDS)
T{ 0. <# S" Test" HOLDS #> S" Test" COMPARE -> 0 }T

\ F.6.2.1725 IS (from testsuite#test:core:IS)
T{ DEFER defer5 -> }T
T{ : is-defer5 IS defer5 ; -> }T
T{ ' * IS defer5 -> }T
T{ 2 3 defer5 -> 6 }T
T{ ' + is-defer5 -> }T
T{ 1 2 defer5 -> 3 }T

\ F.6.2.2020 PARSE-NAME (from testsuite#test:core:PARSE-NAME)
T{ PARSE-NAME abcd S" abcd" S= -> <TRUE> }T
T{ PARSE-NAME abcde S" abcde" S= -> <TRUE> }T
\ test empty parse area
\ T{ PARSE-NAME
NIP -> 0 }T \ empty line
\ T{ PARSE-NAME
NIP -> 0 }T \ line with white space
\ T{ : parse-name-test ( "name1" "name2" -- n )
PARSE-NAME PARSE-NAME S= ; -> }T
T{ parse-name-test abcd abcd -> <TRUE> }T
T{ parse-name-test abcd abcd -> <TRUE> }T
T{ parse-name-test abcde abcdf -> <FALSE> }T
T{ parse-name-test abcdf abcde -> <FALSE> }T
\ T{ parse-name-test abcde abcde
-> <TRUE> }T
\ T{ parse-name-test abcde abcde
-> <TRUE> }T
\ line with white space

\ F.6.2.2182 SAVE-INPUT (from testsuite#test:core:SAVE-INPUT)
\ Testing with a file source
\ VARIABLE siv -1 siv !
\ : NeverExecuted
\ ." This should never be executed" ABORT
\ ;
\ 11111 SAVE-INPUT
\ siv @
\ [IF]
\ 0 siv !
\ RESTORE-INPUT
\ NeverExecuted
\ [ELSE]
\ Testing the ELSE part is executed
\ 22222
\ [THEN]
T{ -> 11111 0 22222 }T \ 0 comes from RESTORE-INPUT
\ Testing with a string source
\ VARIABLE si_inc 0 si_inc !
\ : si1
\ si_inc @ >IN +!
\ 15 si_inc !
\ ;
\ : s$ S" SAVE-INPUT si1 RESTORE-INPUT 12345" ;
T{ s$ EVALUATE si_inc @ -> 0 2345 15 }T
\ Testing nesting
\ : read_a_line
\ REFILL 0=
\ ABORT" REFILL failed"
\ ;
\ 0 si_inc !
\ 2VARIABLE 2res -1. 2res 2!
\ : si2
\ read_a_line
\ read_a_line
\ SAVE-INPUT
\ read_a_line
\ read_a_line
\ s$ EVALUATE 2res 2!
\ RESTORE-INPUT
\ ;
\ WARNING: do not delete or insert lines of
\ text after si2 is called otherwise the next test will
\ fail
\ si2
\ 33333 \ This line should be ignored
\ 2res 2@ 44444 \ RESTORE-INPUT should return to this line
\ 55555
T{ -> 0 0 2345 44444 55555 }T

\ F.6.2.2298 TRUE (from testsuite#test:core:TRUE)
T{ TRUE -> <TRUE> }T
T{ TRUE -> 0 INVERT }T

\ F.6.2.2530 [COMPILE] (from testsuite#test:core:[COMPILE])
\ With default compilation semantics
T{ : [c1] [COMPILE] DUP ; IMMEDIATE -> }T
T{ 123 [c1] -> 123 123 }T
\ With an immediate word
T{ : [c2] [COMPILE] [c1] ; -> }T
T{ 234 [c2] -> 234 234 }T
\ With special compilation semantics
T{ : [cif] [COMPILE] IF ; IMMEDIATE -> }T
T{ : [c3] [cif] 111 ELSE 222 THEN ; -> }T
T{ -1 [c3] -> 111 }T
T{ 0 [c3] -> 222 }T
\ T.7
\ F.8 The optional Double-Number word set
\ T.8
\ Two additional constants are defined to assist tests in this word set:
\ MAX-INT 2/ CONSTANT HI-INT \ 001...1
\ MIN-INT 2/ CONSTANT LO-INT \ 110...1
\ Before anything can be tested, the text interpreter must be
\ tested (F.8.3.2).
\ Once the F.8.6.1.0360 2CONSTANT test has been preformed
\ we can also define a number of double constants:
\ 1S MAX-INT 2CONSTANT MAX-2INT \ 01...1
\ 0 MIN-INT 2CONSTANT MIN-2INT \ 10...0
\ MAX-2INT 2/ 2CONSTANT HI-2INT \ 001...1
\ MIN-2INT 2/ 2CONSTANT LO-2INT \ 110...0
\ The rest of the word set can be tesed:
\ F.8.6.1.1230 DNEGATE,
\ F.8.6.1.1040 D+, F.8.6.1.1050 D-,
\ F.8.6.1.1075 D0<, F.8.6.1.1080 D0=,
\ F.8.6.1.1090 D2*, F.8.6.1.1100 D2/,
\ F.8.6.1.1110 D<, F.8.6.1.1120 D=,
\ F.8.6.1.0390 2LITERAL, F.8.6.1.0440 2VARIABLE,
\ F.8.6.1.1210 DMAX, F.8.6.1.1220 DMIN,
\ F.8.6.1.1140 D>S, F.8.6.1.1160 DABS,
\ F.8.6.1.1830 M+, F.8.6.1.1820 M*/ and
\ F.8.6.1.1070 D.R which also tests D. before moving on
\ to the existion words with the
\ F.8.6.2.0420 2ROT and F.8.6.2.1270 DU< tests.
\ F.8.3.2 Text interpreter input number conversion
T{ 1. -> 1 0 }T
T{ -2. -> -2 -1 }T
T{ : rdl1 3. ; rdl1 -> 3 0 }T
T{ : rdl2 -4. ; rdl2 -> -4 -1 }T
