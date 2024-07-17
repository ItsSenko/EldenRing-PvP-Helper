sub rsp,40
mov rcx,0x{0:X}            ;EquipParamGoodsEntryPtr
mov edx,0x{1:X}            ;EquipParamGoodsRowId
call 0x{2:X}               ;GetEquipParamGoodsEntry
add rsp,40
ret