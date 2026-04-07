\ Register allocation and helpers jaw

Decimal

start-macros

\ include constants.fs

\ Aliases
' r0 alias mul0 \ result of mul operation
' r1 alias mul1 \ result of mul operation
' r2 alias zero \ always zero, note: GCC ABI has r1 as zero

\ since we have registers to spare, use it for the timer counter
' r4 alias ticks0 \ timer ticks every 1024 microseconds
' r5 alias ticks1
' r6 alias ticks2
' r7 alias millis0 \ millisecond timer
' r8 alias millis1
' r9 alias millis2
' r10 alias millis3
' r11 alias millisfraction
' r12 alias buffer-status
' r13 alias mempivot
' r14 alias temp4
' r15 alias temp5
\ Registers above 16 can be used for cpi
' r16 alias temp0
' r17 alias temp1
' r18 alias temp2
' r19 alias temp3
' r20 alias read-offset
' r21 alias write-offset
' r22 alias WL
' r23 alias WH
' r24 alias TOSL
' r25 alias TOSH

\ Indirect load registers allocation

\ TODO: make use of ldx / XL / XH
\ options:
\ put W in it
\ put TOS in it and do @ / c@ with ldx

\ X can read data memory indirectly
' XL alias IPL \ r26
' XH alias IPH \ r27
\ Y can read data memory indirectly
\ YL r28 data stack pointer
\ YH r29 data stack pointer
\ Z can read program memory or data memory 
\ ZL r30 indirect read/write general purpose
\ ZH r31 indirect read/write general purpose

\ SPL SPH  (stack pointer) is memory mapped and not addressable as register

end-macros
\ end macros so we can read the environment variable for the target

e? stack-grows-upwards [IF]

start-macros

also cross 
return-stack borders drop Constant data-stack-init
previous

true Constant stack-grows-upwards

\ saves the value of stack to the address pointed by Y
: savetos
  tosl stY+, tosh stY+,  ;

: savetemp
	temp0 stY+, temp1 stY+, ;

\ for rot
: savetemp2
	temp2 stY+, temp3 stY+, ;

\ for um*
: savetemp4
	temp4 stY+, temp5 stY+, ;

\ loads the value at address pointed by Y to stack
: loadtos
  tosh ld-Y, tosl ld-Y,  ;

: loadtemp
  temp1 ld-Y, temp0 ld-Y,  ;

[ELSE]

start-macros

also cross 
data-stack borders nip Constant data-stack-init
previous

false Constant stack-grows-upwards

\ saves the value of stack to the address pointed by Y
: savetos
  tosh st-Y, tosl st-Y, ;

: savetemp
	temp1 st-Y, temp0 st-Y, ;

\ loads the value at address pointed by Y to stack
: loadtos
  tosl ldY+, tosh ldY+, ;

: loadtemp
  temp0 ldY+, temp1 ldY+, ;

\ for um*
: savetemp4
	temp5 st-y, temp4 st-y, ;

[THEN]

: end-code+
  savebranch0 end-code ;

: end-label+
  savebranch0 end-label	 ;

also cross 
dictionary extent drop Constant rom-start
ram-dictionary extent drop Constant ram-start
return-stack borders nip 1- Constant return-stack-init

previous 

ram-start $1ff and $100 <> [IF] 
  ." ram dictionary must start at $x100" 1 throw
[THEN]

\ Helpers for choosing either rom (program memory) or ram (data memory)
\ depending on the memory address. If rom address is not 0, it is expected
\ the ram is before rom. In this case the normal ram starts at 100.

: zh-mask-rom-address,
\ mask rom address if rom is not starting with 0
  rom-start IF ZH $7f andi, THEN ;

: tosh-pm-forth,
  rom-start IF tosh rom-start 8 rshift ori, THEN ;

: mempivot-init,
\ depending which one is on top, we use it as pivot
\ and initialise the pivot register
  rom-start IF
    rom-start highbyte 1-
  ELSE
    ram-start highbyte
  THEN
  temp0 swap ldi,
	mempivot temp0 mov, ;

: ?rom-address-cp,
\ carry is set if it is rom / program memory
  \ cp: rd rr c = rr > rd
  rom-start IF
    mempivot ZH cp,
  ELSE
    ZH mempivot cp,
  THEN ;

: addr>pm ( addr -- addr )
\ translate the dictionary address to device pm address
\ when its not starting at 0
  rom-start - ;

: addr>data ( addr -- addr )
\ translate the dictionary address to device pm address
\ when its not starting at 0. SRAM is starting at $100, however
\ the data area on the AVR starts 0. So we need to keep the $100 offset
  ram-start - $100 + ;

: pout
  drop exit 
 	temp1 swap ldi,
  [ also cross ] there [ previous ]
	temp0 UCSR0A in/lds,
  temp0 5 sbrs, 
  rjmp, \ wait for empty transmit buffer
	temp1 UDR0 out/sts, \ put data (temp1) into buffer sends the data
;

also cross 


: jmp! ( addr addr -- )
  %1001010000001100 over X ! 
  swap addr>pm 2/ swap X cell+ X ! ;

previous

end-macros
