\ Generated from forth-standard.org
\ Word: n>r
\ Slug: op-n-greater-r
\ Wordset: tools-ext
\ Source: tools/NtoR

\ : TNR1 N>R SWAP NR> ;
T{ 1 2 10 20 30 3 TNR1 -> 2 1 10 20 30 3 }T
\ : TNR2 N>R N>R SWAP NR> NR> ;
T{ 1 2 10 20 30 3 40 50 2 TNR2 -> 2 1 10 20 30 3 40 50 2 }T
