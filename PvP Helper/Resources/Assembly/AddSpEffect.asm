sub rsp,0x48
mov rcx,0x{0:X}            ;Player Instance
mov edx,{1}                ;ID
mov r8d,1				   ;Unk
call 0x{2:X}               ;AddSpEffectToPlayer
add rsp,0x48
ret