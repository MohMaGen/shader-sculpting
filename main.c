#define COMMON_IMPLEMENTATION
#include "common.h"
#include <stdlib.h>
#include <stdio.h>
#include <math.h>


int main(int argc, char **argv) {
    InitWindow(800, 800, "shader-sculpting");

    RenderTexture2D texture = LoadRenderTexture(GetScreenWidth(), GetScreenHeight());

    Shader shader = LoadShader(NULL, "shader.fs");

    int time_loc          = GetShaderLocation(shader, "time");
    int colors_loc        = GetShaderLocation(shader, "colors");
    int fog_color_loc     = GetShaderLocation(shader, "fogColor");
    int fog_intensity_loc = GetShaderLocation(shader, "fogIntensity");

    SetShaderValueV(shader, colors_loc, rgba_colors, SHADER_UNIFORM_VEC4, 10);

    SetShaderValue(shader, fog_color_loc, &rgba_colors[Background].r,
                   SHADER_UNIFORM_VEC4);

    float intensity = 0.3;
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
