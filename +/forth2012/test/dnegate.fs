\ Generated from forth-standard.org
\ Word: dnegate
\ Slug: dnegate
\ Wordset: double
\ Source: double/DNEGATE

T{ 0. DNEGATE -> 0. }T
T{ 1. DNEGATE -> -1. }T
T{ -1. DNEGATE -> 1. }T
T{ max-2int DNEGATE -> min-2int SWAP 1+ SWAP }T
T{ min-2int SWAP 1+ SWAP DNEGATE -> max-2int }T
