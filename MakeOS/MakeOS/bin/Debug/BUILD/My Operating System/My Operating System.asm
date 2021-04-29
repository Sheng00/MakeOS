org 0x7c00
mov ah, 02h
mov al, 10h
mov dl, 0h
mov ch, 0
mov dh, 0
mov cl, 2
mov bx, kernal_load
int 13h
cli
jmp kernal_load
times 510 - ($-$$) db 0
dw 0xAA55

kernal_load:


times 8192 - ($-$$) db 0
