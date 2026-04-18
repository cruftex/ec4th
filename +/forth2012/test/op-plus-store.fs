\ Generated from forth-standard.org
\ Word: +!
\ Slug: op-plus-store
\ Wordset: core
\ Source: core/PlusStore

\ T.6.1.0130 +!
T{ 0 1ST ! -> }T
T{ 1 1ST +! -> }T
T{ 1ST @ -> 1 }T
T{ -1 1ST +! 1ST @ -> 0 }T
