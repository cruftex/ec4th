\ Generated from forth-standard.org
\ Word: defer!
\ Slug: defer-store
\ Wordset: core-ext
\ Source: core/DEFERStore

\ T.6.2.---- DEFER!
T{ DEFER defer3 -> }T
T{ ' * ' defer3 DEFER! -> }T
T{ 2 3 defer3 -> 6 }T
T{ ' + ' defer3 DEFER! -> }T
T{ 1 2 defer3 -> 3 }T
