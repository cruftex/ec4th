\ Generated from forth-standard.org
\ Word: ekey>fkey
\ Slug: op-ekey-to-fkey
\ Wordset: facility-ext
\ Source: facility/EKEYtoFKEY

\ T.10.6.2.---- EKEY>FKEY
\ : TFKEY" ( "ccc<quote>" -- u flag )
\ CR ." Please press " POSTPONE ." EKEY EKEY>FKEY ;
T{ TFKEY" <left>" -> K-LEFT <TRUE> }T
T{ TFKEY" <right>" -> K-RIGHT <TRUE> }T
T{ TFKEY" <up>" -> K-UP <TRUE> }T
T{ TFKEY" <down>" -> K-DOWN <TRUE> }T
T{ TFKEY" <home>" -> K-HOME <TRUE> }T
T{ TFKEY" <end>" -> K-END <TRUE> }T
T{ TFKEY" <prior>" -> K-PRIOR <TRUE> }T
T{ TFKEY" <next>" -> K-NEXT <TRUE> }T
T{ TFKEY" <F1>" -> K-F1 <TRUE> }T
T{ TFKEY" <F2>" -> K-F2 <TRUE> }T
T{ TFKEY" <F3>" -> K-F3 <TRUE> }T
T{ TFKEY" <F4>" -> K-F4 <TRUE> }T
T{ TFKEY" <F5>" -> K-F5 <TRUE> }T
T{ TFKEY" <F6>" -> K-F6 <TRUE> }T
T{ TFKEY" <F7>" -> K-F7 <TRUE> }T
T{ TFKEY" <F8>" -> K-F8 <TRUE> }T
T{ TFKEY" <F9>" -> K-F9 <TRUE> }T
T{ TFKEY" <F10>" -> K-F10 <TRUE> }T
T{ TFKEY" <F11>" -> K-F11 <TRUE> }T
T{ TFKEY" <F11>" -> K-F12 <TRUE> }T
T{ TFKEY" <shift-left>" -> K-LEFT K-SHIFT-MASK OR <TRUE> }T
T{ TFKEY" <ctrl-left>" -> K-LEFT K-CTRL-MASK OR <TRUE> }T
T{ TFKEY" <alt-left>" -> K-LEFT K-ALT-MASK OR <TRUE> }T
T{ TFKEY" <a>" SWAP EKEY>CHAR -> <FALSE> CHAR a <TRUE> }T
