// pch.h: This is a precompiled header file.
// Files listed below are compiled only once, improving build performance for future builds.
// This also affects IntelliSense performance, including code completion and many code browsing features.
// However, files listed here are ALL re-compiled if any one of them is updated between builds.
// Do not add files here that you will be updating frequently as this negates the performance advantage.

#ifndef PCH_H
#define PCH_H

// add headers that you want to pre-compile here
#include "framework.h"
#include <iostream>
#include <ft2build.h>
#include FT_FREETYPE_H

struct Glyph {
    unsigned char charCode;
    int width;
    int height;
    int left;
    int top;
    unsigned int advance;
    unsigned char* bitmapData;
};

FT_Library library;

extern "C" __declspec(dllexport) bool Init_FreeType() {
    return (bool)FT_Init_FreeType(&library);
}

extern "C" __declspec(dllexport) bool Done_FreeType() {
    return (bool)FT_Done_FreeType(library);
}

extern "C" __declspec(dllexport) Glyph* Load_Glyphs(char const* filename, unsigned char from, unsigned char to, int size) {
    if (to <= from) return nullptr;

    Glyph* outputGlpyhs = new Glyph[to - from];

    FT_Face face;
    if (FT_New_Face(library, filename, 0, &face)) {
        std::cout << "ERROR::FREETYPE: Failed to load font" << std::endl;
        return nullptr;
    }
    else {
        // set size to load glyphs as
        FT_Set_Pixel_Sizes(face, 0, size);

        // load first 128 characters of ASCII set
        int index = 0;
        for (unsigned char c = from; c < to; c++) {
            // Load character glyph 
            if (FT_Load_Char(face, c, FT_LOAD_RENDER)) {
                std::cout << "ERROR::FREETYTPE: Failed to load Glyph" << std::endl;
                continue;
            }

            // Save glyph
            outputGlpyhs[index].charCode = c;
            outputGlpyhs[index].width = face->glyph->bitmap.width;
            outputGlpyhs[index].height = face->glyph->bitmap.rows;
            outputGlpyhs[index].left = face->glyph->bitmap_left;
            outputGlpyhs[index].top = face->glyph->bitmap_top;
            outputGlpyhs[index].advance = static_cast<unsigned int>(face->glyph->advance.x);

            // Propably the line that causes bitmap corruption?!
            // Copy bitmap data
            int bitmapSize = face->glyph->bitmap.width * face->glyph->bitmap.rows;
            outputGlpyhs[index].bitmapData = new unsigned char[bitmapSize];

            for (int i = 0; i < bitmapSize; i++) {
                outputGlpyhs[index].bitmapData[i] = face->glyph->bitmap.buffer[i];
            }

            index++;
        }
    }

    // destroy FreeType once we're finished
    FT_Done_Face(face);

    return outputGlpyhs;
}

extern "C" __declspec(dllexport) bool Free_Glyphs(Glyph* glyphsToFree, int length) {
    // Delete bitmaps
    for (int i = 0; i < length; i++) {
        delete[] glyphsToFree[i].bitmapData;
    }

    // Delete glyphs
    delete[] glyphsToFree;

    return true;
}

#endif //PCH_H
