using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VengineX.Debugging.Logging;

namespace VengineX.Wrappers.FreeType
{
    public static class FreeTypeWrapper
    {

        private const string dllFilePath = "lib/freetypelib.dll";


        [DllImport(dllFilePath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe extern static bool Init_FreeType();


        [DllImport(dllFilePath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe extern static bool Done_FreeType();


        [DllImport(dllFilePath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe extern static FreeTypeGlyph* Load_Glyphs(ref byte str, byte from, byte to, int fontSize);


        [DllImport(dllFilePath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe extern static bool Free_Glyphs(FreeTypeGlyph* glyphsToFree, int length);


        /// <summary>
        /// Initializes freetype. Call before loading glyphs.
        /// </summary>
        public static void InitFreeType()
        {
            if (!Initialized)
            {
                if (Init_FreeType())
                {
                    Initialized = true;
                    Logger.Log(Severity.Info, Tag.Initialization, "FreeType initialized.");
                }
                else
                {
                    Logger.Log(Severity.Error, Tag.Initialization, "Failed to initialize FreeType");
                }
            }
            else
            {
                Logger.Log(Severity.Warning, "FreeType already initialized!");
            }
        }


        public static bool Initialized { get; private set; } = false;


        /// <summary>
        /// Loads glyphs from a true type font using free type font.
        /// </summary>
        /// <param name="filePath">Path to ttf file</param>
        /// <param name="fromCharCode">Start of range to load (char codes)</param>
        /// <param name="toCharCode">End of range to load (char code)</param>
        /// <param name="size">Font size</param>
        /// <returns>Unamanged array holding all loaded glyphs. Dont release it, use <see cref="FreeGlyphs(FreeTypeGlyph*, int)"/> instead!</returns>
        public static unsafe FreeTypeGlyph* LoadGlyphs(string filePath, byte fromCharCode, byte toCharCode, int size)
        {
            if (!Initialized)
            {
                InitFreeType();
            }

            // Convert file path to c string
            byte[] bytes = Encoding.ASCII.GetBytes(filePath);
            //int length = (toCharCode - fromCharCode);

            // Load glyphs with free type
            FreeTypeGlyph* glyphs = Load_Glyphs(ref bytes[0], fromCharCode, toCharCode, size);


            //// TODO TEST log glyphs
            //for (int i = 0; i < length; i++) {
            //    Console.Write($"{(char)glyphs[i].charCode} ({glyphs[i].charCode})={(IntPtr)glyphs[i].bitmapData}, ");
            //Console.WriteLine($"charCode:   {glyphs[i].charCode}");
            //Console.WriteLine($"char:       {(char)glyphs[i].charCode}");
            //Console.WriteLine($"width:      {glyphs[i].width}");
            //Console.WriteLine($"height:     {glyphs[i].height}");
            //Console.WriteLine($"left:       {glyphs[i].left}");
            //Console.WriteLine($"top:        {glyphs[i].top}");
            //Console.WriteLine($"advance:    {glyphs[i].advance}");
            //Console.WriteLine($"data:       {(IntPtr)glyphs[i].bitmapData:X}\n");
            //}
            //Console.WriteLine();


            return glyphs;
        }


        /// <summary>
        /// Deletes the unmanaged glyph array.<br/>
        /// Call this if you don't need the loaded glyphs anymore.<br/>
        /// Not calling this will lead to memory leaks.
        /// </summary>
        /// <param name="glyphsToFree">The unmanaged glyphs array to free.</param>
        /// <param name="length">Lenght of the unmanaged array.</param>
        public static unsafe void FreeGlyphs(FreeTypeGlyph* glyphsToFree, int length)
        {
            Free_Glyphs(glyphsToFree, length);
        }


        /// <summary>
        /// Uninitializes freetype.<br/>
        /// Call when you are done using freetype.
        /// </summary>
        public static void DoneFreeType()
        {
            if (Initialized)
            {
                Done_FreeType();
                Initialized = false;
            }
        }
    }
}
