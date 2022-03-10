using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Resources;
using VengineX.Resources.Stbi;

namespace VengineX.Graphics.Rendering.Textures
{
    /// <summary>
    /// Parameters for loading a <see cref="Texture2D"/>.
    /// </summary>
    public struct Texture2DLoadingParameters : ILoadingParameters
    {
        /// <summary>
        /// Path to the texture.
        /// </summary>
        public string FilePath { get; set; }
        public PixelInternalFormat PixelInternalFormat { get; set; }
        public PixelFormat PixelFormat { get; set; }
        public PixelType PixelType { get; set; }
        public TextureMinFilter MinFilter { get; set; }
        public TextureMagFilter MagFilter { get; set; }
        public TextureWrapMode WrapModeS { get; set; }
        public TextureWrapMode WrapModeT { get; set; }
        public LoadingFunction LoadingFunction { get; set; }
        public bool GenerateMipmaps { get; set; }
    }
}
