\ +/eckernel/core/io.fs

require strings.fs

\ UNDEF-WORDS
decimal

\ FIXME: remove (emit)
: emit (emit) ;

: (.")
    "lit count type ;

: "lit ( -- addr )
    r> r> dup count + aligned >r swap >r ;

: type ( addr u -- ) \ core
    0 DO
      dup c@ (emit) 1+
    LOOP drop ;

\ Input                                                13feb93py

hex
\ FIXME: remove not needed constants
20 Constant bl
07 constant #bell ( -- c ) \ gforth
08 constant #bs ( -- c ) \ gforth
09 constant #tab ( -- c ) \ gforth
7F constant #del ( -- c ) \ gforth
0D constant #cr   ( -- c ) \ gforth
\ the newline key code
0C constant #ff ( -- c ) \ gforth
0A constant #lf ( -- c ) \ gforth
decimal

: bell  #bell emit [ has? os [IF] ] outfile-id flush-file drop [ [THEN] ] ;

: cr ( -- ) \ core c-r
\G Output a newline (of the favourite kind of the host OS).  Note
\G that due to the way the Forth command line interpreter inserts
\G newlines, the preferred way to use @code{cr} is at the start
\G of a piece of text; e.g., @code{cr ." hello, world"}.
    #cr emit #lf emit ;

: space ( -- ) \ core
\G Display one space.
  bl emit ;

\ : ?  ( addr -- )
\   @ . ;

: spaces ( n1 -- ) \ core
    0 DO space LOOP ;


\ : (ud.) ( ud -- c-addr cnt )
\   <# #s #> ;

\ : ud. ( ud -- )
\   (ud.) type space ;

\ : u. ( u -- ) \ core
\   0 ud. ;

ALL-WORDS 
