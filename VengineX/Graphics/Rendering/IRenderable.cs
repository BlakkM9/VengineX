using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Graphics.Rendering
{
    /// <summary>
    /// Interface for everything that can be rendered.<br/>
    /// For use in <see cref="RenderPipelineBase"/>.
    /// </summary>
    public interface IRenderable
    {
        public abstract ref Matrix4 ModelMatrix { get; }

        public void Render();
    }
}
