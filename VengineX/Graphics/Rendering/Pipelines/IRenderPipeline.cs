using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Graphics.Rendering.Pipelines
{
    /// <summary>
    /// Interface for render pipelines.
    /// </summary>
    public interface IRenderPipeline
    {
        /// <summary>
        /// Render with this pipeline.
        /// </summary>
        public void Render();
    }
}
