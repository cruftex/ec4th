\ Generated from forth-standard.org
\ Word: allocate
\ Slug: allocate
\ Wordset: memory
\ Source: memory/ALLOCATE

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
