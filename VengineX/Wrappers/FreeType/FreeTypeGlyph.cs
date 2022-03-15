using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Wrappers.FreeType
{
    /// <summary>
    /// Glyph that is returned from freetype when loading a font.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FreeTypeGlyph
    {
        public char charCode;
        public int width;
        public int height;
        public int left;
        public int top;
        public uint advance;
        public byte* bitmapData;
    }
}
