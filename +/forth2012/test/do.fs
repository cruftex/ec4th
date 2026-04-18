\ Generated from forth-standard.org
\ Word: do
\ Slug: do
\ Wordset: core
\ Source: core/DO

\ T.6.1.1800 LOOP (from loop.fs)
T{ : GD1 DO I LOOP ; -> }T
T{ 4 1 GD1 -> 1 2 3 }T
T{ 2 -1 GD1 -> -1 0 1 }T
T{ MID-UINT+1 MID-UINT GD1 -> MID-UINT }T

\ T.6.1.0140 +LOOP (from op-plus-loop.fs)
T{ : GD2 DO I -1 +LOOP ; -> }T
T{ 1 4 GD2 -> 4 3 2 1 }T
T{ -1 2 GD2 -> 2 1 0 -1 }T
T{ MID-UINT MID-UINT+1 GD2 -> MID-UINT+1 MID-UINT }T
\ VARIABLE gditerations
\ VARIABLE gdincrement
\ : gd7 ( limit start increment -- )
\ gdincrement !
\ 0 gditerations !
\ DO
\ 1 gditerations +!
\ I
\ gditerations @ 6 = IF LEAVE THEN
\ gdincrement @
\ +LOOP gditerations @
\ ;
T{ 4 4 -1 gd7 -> 4 1 }T
T{ 1 4 -1 gd7 -> 4 3 2 1 4 }T
T{ 4 1 -1 gd7 -> 1 0 -1 -2 -3 -4 6 }T
T{ 4 1 0 gd7 -> 1 1 1 1 1 1 6 }T
T{ 0 0 0 gd7 -> 0 0 0 0 0 0 6 }T
T{ 1 4 0 gd7 -> 4 4 4 4 4 4 6 }T
T{ 1 4 1 gd7 -> 4 5 6 7 8 9 6 }T
T{ 4 1 1 gd7 -> 1 2 3 3 }T
T{ 4 4 1 gd7 -> 4 5 6 7 8 9 6 }T
T{ 2 -1 -1 gd7 -> -1 -2 -3 -4 -5 -6 6 }T
T{ -1 2 -1 gd7 -> 2 1 0 -1 4 }T
T{ 2 -1 0 gd7 -> -1 -1 -1 -1 -1 -1 6 }T
T{ -1 2 0 gd7 -> 2 2 2 2 2 2 6 }T
T{ -1 2 1 gd7 -> 2 3 4 5 6 7 6 }T
T{ 2 -1 1 gd7 -> -1 0 1 3 }T
T{ -20 30 -10 gd7 -> 30 20 10 0 -10 -20 6 }T
T{ -20 31 -10 gd7 -> 31 21 11 1 -9 -19 6 }T
T{ -20 29 -10 gd7 -> 29 19 9 -1 -11 5 }T
\ With large and small increments
\ MAX-UINT 8 RSHIFT 1+ CONSTANT ustep
\ ustep NEGATE CONSTANT -ustep
\ MAX-INT 7 RSHIFT 1+ CONSTANT step
\ step NEGATE CONSTANT -step
\ VARIABLE bump
T{ : gd8 bump ! DO 1+ bump @ +LOOP ; -> }T
T{ 0 MAX-UINT 0 ustep gd8 -> 256 }T
T{ 0 0 MAX-UINT -ustep gd8 -> 256 }T
T{ 0 MAX-INT MIN-INT step gd8 -> 256 }T
T{ 0 MIN-INT MAX-INT -step gd8 -> 256 }T

\ T.6.1.1730 J (from j.fs)
T{ : GD3 DO 1 0 DO J LOOP LOOP ; -> }T
T{ 4 1 GD3 -> 1 2 3 }T
T{ 2 -1 GD3 -> -1 0 1 }T
T{ MID-UINT+1 MID-UINT GD3 -> MID-UINT }T
T{ : GD4 DO 1 0 DO J LOOP -1 +LOOP ; -> }T
T{ 1 4 GD4 -> 4 3 2 1 }T
T{ -1 2 GD4 -> 2 1 0 -1 }T
T{ MID-UINT MID-UINT+1 GD4 -> MID-UINT+1 MID-UINT }T

\ T.6.1.1760 LEAVE (from leave.fs)
\ T{ : GD5 123 SWAP 0 DO
\ I 4 > IF DROP 234 LEAVE THEN
LOOP ; -> }T
T{ 1 GD5 -> 123 }T
T{ 5 GD5 -> 123 }T
T{ 6 GD5 -> 234 }T

\ T.6.1.2380 UNLOOP (from unloop.fs)
\ T{ : GD6 ( PAT: {0 0},{0 0}{1 0}{1 1},{0 0}{1 0}{1 1}{2 0}{2 1}{2 2} )
\ 0 SWAP 0 DO
\ I 1+ 0 DO
\ I J + 3 = IF
\ I UNLOOP I UNLOOP EXIT THEN 1+
\ LOOP
LOOP ; -> }T
T{ 1 GD6 -> 1 }T
T{ 2 GD6 -> 3 }T
T{ 3 GD6 -> 4 1 2 }T
