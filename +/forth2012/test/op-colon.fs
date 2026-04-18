\ Generated from forth-standard.org
\ Word: :
\ Slug: op-colon
\ Wordset: core
\ Source: core/Colon

\ T.6.1.0450 : (from op-colon.fs)
T{ : NOP : POSTPONE ; ; -> }T
T{ NOP NOP1 NOP NOP2 -> }T
T{ NOP1 -> }T
T{ NOP2 -> }T
\ The following tests the dictionary search order:
T{ : GDX 123 ; : GDX GDX 234 ; -> }T
T{ GDX -> 123 234 }T
