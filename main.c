#define COMMON_IMPLEMENTATION
#include "common.h"
#include <stdlib.h>
#include <stdio.h>
#include <math.h>


int main(int argc, char **argv) {
    InitWindow(300, 300, "shader-sculpting");

    RenderTexture2D texture = LoadRenderTexture(300, 300);

    Shader shader = LoadShader(NULL, "shader.fs");

    int time_loc          = GetShaderLocation(shader, "time");
    int fog_color_loc     = GetShaderLocation(shader, "fogColor");
    int fog_intensity_loc = GetShaderLocation(shader, "fogIntensity");


    SetShaderValue(shader, fog_color_loc, &rgba_colors[Background].r,
                   SHADER_UNIFORM_VEC4);

    float intensity = 0.2;
    SetShaderValue(shader, fog_intensity_loc, &intensity,
                   SHADER_UNIFORM_FLOAT);

    float time = 0;

    while (!WindowShouldClose()) {
        time += GetFrameTime();

        SetShaderValue(shader, time_loc, &time,
                       SHADER_UNIFORM_FLOAT);

        texture.texture.width  = GetScreenWidth();
        texture.texture.height = GetScreenHeight();

        BeginDrawing();
            ClearBackground(colors[Background]);
            BeginShaderMode(shader);
            DrawTextureEx(texture.texture, (Vector2){ 0.0f, 0.0f }, 0, 1, colors[Red_TTY]);
            EndShaderMode();
        EndDrawing();
    }

    UnloadTexture(texture.texture);
    CloseWindow();
	return 0;
}
