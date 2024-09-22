sub rsp,0x48
mov rcx,0x{0:X}            ;SpecialEffects
mov edx,{1}                ;ID
call 0x{2:X}               ;RemoveSpEffectFromPlayer
add rsp,0x48
ret