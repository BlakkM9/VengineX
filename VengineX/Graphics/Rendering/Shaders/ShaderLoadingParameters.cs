using VengineX.Resources;

namespace VengineX.Graphics.Rendering.Shaders
{
    /// <summary>
    /// Parameters for loading a <see cref="Shader"/> as <see cref="ILoadableResource"/>
    /// </summary>
    public struct ShaderLoadingParameters : ILoadingParameters
    {
        /// <summary>
        /// The first token of automaticall generated resource strings.
        /// </summary>
        private const string RESOURCE_TYPE = "shader";

        /// <summary>
        /// Path to the vertex shader source file.
        /// </summary>
        public string VertexPath { get; set; }

        /// <summary>
        /// Path to the fragment shader source file.
        /// </summary>
        public string FragmentPath { get; set; }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string ProvideResourcePath()
        {
            // Split path
            string[] tokens = FragmentPath.Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

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
