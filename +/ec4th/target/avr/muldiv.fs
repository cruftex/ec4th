\ ################ 

decimal

Code um* ( n1 n2 - dn )
\ clobbers r0 and r1
	loadtemp
	temp2 tosl movw,
    \ temp0:temp1 = A
    \ tosl:tosh = B
	\ temp4:temp5:tosl:tosh = product
    \ A_lo * B_lo
	temp0 temp2 mul,
	temp4 r0 movw,
	tosl clr,
	tosh clr,
    \ A_hi * B_lo
	temp1 temp2 mul,
	temp5 r0 add,
	tosl r1 adc,
	tosh zero adc,
    \ A_lo * B_hi
	temp0 temp3 mul,
	temp5 r0 add,
	tosl r1 adc,
	tosh zero adc,
    \ A_hi * B_hi
	temp1 temp3 mul,
	tosl r0 add,
	tosh r1 adc,
	savetemp4
	do_next rjmp,
End-code+

Start-macros
' tosl Alias dres16uL
' tosh Alias dres16uH
' tosl Alias dd16uL
' tosh Alias dd16uH
' temp0 Alias drem16uL
' temp1 Alias drem16uH
' temp4 Alias dv16uL
' temp5 Alias dv16uH
' temp2 Alias dcnt16u

End-macros

Code u/mod ( n1 n2 -- remainder Quotient) 
\ 16 bit / 16 bit division as per AVR 200 application note
\ ec4th is using u/mod directly for normal number output

\ .def	drem16uL=r14
\ .def	drem16uH=r15
\ .def	dres16uL=r16
\ .def	dres16uH=r17
\ .def	dd16uL	=r16
\ .def	dd16uH	=r17
\ .def	dv16uL	=r18
\ .def	dv16uH	=r19
\ .def	dcnt16u	=r20

	dv16uL tosl movw,
	loadtos				\ in dd16uL
	drem16ul clr,
	drem16uH drem16uH sub,
	dcnt16u 17 ldi,
	2 $:
	dd16uL rol,
	dd16uH rol,
	dcnt16u dec,
	0 $ brne,
	savetemp
	\ for downwards growing: drem16uh st-y, drem16ul st-y,
    do_next rjmp,
	0 $:
	drem16uL rol,
	drem16uH rol,
	drem16uL dv16uL sub,
	drem16uH dv16uH sbc,
	1 $ brcc,
	drem16uL dv16uL add, 
	drem16uH dv16uH adc,
	clc,		\ clear carry to be shifted into result
	2 $ rjmp,
	1 $:
	sec,		\ set carry to be shifted into result
	2 $ rjmp,
End-Code+
