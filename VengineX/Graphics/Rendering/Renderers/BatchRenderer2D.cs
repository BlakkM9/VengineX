using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Runtime.InteropServices;
using VengineX.Graphics.Rendering.Cameras;
using VengineX.Graphics.Rendering.Meshes;
using VengineX.Graphics.Rendering.Shaders;
using VengineX.Graphics.Rendering.Textures;
using VengineX.Graphics.Rendering.Vertices;
using VengineX.Resources;

namespace VengineX.Graphics.Rendering.Renderers
{
    public class BatchRenderer2D : IDisposable
    {

        private Shader _batchShader;
        private Uniform _viewMatrixUniform;
        private Uniform _projMatrixUniform;


        private readonly int _maxVertexCount;
        private readonly int _maxIndexCount;

        private readonly Mesh<UIVertex> _mesh;

        private readonly Texture2D _whiteTexture;

        private int _indexCount = 0;

        private UIVertex[] _vertices;
        private int _vertexIndex = 0;

        private Texture2D _nextTexture;
        private Texture2D _currentTexture;


        public BatchRenderer2D(int maxQuadCount, Shader batchShader)
        {
            _batchShader = batchShader;
            _batchShader.GetUniform("uTexture").Set1(0);
            _viewMatrixUniform = _batchShader.GetUniform("V");
            _projMatrixUniform = _batchShader.GetUniform("P");

            _maxVertexCount = maxQuadCount * 4;
            _maxIndexCount = maxQuadCount * 6;
            _vertices = new UIVertex[_maxVertexCount];

            uint[] indices = new uint[_maxIndexCount];
            uint offset = 0;

            for (int i = 0; i < indices.Length; i += 6)
            {
                indices[i + 0] = 0 + offset;
                indices[i + 1] = 1 + offset;
                indices[i + 2] = 2 + offset;

                indices[i + 3] = 2 + offset;
                indices[i + 4] = 1 + offset;
                indices[i + 5] = 3 + offset;

                offset += 4;
            }

            _mesh = new Mesh<UIVertex>(BufferUsageHint.DynamicDraw, BufferUsageHint.StaticDraw, null, indices);
            // Create new empty buffer
            _mesh.BufferVertices(null, Marshal.SizeOf<UIVertex>() * _maxVertexCount);


            // TODO find out why this cannot be done in game load and received from cache
            byte[] white = new byte[]
            {
                0xff, 0xff, 0xff, 0xff
            };
            GCHandle pinned = GCHandle.Alloc(white, GCHandleType.Pinned);

            Texture2DParameters texParams = new Texture2DParameters()
            {
                Height = 1,
                Width = 1,
                PixelFormat = PixelFormat.Rgba,
                InternalFormat = SizedInternalFormat.Rgba8,
                PixelType = PixelType.UnsignedByte,
                WrapModeS = TextureWrapMode.Repeat,
                WrapModeT = TextureWrapMode.Repeat,
                GenerateMipmaps = false,
                PixelData = pinned.AddrOfPinnedObject(),

            };
            _whiteTexture = new Texture2D(ref texParams);
            pinned.Free();

            _currentTexture = _whiteTexture;
            _nextTexture = _whiteTexture;
        }


        /// <summary>
        /// Creates a new batch renderer 2d with the given max quad count.<br/>
        /// Uses the default batch ui shader ("shader.ui.batch").
        /// </summary>
        /// <param name="maxQuadCount"></param>
        public BatchRenderer2D(int maxQuadCount) : this(maxQuadCount, ResourceManager.GetResource<Shader>("shader.ui.batch")) { }


        /// <summary>
        /// Begin the batch renderer. Submit geometry between <see cref="Begin(Camera)"/> and <see cref="End"/><br/>
        /// Render batch by calling <see cref="Flush"/>.
        /// </summary>
        /// <param name="camera">Camera that provides view and proj matrices for this batch.</param>
        public void Begin(Camera camera)
        {
            _vertexIndex = 0;
            _viewMatrixUniform.SetMat4(ref camera.ViewMatrix);
            _projMatrixUniform.SetMat4(ref camera.ProjectionMatrix);
        }


        /// <summary>
        /// Beginning new batch inside of current batch. View and Proj matrices do not need to set again.
        /// </summary>
        private void Begin()
        {
            _vertexIndex = 0;
        }


        /// <summary>
        /// Submits a quad to the renderer with given values.
        /// </summary>
        /// <param name="size">Size of the quad.</param>
        /// <param name="position">Absolute position of the quad.</param>
        /// <param name="uvs">Array with lengh of 4, containing uvs for quad.</param>
        /// <param name="color">Color/Tint of the quad.</param>
        /// <param name="texture">Texture of this quad. Might be null if no texture.</param>
        public void Submit(Vector2 size, Vector2 position, Vector2[] uvs, Vector4 color, Texture2D? texture)
        {
            if (texture == null) { _nextTexture = _whiteTexture; }
            else { _nextTexture = texture; }

            if (_indexCount + 6 > _maxIndexCount || _currentTexture.Handle != _nextTexture.Handle)
            {
                End();
                Flush();
                Begin();
            }
            _currentTexture = _nextTexture;

            // bottom left
            _vertices[_vertexIndex + 0] = new UIVertex()
            {
                position = new Vector3(position.X, position.Y + size.Y, 0),
                color = color,
                uvs = uvs[0],
            };
            // top left
            _vertices[_vertexIndex + 1] = new UIVertex()
            {
                position = new Vector3(position.X, position.Y, 0),
                color = color,
                uvs = uvs[1],
            };
            // bottom right
            _vertices[_vertexIndex + 2] = new UIVertex()
            {
                position = new Vector3(position.X + size.X, position.Y + size.Y, 0),
                color = color,
                uvs = uvs[2],
            };
            // top right
            _vertices[_vertexIndex + 3] = new UIVertex()
            {
                position = new Vector3(position.X + size.X, position.Y, 0),
                color = color,
                uvs = uvs[3],
            };

            _vertexIndex += 4;
            _indexCount += 6;
        }


        /// <summary>
        /// Submits the given quad to the batch.
        /// </summary>
        public void Submit(QuadVertex quad)
        {
            Submit(quad.size, quad.position,
                new Vector2[] { quad.uv0, quad.uv1, quad.uv2, quad.uv3 },
                quad.color, quad.texture);
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void End()
        {
            int size = _vertexIndex * Marshal.SizeOf<UIVertex>();
            _mesh.BufferSubData(_vertices, size);
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Flush()
        {
            _currentTexture.Bind();
            _batchShader.Bind();

            _mesh.Render(_indexCount);
            _indexCount = 0;
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
                    // Dispose managed state (managed objects)
                    _whiteTexture.Dispose();
                    _mesh.Dispose();
                }

                // Free unmanaged resources (unmanaged objects) and override finalizer
                // Set large fields to null
                _vertices = null;
                _disposedValue = true;
            }
        }

        //// Override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        //~UIBatchRenderer()
        //{
        //    // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //    Dispose(disposing: false);
        //}

        /// <summary>
        /// Disposable pattern
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
