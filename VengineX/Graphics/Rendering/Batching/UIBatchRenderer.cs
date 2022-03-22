using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VengineX.Debugging.Logging;
using VengineX.Graphics.Rendering.Shaders;
using VengineX.Graphics.Rendering.Textures;
using VengineX.Resources;
using VengineX.Utils;

namespace VengineX.Graphics.Rendering.Batching
{
    public struct Vertex
    {
        public Vector3 position;
        public Vector4 color;
        public Vector2 uvs;
        public float textureIndex;

        public override string ToString()
        {
            return $"pos: {position} col: {color} uvs: {uvs} texIndex: {textureIndex}";
        }
    }

    public struct UIQuad
    {
        public Vector2 size;
        public Vector2 positon;
        public Vector2[] uvs;
        public Vector4 color;
        public Texture2D? texture;
    }

    public class UIBatchRenderer : IBatchRenderer
    {

        private static Shader _uiBatchShader;
        public static Uniform ViewMatrixUniform { get; private set; }
        public static Uniform ProjMatrixUniform { get; private set; }

        private static int _maxTextureCount;
        private readonly int _maxQuadCount;
        private readonly int _maxVertexCount;
        private readonly int _maxIndexCount;

        private readonly Mesh<Vertex> _mesh;

        private readonly Texture2D _whiteTexture;
        private readonly uint _whiteTextureSlot = 0;

        private int _indexCount = 0;

        private Vertex[] _vertices;
        private int _vertexIndex = 0;

        private readonly Texture2D[] _textures;
        private int _textureSlotIndex = 1;

        public UIBatchRenderer(int maxQuadCount)
        {
            // Lazy shader init
            if (_uiBatchShader == null)
            {
                _maxTextureCount = GL.GetInteger(GetPName.MaxTextureImageUnits);

                _uiBatchShader = ResourceManager.GetResource<Shader>("shader.ui.batch");

                int[] samplers = new int[_maxTextureCount];
                for (int i = 0; i < samplers.Length; i++) { samplers[i] = i; }
                _uiBatchShader.GetUniform("uTextures[0]").Set1(samplers);

                ViewMatrixUniform = _uiBatchShader.GetUniform("V");
                ProjMatrixUniform = _uiBatchShader.GetUniform("P");
            }

            _maxVertexCount = maxQuadCount * 4;
            _maxIndexCount = maxQuadCount * 6;
            _vertices = new Vertex[_maxVertexCount];


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

            _mesh = new Mesh<Vertex>(Vector3.Zero, BufferUsageHint.DynamicDraw, BufferUsageHint.StaticDraw, null, indices);
            _mesh.BufferVertices(null, Marshal.SizeOf<Vertex>() * _maxVertexCount);


            unsafe
            {
                byte[] white = new byte[]
                {
                    0xff, 0xff, 0xff, 0xff
                };

                fixed (byte* pWhite = white)
                {
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
                        PixelData = (IntPtr)pWhite

                    };

                    _whiteTexture = new Texture2D(ref texParams);
                }
            }

            _textures = new Texture2D[_maxTextureCount];
            _textures[0] = _whiteTexture;
        }


        /// <summary>
        /// Adds a quad to the batch.
        /// </summary>
        public void Add(UIQuad element)
        {
            if (_indexCount + 6 > _maxIndexCount || _textureSlotIndex > _maxTextureCount - 1)
            {
                End();
                Flush();
                Begin();
            }


            float textureIndex = 0.0f;

            if (element.texture != null)
            {
                for (int i = 1; i < _textureSlotIndex; i++)
                {
                    if (_textures[i].Handle == element.texture.Handle)
                    {
                        _textureSlotIndex = i;
                        break;
                    }
                }

                if (textureIndex == 0.0f)
                {
                    textureIndex = _textureSlotIndex;
                    _textures[_textureSlotIndex] = element.texture;
                    _textureSlotIndex++;
                }
            }

            // bottom left
            _vertices[_vertexIndex + 0] = new Vertex()
            {
                position = new Vector3(element.positon.X, element.positon.Y + element.size.Y, 0),
                color = element.color,
                uvs = element.uvs[0],
                textureIndex = textureIndex,
            };
            // top left
            _vertices[_vertexIndex + 1] = new Vertex()
            {
                position = new Vector3(element.positon.X, element.positon.Y, 0),
                color = element.color,
                uvs = element.uvs[1],
                textureIndex = textureIndex,
            };
            // bottom right
            _vertices[_vertexIndex + 2] = new Vertex()
            {
                position = new Vector3(element.positon.X + element.size.X, element.positon.Y + element.size.Y, 0),
                color = element.color,
                uvs = element.uvs[2],
                textureIndex = textureIndex,
            };
            // top right
            _vertices[_vertexIndex + 3] = new Vertex()
            {
                position = new Vector3(element.positon.X + element.size.X, element.positon.Y, 0),
                color = element.color,
                uvs = element.uvs[3],
                textureIndex = textureIndex,
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
            int size = _vertexIndex * Marshal.SizeOf<Vertex>();
            _mesh.BufferSubData(_vertices, size);
        }


        public void Flush()
        {
            _uiBatchShader.Bind();
            for (uint i = 0; i < _textureSlotIndex; i++)
            {
                _textures[i]?.Bind(i);
            }
            //_textures[0].Bind(0);

            _mesh.Render(_indexCount);
            _indexCount = 0;
            _textureSlotIndex = 1;
        }


        #region IDisposable

        private bool _disposedValue;

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

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
