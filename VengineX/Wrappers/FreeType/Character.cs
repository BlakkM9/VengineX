using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering.Textures;

namespace VengineX.Wrappers.FreeType
{
    public struct Character
    {
        //public Texture2D texture;
        public Vector2i size;
        public Vector2i bearing;
        public uint advance;


        //public void Dispose()
        //{
        //    ((IDisposable)texture).Dispose();
        //}
    }
}
