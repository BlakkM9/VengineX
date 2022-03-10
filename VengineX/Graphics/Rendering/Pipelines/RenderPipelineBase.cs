using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Graphics.Rendering.Pipelines
{
    /// <summary>
    /// Base class for rendering pipelines.
    /// </summary>
    public abstract class RenderPipelineBase : IRenderPipeline, IDisposable
    {
        private bool disposedValue;

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


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DisposeManaged();
                }

                DisposeUnmanaged();
                disposedValue = true;
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
    }
}
