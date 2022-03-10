// pch.h: This is a precompiled header file.
// Files listed below are compiled only once, improving build performance for future builds.
// This also affects IntelliSense performance, including code completion and many code browsing features.
// However, files listed here are ALL re-compiled if any one of them is updated between builds.
// Do not add files here that you will be updating frequently as this negates the performance advantage.

#ifndef PCH_H
#define PCH_H

// add headers that you want to pre-compile here
#include "framework.h"

#define STB_IMAGE_IMPLEMENTATION
#include "stb_image.h"

extern "C" __declspec(dllexport) unsigned char* load(char const* filename, int* x, int* y, int* channels_in_file, int desired_channels) {
    return stbi_load(filename, x, y, channels_in_file, desired_channels);
}

extern "C" __declspec(dllexport) unsigned short* load_16(char const* filename, int* x, int* y, int* channels_in_file, int desired_channels) {
    return stbi_load_16(filename, x, y, channels_in_file, desired_channels);
}

extern "C" __declspec(dllexport) float* loadf(char const* filename, int* x, int* y, int* channels_in_file, int desired_channels) {
    return stbi_loadf(filename, x, y, channels_in_file, desired_channels);
}

extern "C" __declspec(dllexport) void image_free(void* retval_from_stbi_load) {
    stbi_image_free(retval_from_stbi_load);
}

extern "C" __declspec(dllexport) void set_flip_vertically_on_load(int flag_true_if_should_flip) {
    stbi_set_flip_vertically_on_load(flag_true_if_should_flip);
}

#endif //PCH_H
