using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public string VertexPath { get; }

        /// <summary>
        /// Path to the fragment shader source file.
        /// </summary>
        public string FragmentPath { get; }


        /// <param name="vertexPath">Path to the vertex shader source file.</param>
        /// <param name="fragmentPath">Path to the fragment shader source file.</param>
        public ShaderLoadingParameters(string vertexPath, string fragmentPath)
        {
            VertexPath = vertexPath;
            FragmentPath = fragmentPath;
        }
    }
}
