using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Core;
using VengineX.ECS;

namespace VengineX.Graphics.Rendering.Meshes
{
    public class MeshComponent : Component
    {
        private BaseMesh _mesh { get; }
        public Material Material { get; }
        public Transform Transform { get; }

        public MeshComponent(BaseMesh mesh, Material material)
        {
            _mesh = mesh;
            Material = material;
            Transform = new Transform();
        }

        public void Render() => _mesh.Render();

        public static implicit operator BaseMesh(MeshComponent m) => m._mesh;


        #region IDisposable

        private bool _disposedValue;

        /// <summary>
        /// Disposable pattern.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects)
                    _mesh.Dispose();
                }

                // Free unmanaged resources (unmanaged objects) and override finalizer
                // Set large fields to null
                _disposedValue = true;
            }
        }


        ///// <summary>
        ///// Disposable pattern.
        ///// </summary>
        //// Override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        //~MeshComponent()
        //{
        //    // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //    Dispose(disposing: false);
        //}


        /// <summary>
        /// Disposable pattern.
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
