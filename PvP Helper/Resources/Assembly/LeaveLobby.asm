sub rsp,0x48
mov rcx,{0:X}            ;Session Pointer maybe
call 0x{1:X}               ;Call to session
add rsp,0x48
ret