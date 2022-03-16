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
        /// <summary>
        /// Width of the texture to create.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Height of the texture to create.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Internal pixel format.
        /// </summary>
        public SizedInternalFormat InternalFormat { get; set; }

        /// <summary>
        /// Pixel format.
        /// </summary>
        public PixelFormat PixelFormat { get; set; }

        /// <summary>
        /// Pixel type.
        /// </summary>
        public PixelType PixelType { get; set; }

        /// <summary>
        /// Minifing filter.
        /// </summary>
        public TextureMinFilter MinFilter { get; set; }

        /// <summary>
        /// Magnifiyng filter.
        /// </summary>
        public TextureMagFilter MagFilter { get; set; }

        /// <summary>
        /// Wrap mode in x dir.
        /// </summary>
        public TextureWrapMode WrapModeS { get; set; }

        /// <summary>
        /// Wrap mode in y dir.
        /// </summary>
        public TextureWrapMode WrapModeT { get; set; }

        /// <summary>
        /// Should mipmaps be generated for this texture?
        /// </summary>
        public bool GenerateMipmaps { get; set; }

        /// <summary>
        /// Optional pixel data for the texture.<br/>
        /// Default is IntPtr.Zero.
        /// </summary>
        public IntPtr PixelData { get; set; }
    }
}
