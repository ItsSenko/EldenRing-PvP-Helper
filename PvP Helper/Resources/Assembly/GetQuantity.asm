sub rsp,40
mov rcx,0x{0:X}            ;ID
call 0x{1:X}               ;GetQuantityCall
add rsp,40
mov rcx,0x{2:X}
mov [rcx], rax
ret