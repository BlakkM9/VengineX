using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Runtime.InteropServices;
using VengineX.Graphics.Rendering.Shaders;
using VengineX.Graphics.Rendering.Textures;
using VengineX.Graphics.Rendering.Vertices;
using VengineX.Resources;

namespace VengineX.Graphics.Rendering.Batching
{
    public class UIBatchRenderer : IBatchRenderer
    {

        private Shader _batchShader;
        private Uniform _viewMatrixUniform;
        private Uniform _projMatrixUniform;

        //public Matrix4 ViewMatrix { set => _viewMatrixUniform.SetMat4(ref value); }
        //public Matrix4 ProjectionMatrix { set => _viewMatrixUniform.SetMat4(ref value); }


        private readonly int _maxVertexCount;
        private readonly int _maxIndexCount;

        private readonly Mesh<UIVertex> _mesh;

        private readonly Texture2D _whiteTexture;

        private int _indexCount = 0;

        private UIVertex[] _vertices;
        private int _vertexIndex = 0;

        private Texture2D _nextTexture;
        private Texture2D _currentTexture;

        public UIBatchRenderer(int maxQuadCount, Shader batchShader)
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

            _mesh = new Mesh<UIVertex>(Vector3.Zero, BufferUsageHint.DynamicDraw, BufferUsageHint.StaticDraw, null, indices);
            // Create new empty buffer
            _mesh.BufferVertices(null, Marshal.SizeOf<UIVertex>() * _maxVertexCount);


            //_whiteTexture = ResourceManager.GetResource<Texture2D>("texture2d.white");
            //Console.WriteLine(_whiteTexture.Handle);

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


        public UIBatchRenderer(int maxQuadCount) : this(maxQuadCount, ResourceManager.GetResource<Shader>("shader.ui.batch")) { }


        public void SetMatrices(ref Matrix4 projMatrix, ref Matrix4 viewMatrix)
        {
            _viewMatrixUniform.SetMat4(ref viewMatrix);
            _projMatrixUniform.SetMat4(ref projMatrix);
        }


        /// <summary>
        /// Adds a quad to the batch.
        /// </summary>
        public void Add(UIBatchQuad quad)
        {
            if (quad.texture == null) { _nextTexture = _whiteTexture; }
            else { _nextTexture = quad.texture; }

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
                position = new Vector3(quad.positon.X, quad.positon.Y + quad.size.Y, 0),
                color = quad.color,
                uvs = quad.uv0,
            };
            // top left
            _vertices[_vertexIndex + 1] = new UIVertex()
            {
                position = new Vector3(quad.positon.X, quad.positon.Y, 0),
                color = quad.color,
                uvs = quad.uv1,
            };
            // bottom right
            _vertices[_vertexIndex + 2] = new UIVertex()
            {
                position = new Vector3(quad.positon.X + quad.size.X, quad.positon.Y + quad.size.Y, 0),
                color = quad.color,
                uvs = quad.uv2,
            };
            // top right
            _vertices[_vertexIndex + 3] = new UIVertex()
            {
                position = new Vector3(quad.positon.X + quad.size.X, quad.positon.Y, 0),
                color = quad.color,
                uvs = quad.uv3,
            };

            _vertexIndex += 4;
            _indexCount += 6;
        }


        public void Begin()
        {
            _vertexIndex = 0;
        }


        public void End()
        {
            int size = _vertexIndex * Marshal.SizeOf<UIVertex>();
            _mesh.BufferSubData(_vertices, size);
        }


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
