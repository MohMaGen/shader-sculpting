#ifndef _SHADER_SCULPTING_GUARD_COMMON_H_
#define _SHADER_SCULPTING_GUARD_COMMON_H_

#include "raylib.h"


enum color_name {
    Foreground = 0,
    Background,
    Black_TTY,
    Red_TTY,
    Green_TTY,
    Blue_TTY,
    Yellow_TTY,
    Magenta_TTY,
    Cyan_TTY,
    White_TTY,
    Color_Names_Len
};


Color colors[Color_Names_Len] = {
    [Foreground]  = (Color){ 0xd3, 0xc6, 0xaa, 0xff },
    [Background]  = (Color){ 0x27, 0x2e, 0x33, 0xff },
    [Black_TTY]   = (Color){ 0x34, 0x3f, 0x44, 0xff },
    [Red_TTY]     = (Color){ 0xe6, 0x7e, 0x80, 0xff },
    [Green_TTY]   = (Color){ 0xa7, 0xc0, 0x80, 0xff },
    [Blue_TTY]    = (Color){ 0xdb, 0xbc, 0x7f, 0xff },
    [Yellow_TTY]  = (Color){ 0x7f, 0xbb, 0xb3, 0xff },
    [Magenta_TTY] = (Color){ 0xd6, 0x99, 0xb6, 0xff },
    [Cyan_TTY]    = (Color){ 0x83, 0xc0, 0x92, 0xff },
    [White_TTY]   = (Color){ 0x85, 0x92, 0x89, 0xff },
}; 



#endif


#ifdef COMMON_IMPLEMENTATION



#endif
