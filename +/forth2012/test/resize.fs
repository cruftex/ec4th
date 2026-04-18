\ Generated from forth-standard.org
\ Word: resize
\ Slug: resize
\ Wordset: memory
\ Source: memory/RESIZE

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
