
#pragma once

#include <cstdint>

#if defined(__GNUC__) || defined(__clang__)
#define EXPORT [[gnu::visibility("default")]]
#else
#define EXPORT
#endif
const static auto Scenes = []() { return 2; }();
extern "C" {

// Creates a new Renderer instance and returns a pointer to it.
EXPORT void *Create_Renderer(uint16_t width, uint16_t height);

// Raytraces the input scene and writes the result to output.
// (You may want to update the signature for your actual API.)
EXPORT void raytrace(const char *input, char *output);
}

#undef EXPORT
