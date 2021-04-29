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
call __enterGraphics13__
mov al, 50h
mov bx, 36h
mov cx, 0h
mov dx, 0h
call draw_square
call __readMouse__
jmp $
include 'drawsquare.txt'
include 'graphics.txt'
include 'printf.txt'
include 'mouse.txt'


times 8192 - ($-$$) db 0
