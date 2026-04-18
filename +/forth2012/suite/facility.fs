\ Generated from forth-standard.org testsuite
\ Wordset: facility

\ F.10.6.2.1306.40 EKEY&gt;FKEY (from testsuite#test:facility:EKEYtoFKEY)
\ : TFKEY" ( "ccc<quote>" -- u flag )
\ CR ." Please press " POSTPONE ." EKEY EKEY>FKEY ;
T{ TFKEY" <left>" -> K-LEFT <TRUE> }T
T{ TFKEY" <right>" -> K-RIGHT <TRUE> }T
T{ TFKEY" <up>" -> K-UP <TRUE> }T
T{ TFKEY" <down>" -> K-DOWN <TRUE> }T
T{ TFKEY" <home>" -> K-HOME <TRUE> }T
T{ TFKEY" <end>" -> K-END <TRUE> }T
T{ TFKEY" <prior>" -> K-PRIOR <TRUE> }T
T{ TFKEY" <next>" -> K-NEXT <TRUE> }T
T{ TFKEY" <F1>" -> K-F1 <TRUE> }T
T{ TFKEY" <F2>" -> K-F2 <TRUE> }T
T{ TFKEY" <F3>" -> K-F3 <TRUE> }T
T{ TFKEY" <F4>" -> K-F4 <TRUE> }T
T{ TFKEY" <F5>" -> K-F5 <TRUE> }T
T{ TFKEY" <F6>" -> K-F6 <TRUE> }T
T{ TFKEY" <F7>" -> K-F7 <TRUE> }T
T{ TFKEY" <F8>" -> K-F8 <TRUE> }T
T{ TFKEY" <F9>" -> K-F9 <TRUE> }T
T{ TFKEY" <F10>" -> K-F10 <TRUE> }T
T{ TFKEY" <F11>" -> K-F11 <TRUE> }T
T{ TFKEY" <F11>" -> K-F12 <TRUE> }T
T{ TFKEY" <shift-left>" -> K-LEFT K-SHIFT-MASK OR <TRUE> }T
T{ TFKEY" <ctrl-left>" -> K-LEFT K-CTRL-MASK OR <TRUE> }T
T{ TFKEY" <alt-left>" -> K-LEFT K-ALT-MASK OR <TRUE> }T
T{ TFKEY" <a>" SWAP EKEY>CHAR -> <FALSE> CHAR a <TRUE> }T
\ T.10
\ F.12 The optional File-Access word set
\ T.11
\ These tests create files in the current directory, if all goes well
\ these will be deleted. If something fails they may not be deleted.
\ If this is a problem ensure you set a suitable directory before
\ running this test. Currently, there is no ANS standard way of doing
\ this. the file names used in these test are:
\ "fatest1.txt", "fatest2.txt" and
\ "fatest3.txt".
\ The test F.11.6.1.1010 CREATE-FILE also tests CLOSE-FILE,
\ F.11.6.1.2485 WRITE-LINE also tests W/O and OPEN-FILE,
\ F.11.6.1.2090 READ-LINE includes a test for R/O,
\ F.11.6.1.2142 REPOSITION-FILE includes tests for R/W,
\ WRITE-FILE, READ-FILE,
\ FILE-POSITION, and S".
\ The F.11.6.1.1522 FILE-SIZE test includes a test for BIN.
\ The test F.11.6.1.2147 RESIZE-FILE should then be run followed by
\ the F.11.6.1.1190 DELETE-FILE test.
\ The F.11.6.1.0080 ( test should be next, followed by
\ F.11.6.1.2218 SOURCE-ID the test which test the extended versions of
\ ( and SOURCE-ID respectively.
\ Finally F.11.6.2.2130 RENAME-FILE tests the extended words
\ RENAME-FILE, FILE-STATUS, and FLUSH-FILE.
