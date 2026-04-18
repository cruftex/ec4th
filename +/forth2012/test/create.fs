\ Generated from forth-standard.org
\ Word: create
\ Slug: create
\ Wordset: core
\ Source: core/CREATE

\ T.6.1.0550 >BODY (from op-to-body.fs)
T{ CREATE CR0 -> }T
T{ ' CR0 >BODY -> HERE }T

\ T.6.1.1250 DOES> (from op-does.fs)
T{ : DOES1 DOES> @ 1 + ; -> }T
T{ : DOES2 DOES> @ 2 + ; -> }T
T{ CREATE CR1 -> }T
T{ CR1 -> HERE }T
T{ 1 , -> }T
T{ CR1 @ -> 1 }T
T{ DOES1 -> }T
T{ CR1 -> 2 }T
T{ DOES2 -> }T
T{ CR1 -> 3 }T
T{ : WEIRD: CREATE DOES> 1 + DOES> 2 + ; -> }T
T{ WEIRD: W1 -> }T
T{ ' W1 >BODY -> HERE }T
T{ W1 -> HERE 1 + }T
T{ W1 -> HERE 2 + }T
