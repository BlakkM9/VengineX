using VengineX.Resources;

namespace VengineX.Graphics.Rendering.Shaders
{
    /// <summary>
    /// Parameters for loading a <see cref="Shader"/> as <see cref="ILoadableResource"/>
    /// </summary>
    public struct ShaderLoadingParameters : ILoadingParameters
    {
        /// <summary>
        /// Path to the vertex shader source file.
        /// </summary>
        public string VertexPath { get; set; }

        /// <summary>
        /// Path to the fragment shader source file.
        /// </summary>
        public string FragmentPath { get; set; }
    }
}
