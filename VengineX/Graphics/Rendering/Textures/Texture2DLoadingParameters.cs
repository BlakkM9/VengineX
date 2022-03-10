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
        public string filePath;
        public PixelInternalFormat pixelInternalFormat;
        public PixelFormat pixelFormat;
        public PixelType pixelType;
        public TextureMinFilter minFilter;
        public TextureMagFilter magFilter;
        public TextureWrapMode wrapModeS;
        public TextureWrapMode wrapModeT;
        public LoadingFunction loadingFunction;
        public bool generateMipmaps;
    }
}
