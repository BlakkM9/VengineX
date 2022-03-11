using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Graphics.Rendering.Textures
{
    /// <summary>
    /// Parameters for creating a texture.
    /// </summary>
    public struct Texture2DParameters
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public TextureTarget TextureTarget { get; set; }
        public PixelInternalFormat PixelInternalFormat { get; set; }
        public PixelFormat PixelFormat { get; set; }
        public PixelType PixelType { get; set; }
        public TextureMinFilter MinFilter { get; set; }
        public TextureMagFilter MagFilter { get; set; }
        public TextureWrapMode WrapModeS { get; set; }
        public TextureWrapMode WrapModeT { get; set; }
        public bool GenerateMipmaps { get; set; }
    }
}
