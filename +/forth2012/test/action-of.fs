\ Generated from forth-standard.org
\ Word: action-of
\ Slug: action-of
\ Wordset: core-ext
\ Source: core/ACTION-OF

\ T.6.2.---- ACTION-OF
T{ DEFER defer1 -> }T
T{ : action-defer1 ACTION-OF defer1 ; -> }T
T{ ' * ' defer1 DEFER! -> }T
T{ 2 3 defer1 -> 6 }T
T{ ACTION-OF defer1 -> ' * }T
T{ action-defer1 -> ' * }T
T{ ' + IS defer1 -> }T
T{ 1 2 defer1 -> 3 }T
T{ ACTION-OF defer1 -> ' + }T
T{ action-defer1 -> ' + }T
