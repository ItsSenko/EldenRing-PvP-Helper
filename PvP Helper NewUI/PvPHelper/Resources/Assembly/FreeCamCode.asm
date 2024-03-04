newmem:
push r15
mov r15,[0x{0:X}] ;GameMan

mov eax,[rbx+0x000000C8]
cmp eax,01
je originalcode
jmp exit

originalcode:
mov byte ptr [r15+0x00000B94],01
pop r15
jmp 0x{1:X} ;returnhere

exit:
mov byte ptr [r15+0x00000B94],00
pop r15
jmp 0x{2:X} ;returnhere