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


struct Glyph
{
    unsigned short int charCode;
    int width;
    int height;
    int left;
    int top;
    unsigned int advance;
    unsigned char* bitmapData;
};


struct CharacterRange
{
    unsigned short int from;
    unsigned short int to;
};


/// <summary>
/// FT_Library instance to use.
/// </summary>
FT_Library library;


extern "C" __declspec(dllexport) int Init_FreeType()
{
    return FT_Init_FreeType(&library);
}


extern "C" __declspec(dllexport) int Done_FreeType()
{
    return FT_Done_FreeType(library);
}


extern "C" __declspec(dllexport) Glyph* Load_Glyphs(char const* filename, CharacterRange* ranges, int rangesCount, int size)
{
    // Return null if no ranges provided
    if (rangesCount <= 0)
    {
        std::cout << "ERROR::FREETYPE: No ranges found!" << std::endl;
        return nullptr;
    }

    //// Calculate total amount of characters
    int totalLen = 0;
    for (int i = 0; i < rangesCount; i++) {
        int len = ranges[i].to - ranges[i].from;

        // Invalid input, return null.
        if (len < 0)
        {
            std::cout << "ERROR::FREETYPE: Invalid range from " << ranges[i].from << " to " << ranges[i].to << "!" << std::endl;
            return nullptr;
        }

        // +1 because last char is included.
        totalLen += len + 1;
    }

    Glyph* outputGlpyhs = new Glyph[totalLen];

    FT_Face face;
    if (FT_New_Face(library, filename, 0, &face))
    {
        std::cout << "ERROR::FREETYPE: Failed to load font!" << std::endl;
        return nullptr;
    }
    else
    {
        // Set size to load glyphs as
        FT_Set_Pixel_Sizes(face, 0, size);

        // Load all ranges
        int index = 0;
        for (int i = 0; i < rangesCount; i++)
        {
            //Load range
            CharacterRange range = ranges[i];

            for (unsigned short int c = range.from; c <= range.to; c++)
            {
                // Load character glyph 
                if (FT_Load_Char(face, c, FT_LOAD_RENDER))
                {
                    std::cout << "ERROR::FREETYTPE: Failed to load Glyph!" << std::endl;
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

                if (bitmapSize > 0)
                {
                    outputGlpyhs[index].bitmapData = new unsigned char[bitmapSize];

                    for (int i = 0; i < bitmapSize; i++)
                    {
                        outputGlpyhs[index].bitmapData[i] = face->glyph->bitmap.buffer[i];
                    }
                }
                else
                {
                    outputGlpyhs[index].bitmapData = nullptr;
                }


                index++;
            }
        }



        //for (unsigned short int c = ranges[0].from; c <= ranges[0].to; c++)
        //{
        //    // Load character glyph 
        //    if (FT_Load_Char(face, c, FT_LOAD_RENDER))
        //    {
        //        std::cout << "ERROR::FREETYTPE: Failed to load Glyph" << std::endl;
        //        continue;
        //    }

        //    // Save glyph
        //    outputGlpyhs[index].charCode = c;
        //    outputGlpyhs[index].width = face->glyph->bitmap.width;
        //    outputGlpyhs[index].height = face->glyph->bitmap.rows;
        //    outputGlpyhs[index].left = face->glyph->bitmap_left;
        //    outputGlpyhs[index].top = face->glyph->bitmap_top;
        //    outputGlpyhs[index].advance = static_cast<unsigned int>(face->glyph->advance.x);

        //    // Copy bitmap data
        //    int bitmapSize = face->glyph->bitmap.width * face->glyph->bitmap.rows;
        //    outputGlpyhs[index].bitmapData = new unsigned char[bitmapSize];

        //    for (int i = 0; i < bitmapSize; i++)
        //    {
        //        outputGlpyhs[index].bitmapData[i] = face->glyph->bitmap.buffer[i];
        //    }

        //    index++;
        //}
    }

    // destroy FreeType once we're finished
    FT_Done_Face(face);

    return outputGlpyhs;
}

extern "C" __declspec(dllexport) int Free_Glyphs(Glyph* glyphsToFree, int length)
{
    // Delete bitmaps
    for (int i = 0; i < length; i++) {
        delete[] glyphsToFree[i].bitmapData;
    }

    // Delete glyphs
    delete[] glyphsToFree;

    return 0;
}

#endif //PCH_H
