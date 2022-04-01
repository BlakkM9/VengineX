using OpenTK.Mathematics;
using VengineX.Graphics.Cameras;
using VengineX.UI;

namespace VengineX.Graphics.Pipelines
{
    /// <summary>
    /// Base class for rendering pipelines.
    /// </summary>
    public abstract class RenderPipelineBase : IRenderPipeline
    {
        /// <summary>
        /// The main camera for this pipeline.
        /// </summary>
        public virtual Camera? MainCamera { get; set; }

        /// <summary>
        /// The clear color for this pipeline.
        /// </summary>
        public virtual Vector4 ClearColor { get; set; }

        /// <summary>
        /// Renders this pipelines content.
        /// </summary>
        public abstract void Render();
    }
}
