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
        /// The first token of automaticall generated resource strings.
        /// </summary>
        private const string RESOURCE_TYPE = "texture2d";

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


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string ProvideResourcePath()
        {
            // Split path
            string[] tokens = FilePath.Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            // Strip extension
            string fileName = tokens[^1];
            fileName = fileName[..fileName.IndexOf('.')];

            // Join RESOURCE_TYPE, path except res + rootFolder and fileName
            List<string> resourceTokens = new List<string>();
            resourceTokens.Add(RESOURCE_TYPE);
            resourceTokens.AddRange(tokens[2..^1]);
            resourceTokens.Add(fileName);
            string resourcePath = string.Join(".", resourceTokens);

            return resourcePath.ToLower();
        }
    }
}
