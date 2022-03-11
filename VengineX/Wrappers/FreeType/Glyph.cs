using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Wrappers.FreeType
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Glyph
    {
        public byte charCode;
        public int width;
        public int height;
        public int left;
        public int top;
        public uint advance;
        public byte* bitmapData;
    }
}
