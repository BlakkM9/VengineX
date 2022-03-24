using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering.Pipelines;

namespace VengineX.Graphics.Rendering
{
    /// <summary>
    /// Interface for everything that can be rendered.<br/>
    /// For use in <see cref="RenderPipelineBase"/>.
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// Model matrix of this renderable.
        /// </summary>
        public abstract ref Matrix4 ModelMatrix { get; }

        /// <summary>
        /// Renders this renderable.
        /// </summary>
        public void Render();
    }
}
