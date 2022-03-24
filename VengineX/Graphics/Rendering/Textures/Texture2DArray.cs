using OpenTK.Graphics.OpenGL4;
using VengineX.Resources;

namespace VengineX.Graphics.Rendering.Textures
{
    public class Texture2DArray : Texture
    {
        public int Width { get; private set; }

        public int Height { get; private set; }

        public int Layers { get; private set; }



        /// <summary>
        /// Creates a empty texture 2d object for being loaded via <see cref="ResourceManager"/>
        /// </summary>
        public Texture2DArray() : base(TextureTarget.Texture2DArray) { }

        /// <summary>
        /// Creates a new texture 2d array from given parameters.
        /// </summary>
        public Texture2DArray(ref Texture2DArrayParameters parameters) : this()
        {
            Width = parameters.Width;
            Height = parameters.Height;
            Layers = parameters.Layers;

            // Min/Mag
            GL.TextureParameter(Handle, TextureParameterName.TextureMinFilter, (int)parameters.MinFilter);
            GL.TextureParameter(Handle, TextureParameterName.TextureMagFilter, (int)parameters.MagFilter);

            // Wrap S/T/R
            GL.TextureParameter(Handle, TextureParameterName.TextureWrapS, (int)parameters.WrapModeS);
            GL.TextureParameter(Handle, TextureParameterName.TextureWrapT, (int)parameters.WrapModeT);
            GL.TextureParameter(Handle, TextureParameterName.TextureWrapR, (int)parameters.WrapModeR);

            GL.TextureStorage3D(Handle, 1, parameters.InternalFormat, Width, Height, Layers);
            GL.TextureSubImage3D(Handle, 0, 0, 0, 0, Width, Height, Layers, parameters.PixelFormat, parameters.PixelType, parameters.PixelData);

            // Generate mipmaps
            if (parameters.GenerateMipmaps)
            {
                GL.GenerateTextureMipmap(Handle);
            }
        }
    }
}
