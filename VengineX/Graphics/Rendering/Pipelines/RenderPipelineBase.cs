using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering.Cameras;
using VengineX.UI.Canvases;

namespace VengineX.Graphics.Rendering.Pipelines
{
    /// <summary>
    /// Base class for rendering pipelines.
    /// </summary>
    public abstract class RenderPipelineBase : IRenderPipeline, IDisposable
    {
        /// <summary>
        /// The main camera for this pipeline.
        /// </summary>
        public virtual Camera Camera { get; protected set; }

        /// <summary>
        /// The overlay ui in this pipeline.
        /// </summary>
        public virtual Canvas OverlayUI { get; protected set; }

        /// <summary>
        /// The clear color for this pipeline.
        /// </summary>
        public virtual Vector4 ClearColor { get; set; }

        /// <summary>
        /// Renders this pipelines content.
        /// </summary>
        public abstract void Render();


        /// <summary>
        /// Dispose managed state (managed objects) here.
        /// </summary>
        public abstract void DisposeManaged();


        /// <summary>
        /// Free unmanaged resources (unmanaged objects) and<br/>
        /// set large fields to null here.
        /// </summary>
        public abstract void DisposeUnmanaged();


        #region IDisposable

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    DisposeManaged();
                }

                DisposeUnmanaged();
                _disposedValue = true;
            }
        }

        ~RenderPipelineBase()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }


        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
