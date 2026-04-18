\ Generated from forth-standard.org
\ Word: c"
\ Slug: op-c-quote
\ Wordset: core-ext
\ Source: core/Cq

T{ : cq1 C" 123" ; -> }T
T{ : cq2 C" " ; -> }T
T{ cq1 COUNT EVALUATE -> 123 }T
T{ cq2 COUNT EVALUATE -> }T
