using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VengineX.Debugging.Logging;
using VengineX.Graphics.Rendering.Vertices;
using VengineX.Utils;

namespace VengineX.Graphics.Rendering
{
    // TODO save indices and vertices in a single buffer (but how to get offset in c#?)
    /// <summary>
    /// Class representing a mesh.
    /// </summary>
    /// <typeparam name="T">
    /// Vertex the mesh should use.<br/>
    /// VertexAttribPointers are determined automatically via reflection.<br/>
    /// Vertex is a struct holding <see cref="float"/>, <see cref="Vector2"/>, <see cref="Vector3"/>, or <see cref="Vector4"/> <b>fields</b>.
    /// </typeparam>
    public class Mesh<T> : IDisposable, IRenderable where T : unmanaged
    {
        /// <summary>
        /// Array holding all currently allowed types for vertex attributes within the provided vertex struct typeparam/>
        /// </summary>
        public static readonly Type[] ALLOWED_VERTEX_FIELD_TYPES =
        {
            typeof(float),
            typeof(Vector2),
            typeof(Vector3),
            typeof(Vector4),
        };

        private uint _vbo;
        private uint _vao;
        private uint _ebo;

        /// <summary>
        /// ModelMatrix of this mesh.
        /// </summary>
        public ref Matrix4 ModelMatrix
        {
            get { return ref _modelMatrix; }
        }
        private Matrix4 _modelMatrix;

        /// <summary>
        /// The buffer usage hint for the vertex buffer of this mesh.
        /// </summary>
        public BufferUsageHint VertexBufferUsage { get; }

        /// <summary>
        /// The buffer usage hint for the index buffer of this mesh.
        /// </summary>
        public BufferUsageHint IndexBufferUsage { get; }


