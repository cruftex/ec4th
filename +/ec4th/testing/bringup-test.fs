\ bringup test pattern for a new forth implementation


: bringup-test ( 0|error -- )
  \ 4711 dup .h sp@ >r 1 2 3 4 5 r> sp! .h cr
  \ 1 2 3 4 .sh cr sp@ >r 2drop 2drop r> sp! .sh cr
  \ test lit and emit
  '^ emit \ ^
  \ depth (sp@ sp0 @ - cell/) and +
  1234 1234 depth '0 + emit \ 3
  drop depth '0 + emit \ 2
  dup depth '0 + emit \ 3
  = '1 + emit \ 0
  1 drop 1 2 over 1 = '1 + emit \ 0
  2drop depth '0 + emit \ 1
  \ test: 2dup
  'p emit
  1 2 2dup '0 + emit '0 + emit '0 + emit \ 2 1 2 1
  \ test: ?dup
  '? emit \ ?
  0 ?dup depth '0 + emit '0 + emit 
  1 ?dup depth '0 + emit '0 + emit '0 + emit
  't emit
  3 2 1 tuck '0 + emit '0 + emit '0 + emit '0 + emit 
  \ test pick and roll having correct offset
  3 2 1 2 pick '0 + emit
  2 roll '0 + emit '0 + emit '0 + emit
  4 3 2 1 3 roll '0 + emit '0 + emit '0 + emit '0 + emit 
  depth '0 + emit cr ;
