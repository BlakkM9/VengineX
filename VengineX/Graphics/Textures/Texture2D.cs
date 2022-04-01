using OpenTK.Graphics.OpenGL4;
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
        /// Create a new texture from given parameters.
        /// </summary>
        /// <param name="parameters">Parameters for creating the texture.</param>
        public Texture2D(ref Texture2DParameters parameters) : this()
        {
            // Update textures size properties
            Width = parameters.Width;
            Height = parameters.Height;

            // Min/Mag
            GL.TextureParameter(Handle, TextureParameterName.TextureMinFilter, (int)parameters.MinFilter);
            GL.TextureParameter(Handle, TextureParameterName.TextureMagFilter, (int)parameters.MagFilter);

            // WrapS/WrapT
            GL.TextureParameter(Handle, TextureParameterName.TextureWrapS, (int)parameters.WrapModeS);
            GL.TextureParameter(Handle, TextureParameterName.TextureWrapT, (int)parameters.WrapModeT);

            // Texture
            GL.TextureStorage2D(Handle, 1, parameters.InternalFormat, Width, Height);
            GL.TextureSubImage2D(Handle, 0, 0, 0, Width, Height, parameters.PixelFormat, parameters.PixelType, parameters.PixelData);

            // Mipmaps
            if (parameters.GenerateMipmaps)
            {
                GL.GenerateTextureMipmap(Handle);
            }
        }


        /// <summary>
        /// Load a Texture2D from file (with given loading parameters; height and width are ignored and taken from actual image).
        /// </summary>
        public void Load(ref ILoadingParameters loadingParameters)
        {
            // Extract specific parameters
            Texture2DLoadingParameters parameters = (Texture2DLoadingParameters)loadingParameters;

            // Load image and extract pixel data
            StbiWrapper.SetFlipVerticallyOnLoad(true);
            Image image;

            // Use loading function from parameters
            switch (parameters.LoadingFunction)
            {
                case LoadingFunction.Load:
                    image = StbiWrapper.Load(parameters.FilePath, parameters.TextureParameters.InternalFormat.ChannelCount());
                    break;
                case LoadingFunction.Load16:
                    image = StbiWrapper.Load16(parameters.FilePath, parameters.TextureParameters.InternalFormat.ChannelCount());
                    break;
                case LoadingFunction.LoadF:
                    image = StbiWrapper.LoadF(parameters.FilePath, parameters.TextureParameters.InternalFormat.ChannelCount());
                    break;
                default:
                    image = StbiWrapper.Load(parameters.FilePath, parameters.TextureParameters.InternalFormat.ChannelCount());
                    Logger.Log(
                        Severity.Error,
                        Tag.Loading,
                        "Unknown stbi loading function: " + parameters.LoadingFunction + ", assuming load!");
                    break;
            }

            // Update textures size properties
            Width = image.Width;
            Height = image.Height;

            // Min/Mag
            GL.TextureParameter(Handle, TextureParameterName.TextureMinFilter, (int)parameters.TextureParameters.MinFilter);
            GL.TextureParameter(Handle, TextureParameterName.TextureMagFilter, (int)parameters.TextureParameters.MagFilter);

            // WrapS/WrapT
            GL.TextureParameter(Handle, TextureParameterName.TextureWrapS, (int)parameters.TextureParameters.WrapModeS);
            GL.TextureParameter(Handle, TextureParameterName.TextureWrapT, (int)parameters.TextureParameters.WrapModeT);

            // Texture
            GL.TextureStorage2D(Handle, 1, parameters.TextureParameters.InternalFormat, Width, Height);
            GL.TextureSubImage2D(
                Handle, 0, 0, 0, Width, Height,
                parameters.TextureParameters.PixelFormat,
                parameters.TextureParameters.PixelType,
                image.Data);

            // Mipmaps
            if (parameters.TextureParameters.GenerateMipmaps)
            {
                GL.GenerateTextureMipmap(Handle);
            }

            // Free image bytes from ram, no longer needed
            image.Dispose();
        }
    }
}
