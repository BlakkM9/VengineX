using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using VengineX.Graphics.Rendering.Vertices;

namespace VengineX.Graphics.Rendering.UnitModels
{
    /// <summary>
    /// A simple quad from -1 to 1 using <see cref="UIVertex"/>.<br/>
    /// Might be used for postprocessing and ui rendering.
    /// </summary>
    public class Quad : IDisposable
    {
        private static readonly UIVertex[] vertices = new UIVertex[] {
            new UIVertex()
            {
                position = new Vector3(-1, 1, 0),
                uvs = new Vector2(0, 1),
                color = Vector4.One
            },
            new UIVertex()
            {
                position = new Vector3(-1, -1, 0),
                uvs = new Vector2(0, 0),
                color = Vector4.One
            },
            new UIVertex()
            {
                position = new Vector3(1, 1, 0),
                uvs = new Vector2(1, 1),
                color = Vector4.One
            },
            new UIVertex()
            {
                position = new Vector3(1, -1, 0),
                uvs = new Vector2(1, 0),
                color = Vector4.One
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
            _mesh = new Mesh<UIVertex>(Vector3.Zero, BufferUsageHint.StaticDraw, BufferUsageHint.StaticDraw, vertices, indices);
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

        /// <summary>
        /// Disposable pattern.
        /// </summary>
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

        /// <summary>
        /// Disposable pattern.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
