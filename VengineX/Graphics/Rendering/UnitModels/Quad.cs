using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering.Vertices;

namespace VengineX.Graphics.Rendering.UnitModels
{

    public class Quad : IDisposable
    {

        private static readonly UIVertex[] vertices = new UIVertex[] {
            new UIVertex()
            {
                position = new Vector3(-1, 1, 0),
                uv = new Vector2(0, 1)
            },
            new UIVertex()
            {
                position = new Vector3(-1, -1, 0),
                uv = new Vector2(0, 0)
            },
            new UIVertex()
            {
                position = new Vector3(1, 1, 0),
                uv = new Vector2(1, 1)
            },
            new UIVertex()
            {
                position = new Vector3(1, -1, 0),
                uv = new Vector2(1, 0)
            }
        };

        private static readonly uint[] indices = new uint[] {
            0, 1, 2,
            2, 1, 3,
        };

        // TODO this could be reused and only created once.
        /// <summary>
        /// Mesh for the unit quad.
        /// </summary>
        private readonly Mesh<UIVertex> _mesh;


        /// <summary>
        /// Creates a new unit quad.
        /// </summary>
        public Quad()
        {
            _mesh = new Mesh<UIVertex>(Vector3.Zero, BufferUsageHint.StaticDraw, vertices, indices);
        }

        /// <summary>
        /// Renders the unitquad.
        /// </summary>
        public void Render()
        {
            _mesh.Render();
        }


        #region IDisposable

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _mesh.Dispose();
                }

                _disposedValue = true;
            }
        }

        //~Quad()
        //{
        //    // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //    Dispose(disposing: false);
        //}

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
