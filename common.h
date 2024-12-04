#ifndef _SHADER_SCULPTING_GUARD_COMMON_H_
#define _SHADER_SCULPTING_GUARD_COMMON_H_

#include "raylib.h"



struct rgba { float r, g, b, a; };
struct rgba rgba_of_color(Color color);

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
   	Max_White,
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
    [Max_White]   = (Color){ 0xff, 0xff, 0xff, 0xff },
}; 

struct rgba rgba_colors[Color_Names_Len] = {
    (struct rgba){ 0.827451, 0.776471, 0.666667, 1.000000 },
    (struct rgba){ 0.152941, 0.180392, 0.200000, 1.000000 },
    (struct rgba){ 0.203922, 0.247059, 0.266667, 1.000000 },
    (struct rgba){ 0.901961, 0.494118, 0.501961, 1.000000 },
    (struct rgba){ 0.654902, 0.752941, 0.501961, 1.000000 },
    (struct rgba){ 0.858824, 0.737255, 0.498039, 1.000000 },
    (struct rgba){ 0.498039, 0.733333, 0.701961, 1.000000 },
    (struct rgba){ 0.839216, 0.600000, 0.713726, 1.000000 },
    (struct rgba){ 0.513726, 0.752941, 0.572549, 1.000000 },
    (struct rgba){ 0.521569, 0.572549, 0.537255, 1.000000 },
    (struct rgba){ 1.000000, 1.000000, 1.000000, 1.000000 },
};

#endif


#ifdef COMMON_IMPLEMENTATION

    struct rgba rgba_of_color(Color color)
    {
        return (struct rgba) {
            .r = (float)color.r / 255.,
            .g = (float)color.g / 255.,
            .b = (float)color.b / 255.,
            .a = (float)color.a / 255.,
        };
    }


#endif
