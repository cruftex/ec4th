\ Generated from forth-standard.org
\ Word: -trailing
\ Slug: op-minus-trailing
\ Wordset: string
\ Source: string/MinusTRAILING

T{ : s8 S" abc " ; -> }T
T{ : s9 S" " ; -> }T
T{ : s10 S" a " ; -> }T
T{ s1 -TRAILING -> s1 }T \ "abcdefghijklmnopqrstuvwxyz"
T{ s8 -TRAILING -> s8 2 - }T \ "abc "
T{ s7 -TRAILING -> s7 }T \ " "
T{ s9 -TRAILING -> s9 DROP 0 }T \ " "
T{ s10 -TRAILING -> s10 1- }T \ " a "
