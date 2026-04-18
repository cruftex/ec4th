\ Generated from forth-standard.org
\ Word: d.r
\ Slug: op-d-dot-r
\ Wordset: double
\ Source: double/DDotR

\ MAX-2INT 71 73 M*/ 2CONSTANT dbl1
\ MIN-2INT 73 79 M*/ 2CONSTANT dbl2
\ : d>ascii ( d -- caddr u )
\ DUP >R <# DABS #S R> SIGN #> ( -- caddr1 u )
\ HERE SWAP 2DUP 2>R CHARS DUP ALLOT MOVE 2R>
\ ;
\ dbl1 d>ascii 2CONSTANT "dbl1"
\ dbl2 d>ascii 2CONSTANT "dbl2"
\ : DoubleOutput
\ CR ." You should see lines duplicated:" CR
\ 5 SPACES "dbl1" TYPE CR
\ 5 SPACES dbl1 D. CR
\ 8 SPACES "dbl1" DUP >R TYPE CR
\ 5 SPACES dbl1 R> 3 + D.R CR
\ 5 SPACES "dbl2" TYPE CR
\ 5 SPACES dbl2 D. CR
\ 10 SPACES "dbl2" DUP >R TYPE CR
\ 5 SPACES dbl2 R> 5 + D.R CR
\ ;
T{ DoubleOutput -> }T
