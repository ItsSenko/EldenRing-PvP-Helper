sub rsp,48     
mov rcx,0x{0:X}   ;PlayerGameData
mov edx,0x{1:X}       ;AmountToAdd
call 0x{2:X}        ;AddSoul_Call
add rsp,48
ret