        /// <summary>
        /// Position of this mesh.<br/>
        /// Changing will update the <see cref="ModelMatrix"/>.
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                _modelMatrix = Matrix4.CreateTranslation(Position);
            }
        }
        private Vector3 _position;


        /// <summary>
        /// Amount of indices in this mesh.
        /// </summary>
        private int _numIndices;


        /// <summary>
        /// Creates a new mesh.
        /// </summary>
        /// <param name="position">Position of the mesh</param>
        /// <param name="bufferUsage"><see cref="BufferUsageHint"/> for the vertex- and indexbuffer of this mesh.</param>
        /// <param name="vertices">Vertices of this mesh.</param>
        /// <param name="indices">Indices of this mesh.</param>
        public Mesh(Vector3 position, BufferUsageHint vertexBufferUsage, BufferUsageHint indexBufferUsage, T[] vertices, uint[] indices)
        {
            _modelMatrix = Matrix4.CreateTranslation(Position);

            Position = position;
            VertexBufferUsage = vertexBufferUsage;
            IndexBufferUsage = indexBufferUsage;

            SetupMesh(vertices, indices);
        }


        /// <summary>
        /// Creates a new mesh.
        /// </summary>
        /// <param name="position">Position of the mesh</param>
        /// <param name="bufferUsage"><see cref="BufferUsageHint"/> for the vertex- and indexbuffer of this mesh.</param>
        /// <param name="vertices">Vertices of this mesh.</param>
        /// <param name="indices">Indices of this mesh.</param>
        public Mesh(Vector3 position, BufferUsageHint vertexBufferUsage, BufferUsageHint indexBufferUsage, UnmanagedArray<T> vertices, UnmanagedArray<uint> indices)
        {
            _modelMatrix = Matrix4.CreateTranslation(Position);

            Position = position;
            VertexBufferUsage = vertexBufferUsage;
            IndexBufferUsage = indexBufferUsage;

            SetupMesh(vertices, indices);
        }


        private void SetupMesh(T[] vertices, uint[] indices)
        {
            _numIndices = indices.Length;

            GL.CreateVertexArrays(1, out _vao);
            GL.CreateBuffers(1, out _vbo);
            GL.CreateBuffers(1, out _ebo);

            GL.NamedBufferData(_vbo, vertices.Length * Marshal.SizeOf(typeof(T)), vertices, VertexBufferUsage);
            GL.NamedBufferData(_ebo, indices.Length * sizeof(uint), indices, VertexBufferUsage);

            SetupAttribPointers();
        }


        private void SetupMesh(UnmanagedArray<T> vertices, UnmanagedArray<uint> indices)
        {
            _numIndices = indices.Length;

            GL.CreateVertexArrays(1, out _vao);
            GL.CreateBuffers(1, out _vbo);
            GL.CreateBuffers(1, out _ebo);

            GL.NamedBufferData(_vbo, vertices.Length * Marshal.SizeOf(typeof(T)), vertices.Pointer, VertexBufferUsage);
            GL.NamedBufferData(_ebo, indices.Length * sizeof(uint), indices.Pointer, IndexBufferUsage);

            SetupAttribPointers();
        }


        private void SetupAttribPointers()
        {
            Type vertexType = typeof(T);
            uint location = 0;
            bool normalized = false;
            VertexAttribType type = VertexAttribType.Float;
            int stride = Marshal.SizeOf(typeof(T));
            uint offset = 0;

            foreach (FieldInfo field in vertexType.GetFields())
            {
                if (!ALLOWED_VERTEX_FIELD_TYPES.Contains(field.FieldType))
                {
                    Logger.Log(Severity.Fatal, "Mesh Vertex fields do currently not support " + field.FieldType.FullName + "!");
                }

                int fieldSize = Marshal.SizeOf(field.FieldType);
                int fieldCount = fieldSize / sizeof(float);

                GL.EnableVertexArrayAttrib(_vao, location);
                GL.VertexArrayAttribBinding(_vao, location, 0);
                GL.VertexArrayAttribFormat(_vao, location, fieldCount, type, normalized, offset);

                location++;
                offset += (uint)fieldSize;
            }

            GL.VertexArrayVertexBuffer(_vao, 0, _vbo, IntPtr.Zero, stride);
            GL.VertexArrayElementBuffer(_vao, _ebo);
        }


        /// <summary>
        /// Binds this meshs VertexArrayObject and calls GL.DrawElements.
        /// </summary>
        public void Render()
        {
            GL.BindVertexArray(_vao);
            GL.DrawElements(PrimitiveType.Triangles, _numIndices, DrawElementsType.UnsignedInt, 0);
        }


        /// <summary>
        /// Updates the vertices for this mesh.
        /// </summary>
        public void BufferData(T[] vertices)
        {
            GL.NamedBufferData(_vbo, vertices.Length * Marshal.SizeOf(typeof(T)), vertices, VertexBufferUsage);
        }


        /// <summary>
        /// Updates the indices for this mesh.
        /// </summary>
        public void BufferData(uint[] indices)
        {
            _numIndices = indices.Length;
            GL.NamedBufferData(_ebo, indices.Length * sizeof(uint), indices, IndexBufferUsage);
        }


        /// <summary>
        /// Updates the vertices for this mesh.
        /// </summary>
        public void BufferData(UnmanagedArray<T> vertices)
        {
            GL.NamedBufferData(_vbo, vertices.Length * Marshal.SizeOf(typeof(T)), vertices.Pointer, VertexBufferUsage);
        }


        /// <summary>
        /// Updates the indices for this mesh.
        /// </summary>
        public void BufferData(UnmanagedArray<uint> indices)
        {
            _numIndices = indices.Length;
            GL.NamedBufferData(_ebo, indices.Length * sizeof(uint), indices.Pointer, IndexBufferUsage);
        }


        public void BufferSubData(T[] vertices, int size)
        {
            GL.NamedBufferSubData(_vbo, IntPtr.Zero, size, vertices);
        }


        public void BufferSubData(UnmanagedArray<T> vertices, int size)
        {
            GL.NamedBufferSubData(_vbo, IntPtr.Zero, size, vertices.Pointer);
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
                    GL.DeleteVertexArray(_vao);
                    GL.DeleteBuffer(_vbo);
                    GL.DeleteBuffer(_ebo);
                }

                _disposedValue = true;
            }
        }

        /// <summary>
        /// Disposable pattern.
        /// </summary>
        ~Mesh()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

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
