#define COMMON_IMPLEMENTATION
#include "common.h"


int main(int argc, char **argv) {
    InitWindow("shader-sculpting");


    while (!WindowShouldClose()) {
        BeginDrawing();
        ClearBackground();
        EndDrawing();
    }
    CloseWindwo();

	return 0;
}
