\ Generated from forth-standard.org testsuite
\ Wordset: floating

\ F.12.6.2.1489 FATAN2 (from testsuite#test:floating:FATAN2)
\ [UNDEFINED] NaN [IF] 0e 0e F/ FCONSTANT NaN [THEN]
\ [UNDEFINED] +Inf [IF] 1e 0e F/ FCONSTANT +Inf [THEN]
\ [UNDEFINED] -Inf [IF] -1e 0e F/ FCONSTANT -Inf [THEN]
\ TRUE verbose !
\ DECIMAL
\ The test harness default for EXACT? is TRUE.
\ Uncomment the following line if your system needs it to
\ be FALSE
\ SET-NEAR
\ VARIABLE #errors 0 #errors !
\ :NONAME ( c-addr u -- )
\ ( Display an error message followed by the
\ line that had the error@. )
\ 1 #errors +! error1 ; error-xt !
\ [UNDEFINED] pi [IF]
\ 0.3141592653589793238463E1 FCONSTANT pi
\ [THEN]
\ [UNDEFINED] -pi [IF]
\ pi FNEGATE FCONSTANT -pi
\ [THEN]
\ FALSE [IF]
\ 0.7853981633974483096157E0 FCONSTANT pi/4
\ -0.7853981633974483096157E0 FCONSTANT -pi/4
\ 0.1570796326794896619231E1 FCONSTANT pi/2
\ -0.1570796326794896619231E1 FCONSTANT -pi/2
\ 0.4712388980384689857694E1 FCONSTANT 3pi/2
\ 0.2356194490192344928847E1 FCONSTANT 3pi/4
\ -0.2356194490192344928847E1 FCONSTANT -3pi/4
\ [ELSE]
\ pi 4e F/ FCONSTANT pi/4
\ -pi 4e F/ FCONSTANT -pi/4
\ pi 2e F/ FCONSTANT pi/2
\ -pi 2e F/ FCONSTANT -pi/2
\ pi/2 3e F* FCONSTANT 3pi/2
\ pi/4 3e F* FCONSTANT 3pi/4
\ -pi/4 3e F* FCONSTANT -3pi/4
\ [THEN]
\ verbose @ [IF]
\ :NONAME ( -- fp.separate? )
\ DEPTH >R 1e DEPTH R> FDROP 2R> = ; EXECUTE
\ CR .( floating-point and data stacks )
\ [IF] .( *separate* ) [ELSE] .( *not separate* ) [THEN]
\ CR
\ [THEN]
\ TESTING normal values
\ y x rad deg
T{ 0e 1e FATAN2 -> 0e R}T \ 0
T{ 1e 1e FATAN2 -> pi/4 R}T \ 45
T{ 1e 0e FATAN2 -> pi/2 R}T \ 90
T{ -1e -1e FATAN2 -> -3pi/4 R}T \ 135
T{ 0e -1e FATAN2 -> pi R}T \ 180
T{ -1e 1e FATAN2 -> -pi/4 R}T \ 225
T{ -1e 0e FATAN2 -> -pi/2 R}T \ 270
T{ -1e 1e FATAN2 -> -pi/4 R}T \ 315
\ TESTING Single UNIX 3 special values spec
\ ISO C / Single UNIX Specification Version 3:
\ http://www.unix.org/single_unix_specification/
\ Select "Topic", then "Math Interfaces", then "atan2()":
\ http://www.opengroup.org/onlinepubs/009695399/
\ functions/atan2f.html
\ If y is +/-0 and x is < 0, +/-pi shall be returned.
T{ 0e -1e FATAN2 -> pi R}T
T{ -0e -1e FATAN2 -> -pi R}T
\ If y is +/-0 and x is > 0, +/-0 shall be returned.
T{ 0e 1e FATAN2 -> 0e R}T
T{ -0e 1e FATAN2 -> -0e R}T
\ If y is < 0 and x is +/-0, -pi/2 shall be returned.
T{ -1e 0e FATAN2 -> -pi/2 R}T
T{ -1e -0e FATAN2 -> -pi/2 R}T
\ If y is > 0 and x is +/-0, pi/2 shall be returned.
T{ 1e 0e FATAN2 -> pi/2 R}T
T{ 1e -0e FATAN2 -> pi/2 R}T
\ TESTING Single UNIX 3 special values optional spec
\ Optional ISO C / single UNIX specs:
\ If either x or y is NaN, a NaN shall be returned.
T{ NaN 1e FATAN2 -> NaN R}T
T{ 1e NaN FATAN2 -> NaN R}T
T{ NaN NaN FATAN2 -> NaN R}T
\ If y is +/-0 and x is -0, +/-pi shall be returned.
T{ 0e -0e FATAN2 -> pi R}T
T{ -0e -0e FATAN2 -> -pi R}T
\ If y is +/-0 and x is +0, +/-0 shall be returned.
T{ 0e 0e FATAN2 -> +0e R}T
T{ -0e 0e FATAN2 -> -0e R}T
\ For finite values of +/-y > 0, if x is -Inf, +/-pi shall be returned.
T{ 1e -Inf FATAN2 -> pi R}T
T{ -1e -Inf FATAN2 -> -pi R}T
\ For finite values of +/-y > 0, if x is +Inf, +/-0 shall be returned.
T{ 1e +Inf FATAN2 -> +0e R}T
T{ -1e +Inf FATAN2 -> -0e R}T
\ For finite values of x, if y is +/-Inf, +/-pi/2 shall be returned.
T{ +Inf 1e FATAN2 -> pi/2 R}T
T{ +Inf -1e FATAN2 -> pi/2 R}T
T{ +Inf 0e FATAN2 -> pi/2 R}T
T{ +Inf -0e FATAN2 -> pi/2 R}T
T{ -Inf 1e FATAN2 -> -pi/2 R}T
T{ -Inf -1e FATAN2 -> -pi/2 R}T
T{ -Inf 0e FATAN2 -> -pi/2 R}T
T{ -Inf -0e FATAN2 -> -pi/2 R}T
\ If y is +/-Inf and x is -Inf, +/-3pi/4 shall be returned.
T{ +Inf -Inf FATAN2 -> 3pi/4 R}T
T{ -Inf -Inf FATAN2 -> -3pi/4 R}T
\ If y is +/-Inf and x is +Inf, +/-pi/4 shall be returned.
T{ +Inf +Inf FATAN2 -> pi/4 R}T
T{ -Inf +Inf FATAN2 -> -pi/4 R}T
\ verbose @ [IF]
\ CR .( #ERRORS: ) #errors @ . CR
\ [THEN]

\ F.12.6.2.1627 FTRUNC (from testsuite#test:floating:FTRUNC)
\ SET-EXACT
T{ -0E FTRUNC F0= -> <TRUE> }T
T{ -1E-9 FTRUNC F0= -> <TRUE> }T
T{ -0.9E FTRUNC F0= -> <TRUE> }T
T{ -1E 1E-5 F+ FTRUNC F0= -> <TRUE> }T
T{ 0E FTRUNC -> 0E R}T
T{ 1E-9 FTRUNC -> 0E R}T
T{ -1E -1E-5 F+ FTRUNC -> -1E R}T
T{ 3.14E FTRUNC -> 3E R}T
T{ 3.99E FTRUNC -> 3E R}T
T{ 4E FTRUNC -> 4E R}T
T{ -4E FTRUNC -> -4E R}T
T{ -4.1E FTRUNC -> -4E R}T

\ F.12.6.2.1628 FVALUE (from testsuite#test:floating:FVALUE)
T{ 0e0 FVALUE Tval -> }T
T{ Tval -> 0e0 R}T
T{ 1e0 TO Tval -> }T
T{ Tval -> 1e0 R}T
\ : setTval Tval FSWAP TO Tval ;
T{ 2e0 setTval Tval -> 1e0 2e0 RR}T
T{ 5e0 TO Tval -> }T
\ : [execute] EXECUTE ; IMMEDIATE
T{ ' Tval ] [execute] [ -> 2e0 R}T
\ T.12
\ T.13
\ F.16 The optional Memory-Allocation word set
\ T.14
\ These test require a new variable to hold the address of the allocated
\ memory. Two helper words are defined to populate the allocated memory
\ and to check the memory:
\ VARIABLE addr
\ : write-cell-mem ( addr n -- )
\ 1+ 1 DO I OVER ! CELL+ LOOP DROP
\ ;
\ : check-cell-mem ( addr n -- )
\ 1+ 1 DO
\ I SWAP >R >R
T{ R> ( I ) -> R@ ( addr ) @ }T
\ R> CELL+
\ LOOP DROP
\ ;
\ : write-char-mem ( addr n -- )
\ 1+ 1 DO I OVER C! CHAR+ LOOP DROP
\ ;
\ : check-char-mem ( addr n -- )
\ 1+ 1 DO
\ I SWAP >R >R
T{ R> ( I ) -> R@ ( addr ) C@ }T
\ R> CHAR+
\ LOOP DROP
\ ;
\ The test F.14.6.1.0707 ALLOCATE includes a test for FREE.
