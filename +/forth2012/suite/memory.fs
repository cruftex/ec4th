\ Generated from forth-standard.org testsuite
\ Wordset: memory

\ F.14.6.1.0707 ALLOCATE (from testsuite#test:memory:ALLOCATE)
\ VARIABLE datsp
\ HERE datsp !
T{ 50 CELLS ALLOCATE SWAP addr ! -> 0 }T
T{ addr @ ALIGNED -> addr @ }T \ Test address is aligned
T{ HERE -> datsp @ }T \ Check data space pointer is unaffected
\ addr @ 50 write-cell-mem
\ addr @ 50 check-cell-mem \ Check we can access the heap
T{ addr @ FREE -> 0 }T
T{ 99 ALLOCATE SWAP addr ! -> 0 }T
T{ addr @ ALIGNED -> addr @ }T \ Test address is aligned
T{ addr @ FREE -> 0 }T
T{ HERE -> datsp @ }T \ Data space pointer unaffected by FREE
T{ -1 ALLOCATE SWAP DROP 0= -> <FALSE> }T \ Memory allocate failed

\ F.14.6.1.2145 RESIZE (from testsuite#test:memory:RESIZE)
T{ 50 CHARS ALLOCATE SWAP addr ! -> 0 }T
\ addr @ 50 write-char-mem addr @ 50 check-char-mem
\ Resize smaller does not change content.
T{ addr @ 28 CHARS RESIZE SWAP addr ! -> 0 }T
\ addr @ 28 check-char-mem
\ Resize larger does not change original content.
T{ addr @ 100 CHARS RESIZE SWAP addr ! -> 0 }T
\ addr @ 28 check-char-mem
\ Resize error does not change addr
T{ addr @ -1 RESIZE 0= -> addr @ <FALSE> }T
T{ addr @ FREE -> 0 }T
T{ HERE -> datsp @ }T \ Data space pointer is unaffected
