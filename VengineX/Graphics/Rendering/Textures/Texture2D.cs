﻿using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Debugging.Logging;
using VengineX.Resources;
using VengineX.Resources.Stbi;
using VengineX.Utils.Extensions;

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
        /// <inheritdoc/>
        /// </summary>
        public override int Handle { get; protected set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override TextureTarget TextureTarget { get; protected set; }


        /// <summary>
        /// Creates a empty texture 2d object for being loaded via <see cref="ResourceManager"/>
        /// </summary>
        public Texture2D()
        {
            TextureTarget = TextureTarget.Texture2D;
        }


        /// <summary>
        /// Create a new texture that can be used as render target
        /// </summary>
        /// <param name="width">Width of the texture</param>
        /// <param name="height">Height of the texture</param>
        /// <param name="parameters">Settings of the texture</param>
        public Texture2D(int width, int height, ref Texture2DLoadingParameters parameters)
        {
            TextureTarget = TextureTarget.Texture2D;

            Handle = GL.GenTexture();

            GL.BindTexture(TextureTarget, Handle);

            GL.TexImage2D(
                TextureTarget,
                0,
                parameters.PixelInternalFormat,
                width,
                height,
                0,
                parameters.PixelFormat,
                parameters.PixelType,
                IntPtr.Zero
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
        /// Load a Texture2D from file (with given loading parameters).
        /// </summary>
        public void Load(ref ILoadingParameters loadingParameters)
        {
            // Extract specific parameters
            Texture2DLoadingParameters parameters = (Texture2DLoadingParameters)loadingParameters;

            // Generate texture
            Handle = GL.GenTexture();


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
                    image = StbiWrapper.Load(parameters.FilePath, parameters.PixelInternalFormat.ChannelCount());
                    break;
                case LoadingFunction.Load16:
                    image = StbiWrapper.Load16(parameters.FilePath, parameters.PixelInternalFormat.ChannelCount());
                    break;
                case LoadingFunction.LoadF:
                    image = StbiWrapper.LoadF(parameters.FilePath, parameters.PixelInternalFormat.ChannelCount());
                    break;
                default:
                    image = StbiWrapper.Load(parameters.FilePath, parameters.PixelInternalFormat.ChannelCount());
                    Logger.Log(
                        Severity.Error,
                        Tag.Loading,
                        "Unknown stbi loading function: " + parameters.LoadingFunction + ", assuming load!");
                    break;
            }


            // Generate texture
            GL.TexImage2D(
                TextureTarget,
                0,
                parameters.PixelInternalFormat,
                image.Width,
                image.Height,
                0,
                parameters.PixelFormat,
                parameters.PixelType,
                image.Data);


            // Free image bytes from ram, no longer needed
            image.Dispose();


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
    }
}
