sub rsp,0x48
lea rcx,[rsp+0x28]
mov rdx,{0}
mov r8,{1}
call 0x{2:X}
add rsp,0x48
ret