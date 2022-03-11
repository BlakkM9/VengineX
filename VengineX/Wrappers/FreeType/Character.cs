using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Wrappers.FreeType
{
    public struct Character
    {
        public int texture;
        public Vector2i size;
        public Vector2i bearing;
        public uint advance;
    }
}
