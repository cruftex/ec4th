\ Generated from forth-standard.org testsuite
\ Wordset: string

\ F.17.6.1.0170 -TRAILING (from testsuite#test:string:-TRAILING)
T{ : s8 S" abc " ; -> }T
T{ : s9 S" " ; -> }T
T{ : s10 S" a " ; -> }T
T{ s1 -TRAILING -> s1 }T \ "abcdefghijklmnopqrstuvwxyz"
T{ s8 -TRAILING -> s8 2 - }T \ "abc "
T{ s7 -TRAILING -> s7 }T \ " "
T{ s9 -TRAILING -> s9 DROP 0 }T \ " "
T{ s10 -TRAILING -> s10 1- }T \ " a "

\ F.17.6.1.0245 /STRING (from testsuite#test:string:/STRING)
T{ s1 5 /STRING -> s1 SWAP 5 + SWAP 5 - }T
T{ s1 10 /STRING -4 /STRING -> s1 6 /STRING }T
T{ s1 0 /STRING -> s1 }T

\ F.17.6.1.0780 BLANK (from testsuite#test:string:BLANK)
\ : s13 S" aaaaa a" ; \ Six spaces
T{ PAD 25 CHAR a FILL -> }T \ Fill PAD with 25 'a's
T{ PAD 5 CHARS + 6 BLANK -> }T \ Put 6 spaced from character 5
T{ PAD 12 s13 COMPARE -> 0 }T \ PAD Should now be same as s13

\ F.17.6.1.0935 COMPARE (from testsuite#test:string:COMPARE)
T{ s1 s1 COMPARE -> 0 }T
T{ s1 PAD SWAP CMOVE -> }T \ Copy s1 to PAD
T{ s1 PAD OVER COMPARE -> 0 }T
T{ s1 PAD 6 COMPARE -> 1 }T
T{ PAD 10 s1 COMPARE -> -1 }T
T{ s1 PAD 0 COMPARE -> 1 }T
T{ PAD 0 s1 COMPARE -> -1 }T
T{ s1 s6 COMPARE -> 1 }T
T{ s6 s1 COMPARE -> -1 }T
\ : "abdde" S" abdde" ;
\ : "abbde" S" abbde" ;
\ : "abcdf" S" abcdf" ;
\ : "abcdee" S" abcdee" ;
T{ s1 "abdde" COMPARE -> -1 }T
T{ s1 "abbde" COMPARE -> 1 }T
T{ s1 "abcdf" COMPARE -> -1 }T
T{ s1 "abcdee" COMPARE -> 1 }T
\ : s11 S" 0abc" ;
\ : s12 S" 0aBc" ;
T{ s11 s12 COMPARE -> 1 }T
T{ s12 s11 COMPARE -> -1 }T

\ F.17.6.1.2191 SEARCH (from testsuite#test:string:SEARCH)
T{ : s2 S" abc" ; -> }T
T{ : s3 S" jklmn" ; -> }T
T{ : s4 S" z" ; -> }T
T{ : s5 S" mnoq" ; -> }T
T{ : s6 S" 12345" ; -> }T
T{ : s7 S" " ; -> }T
T{ s1 s2 SEARCH -> s1 <TRUE> }T
T{ s1 s3 SEARCH -> s1 9 /STRING <TRUE> }T
T{ s1 s4 SEARCH -> s1 25 /STRING <TRUE> }T
T{ s1 s5 SEARCH -> s1 <FALSE> }T
T{ s1 s6 SEARCH -> s1 <FALSE> }T
T{ s1 s7 SEARCH -> s1 <TRUE> }T

\ F.17.6.1.2212 SLITERAL (from testsuite#test:string:SLITERAL)
T{ : s14 [ s1 ] SLITERAL ; -> }T
T{ s1 s14 COMPARE -> 0 }T
T{ s1 s14 ROT = ROT ROT = -> <TRUE> <FALSE> }T

\ F.17.6.2.2255 SUBSTITUTE (from testsuite#test:string:SUBSTITUTE)
\ 30 CHARS BUFFER: subbuff \ Destination buffer
\ Define a few string constants
\ : "hi" S" hi" ;
\ : "wld" S" wld" ;
\ : "hello" S" hello" ;
\ : "world" S" world" ;
\ Define a few test strings
\ : sub1 S" Start: %hi%,%wld%! :End" ; \ Original string
\ : sub2 S" Start: hello,world! :End" ; \ First target string
\ : sub3 S" Start: world,hello! :End" ; \ Second target string
\ Define the hi and wld substitutions
T{ "hello" "hi" REPLACES -> }T \ Replace "%hi%" with "hello"
T{ "world" "wld" REPLACES -> }T \ Replace "%wld%" with "world"
\ "%hi%,%wld%" changed to "hello,world"
T{ sub1 subbuff 30 SUBSTITUTE ROT ROT sub2 COMPARE -> 2 0 }T
\ Change the hi and wld substitutions
T{ "world" "hi" REPLACES -> }T
T{ "hello" "wld" REPLACES -> }T
\ Now "%hi%,%wld%" should be changed to "world,hello"
T{ sub1 subbuff 30 SUBSTITUTE ROT ROT sub3 COMPARE -> 2 0 }T
\ Where the subsitution name is not defined
\ : sub4 S" aaa%bbb%ccc" ;
T{ sub4 subbuff 30 SUBSTITUTE ROT ROT sub4 COMPARE -> 0 0 }T
\ Finally the % character itself
\ : sub5 S" aaa%%bbb" ;
\ : sub6 S" aaa%bbb" ;
T{ sub5 subbuff 30 SUBSTITUTE ROT ROT sub6 COMPARE -> 0 0 }T

\ F.17.6.2.2375 UNESCAPE (from testsuite#test:string:UNESCAPE)
\ Using subbuff, sub5 and sub6 from F.17.6.2.2255 SUBSTITUTE.
T{ sub6 subbuff UNESCAPE sub5 COMPARE -> 0 }T
\ F.21 The optional Extended Character word set
\ T.18
\ These test assume the UTF-8 character encoding is being used.
