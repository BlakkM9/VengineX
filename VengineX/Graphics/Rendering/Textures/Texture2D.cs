using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Debugging.Logging;
using VengineX.Resources;
using VengineX.Utils.Extensions;
using VengineX.Wrappers.Stbi;

namespace VengineX.Graphics.Rendering.Textures
{
    /// <summary>
    /// A single texture to use in shaders.
    /// </summary>
    public class Texture2D : Texture, IResource, ILoadableResource
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string ResourcePath { get; set; } = "";

        /// <summary>
        /// Width of this texture (in pixels).
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Height of this texture (in pixels).
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Creates a empty texture 2d object for being loaded via <see cref="ResourceManager"/>
        /// </summary>
        public Texture2D() : base(TextureTarget.Texture2D) { }


        /// <summary>
        /// Create a new empty texture.
        /// </summary>
        /// <param name="parameters">Parameters for creating the texture.</param>
        public Texture2D(ref Texture2DParameters parameters) : base(TextureTarget.Texture2D)
        {
            // Update textures size properties
            Width = parameters.Width;
            Height = parameters.Height;

            GL.BindTexture(TextureTarget, Handle);

            GL.TexImage2D(
                TextureTarget,
                0,
                parameters.PixelInternalFormat,
                parameters.Width,
                parameters.Height,
                0,
                parameters.PixelFormat,
                parameters.PixelType,
                parameters.PixelData
            );

            // Set parameters
            // Min/Mag
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)parameters.MinFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)parameters.MagFilter);


            // WrapS/WrapT
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)parameters.WrapModeS);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)parameters.WrapModeT);


            // Mipmaps
            if (parameters.GenerateMipmaps)
            {
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            }
        }


        /// <summary>
        /// Load a Texture2D from file (with given loading parameters; height and width are ignored and taken from actual image).
        /// </summary>
        public void Load(ref ILoadingParameters loadingParameters)
        {
            // Extract specific parameters
            Texture2DLoadingParameters parameters = (Texture2DLoadingParameters)loadingParameters;

            // Bind the handle
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget, Handle);


            // Load image and extract pixel data
            StbiWrapper.SetFlipVerticallyOnLoad(true);
            Image image;
            // Use loading function from parameters
            switch (parameters.LoadingFunction)
            {
                case LoadingFunction.Load:
                    image = StbiWrapper.Load(parameters.FilePath, parameters.TextureParameters.PixelInternalFormat.ChannelCount());
                    break;
                case LoadingFunction.Load16:
                    image = StbiWrapper.Load16(parameters.FilePath, parameters.TextureParameters.PixelInternalFormat.ChannelCount());
                    break;
                case LoadingFunction.LoadF:
                    image = StbiWrapper.LoadF(parameters.FilePath, parameters.TextureParameters.PixelInternalFormat.ChannelCount());
                    break;
                default:
                    image = StbiWrapper.Load(parameters.FilePath, parameters.TextureParameters.PixelInternalFormat.ChannelCount());
                    Logger.Log(
                        Severity.Error,
                        Tag.Loading,
                        "Unknown stbi loading function: " + parameters.LoadingFunction + ", assuming load!");
                    break;
            }


            // Update textures size properties
            Width = image.Width;
            Height = image.Height;

            // Generate texture
            GL.TexImage2D(
                TextureTarget,
                0,
                parameters.TextureParameters.PixelInternalFormat,
                image.Width,
                image.Height,
                0,
                parameters.TextureParameters.PixelFormat,
                parameters.TextureParameters.PixelType,
                image.Data);


            // Free image bytes from ram, no longer needed
            image.Dispose();


            // Set parameters
            // Min/Mag
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)parameters.TextureParameters.MinFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)parameters.TextureParameters.MagFilter);


            // WrapS/WrapT
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)parameters.TextureParameters.WrapModeS);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)parameters.TextureParameters.WrapModeT);


            // Mipmaps
            if (parameters.TextureParameters.GenerateMipmaps)
            {
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            }
        }
    }
}
