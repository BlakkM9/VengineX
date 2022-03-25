using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.ECS;

namespace VengineX.UI.Elements
{
    public class CanvasComponent : Component
    {
        private readonly Canvas _canvas;

        public CanvasComponent(Canvas canvas) => _canvas = canvas;

        public void UpdateEvents() => _canvas.UpdateEvents();

        public void UpdateLayout() => _canvas.UpdateLayout();

        public void Render() => _canvas.Render();

        public static implicit operator Canvas(CanvasComponent c) => c._canvas;


        #region IDisposable

        private bool _disposedValue;

        /// <summary>
        /// Disposable pattern
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects)
                    _canvas.Dispose();
                }

                // Free unmanaged resources (unmanaged objects) and override finalizer
                // Set large fields to null
                _disposedValue = true;
            }
        }


        // Override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~CanvasComponent()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }


        /// <summary>
        /// Disposable pattern
        /// </summary>
        public override void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
