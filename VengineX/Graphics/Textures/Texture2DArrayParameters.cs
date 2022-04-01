using OpenTK.Graphics.OpenGL4;

namespace VengineX.Graphics.Textures
{
    public struct Texture2DArrayParameters
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Layers { get; set; }

        public TextureMinFilter MinFilter { get; set; }
        public TextureMagFilter MagFilter { get; set; }

        public TextureWrapMode WrapModeS { get; set; }
        public TextureWrapMode WrapModeT { get; set; }
        public TextureWrapMode WrapModeR { get; set; }

        public SizedInternalFormat InternalFormat { get; set; }
        public PixelFormat PixelFormat { get; set; }
        public PixelType PixelType { get; set; }
        public bool GenerateMipmaps { get; set; }

        public IntPtr PixelData { get; set; }

    }
}
