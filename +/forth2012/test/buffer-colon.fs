\ Generated from forth-standard.org
\ Word: buffer:
\ Slug: buffer-colon
\ Wordset: core-ext
\ Source: core/BUFFERColon

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
