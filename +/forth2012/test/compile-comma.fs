\ Generated from forth-standard.org
\ Word: compile,
\ Slug: compile-comma
\ Wordset: core-ext
\ Source: core/COMPILEComma

\ :NONAME DUP + ; CONSTANT dup+
T{ : q dup+ COMPILE, ; -> }T
T{ : as [ q ] ; -> }T
T{ 123 as -> 246 }T
