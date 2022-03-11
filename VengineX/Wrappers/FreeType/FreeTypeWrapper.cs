using Minetekk.Engine.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Wrappers.FreeType
{
    public static class FreeTypeWrapper
    {

        private const string dllFilePath = "lib/freetypelib.dll";


        [DllImport(dllFilePath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe extern static bool Init_FreeType();


        [DllImport(dllFilePath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe extern static bool Free_FreeType();


        [DllImport(dllFilePath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe extern static Glyph* Load_Glyphs(ref byte str, byte from, byte to, int fontSize);


        [DllImport(dllFilePath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe extern static bool Free_Glyphs(Glyph* glyphsToFree, int length);


        /// <summary>
        /// Loads glyphs from a true type font using free type font.
        /// </summary>
        /// <param name="filePath">Path to ttf file</param>
        /// <param name="fromCharCode">Start of range to load (char codes)</param>
        /// <param name="toCharCode">End of range to load (char code)</param>
        /// <param name="size">Font size</param>
        /// <returns>Unamanged array holding all loaded glyphs. Dont release it, use <see cref="FreeGlyphs(ref UnmanagedArray{Glyph})"/> instead!</returns>
        public static unsafe Glyph* LoadGlyphs(string filePath, byte fromCharCode, byte toCharCode, int size)
        {
            // TODO don't do that every time
            Init_FreeType();


            // Convert file path to c string
            byte[] bytes = Encoding.ASCII.GetBytes(filePath);
            //int length = (toCharCode - fromCharCode);

            // Load glyphs with free type
            Glyph* glyphs = Load_Glyphs(ref bytes[0], fromCharCode, toCharCode, size);


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


        public static unsafe void FreeGlyphs(Glyph* glyphsToFree, int length)
        {
            Free_Glyphs(glyphsToFree, length);
        }


        public static void FreeFreeType()
        {
            Free_FreeType();
        }
    }
}
