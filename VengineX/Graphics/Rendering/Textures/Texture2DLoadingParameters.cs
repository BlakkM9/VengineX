using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Resources;
using VengineX.Wrappers.Stbi;

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

        /// <summary>
        /// The stbi_image loading function to use for loading this image.
        /// </summary>
        public LoadingFunction LoadingFunction { get; set; }

        /// <summary>
        /// Parameters for creating the texture with OpenGL.
        /// </summary>
        public Texture2DParameters TextureParameters { get; set; }
    }
}
