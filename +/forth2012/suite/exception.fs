\ Generated from forth-standard.org testsuite
\ Wordset: exception

\ F.9.6.1.2275 THROW (from testsuite#test:exception:THROW)
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

\ F.9.6.2.0680 ABORT" (from testsuite#test:exception:ABORTq)
\ DECIMAL
\ -1 CONSTANT exc_abort
\ -2 CONSTANT exc_abort"
\ -13 CONSTANT exc_undef
\ : t6 ABORT ;
\ The 77 in t10 is necessary for the second
\ ABORT" test as the data stack is restored to a
\ depth of 2 when THROW is executed. The 77 ensures
\ the top of stack value is known for the results check.
\ : t10 77 SWAP ABORT" This should not be displayed" ;
\ : c6 CATCH
\ CASE exc_abort OF 11 ENDOF
\ exc_abort" OF 12 ENDOF
\ exc_undef OF 13 ENDOF
\ ENDCASE
\ ;
T{ 1 2 ' t6 c6 -> 1 2 11 }T \ Test that ABORT is caught
T{ 3 0 ' t10 c6 -> 3 77 }T \ ABORT" does nothing
T{ 4 5 ' t10 c6 -> 4 77 12 }T \ ABORT" caught, no message
