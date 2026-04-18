\ Generated from forth-standard.org
\ Word: fvalue
\ Slug: fvalue
\ Wordset: floating-ext
\ Source: float/FVALUE

T{ 0e0 FVALUE Tval -> }T
T{ Tval -> 0e0 R}T
T{ 1e0 TO Tval -> }T
T{ Tval -> 1e0 R}T
\ : setTval Tval FSWAP TO Tval ;
T{ 2e0 setTval Tval -> 1e0 2e0 RR}T
T{ 5e0 TO Tval -> }T
\ : [execute] EXECUTE ; IMMEDIATE
T{ ' Tval ] [execute] [ -> 2e0 R}T
