#!/bin/sh

set -e

mkdir -p output

gforth -e "fpath= .|`pwd`" build.fs -e bye 
t=output/ec4th-arduino-nano-regular
# elf not needed, simavr supports hex input also
# avr-objcopy -I binary -O elf32-avr --change-section-vma .data=0x0000 --rename-section .data=.text $t.bin $t.elf
# avr-objcopy -O ihex -R .eeprom $t.elf $t.hex
# avr-objcopy -I binary -O ihex --change-section-vma .data=0x0000 --rename-section .data=.text $t.bin $t.hex2

avr-objcopy -I binary -O ihex $t.bin $t.hex

# simavr -m atmega328p -f 16000000 $t.elf
