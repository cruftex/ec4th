\ Generated from forth-standard.org
\ Word: throw
\ Slug: throw
\ Wordset: exception
\ Source: exception/THROW

\ tests (from throw.fs)
\ DECIMAL
\ : t1 9 ;
\ : c1 1 2 3 ['] t1 CATCH ;
T{ c1 -> 1 2 3 9 0 }T \ No THROW executed
\ : t2 8 0 THROW ;
\ : c2 1 2 ['] t2 CATCH ;
T{ c2 -> 1 2 8 0 }T \ 0 THROW does nothing
\ : t3 7 8 9 99 THROW ;
\ : c3 1 2 ['] t3 CATCH ;
T{ c3 -> 1 2 99 }T \ Restores stack to CATCH depth
\ : t4 1- DUP 0> IF RECURSE ELSE 999 THROW -222 THEN ;
\ : c4 3 4 5 10 ['] t4 CATCH -111 ;
T{ c4 -> 3 4 5 0 999 -111 }T \ Test return stack unwinding
\ : t5 2DROP 2DROP 9999 THROW ;
\ : c5 1 2 3 4 ['] t5 CATCH \ Test depth restored correctly
\ DEPTH >R DROP 2DROP 2DROP R> ; \ after stack has been emptied
T{ c5 -> 5 }T
