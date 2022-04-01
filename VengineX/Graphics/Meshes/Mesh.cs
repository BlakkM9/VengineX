using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Reflection;
using System.Runtime.InteropServices;
using VengineX.Debugging.Logging;
using VengineX.Utils;

namespace VengineX.Graphics.Meshes
{

    /// <summary>
    /// Class representing a mesh.
    /// </summary>
    /// <typeparam name="T">
    /// Vertex the mesh should use.<br/>
    /// VertexAttribPointers are determined automatically via reflection.<br/>
    /// Vertex is a struct holding <see cref="float"/>, <see cref="Vector2"/>, <see cref="Vector3"/>, or <see cref="Vector4"/> <b>fields</b>.
    /// </typeparam>
    public class Mesh<T> : MeshBase where T : unmanaged
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


        /// <summary>
        /// Creates a new empty mesh.
        /// </summary>
        /// <param name="vertexBufferUsage"><see cref="BufferUsageHint"/> for the vertex buffer of this mesh.</param>
        /// <param name="indexBufferUsage"><see cref="BufferUsageHint"/> for the index buffer of this mesh.</param>
        public Mesh(BufferUsageHint vertexBufferUsage, BufferUsageHint indexBufferUsage) : this(vertexBufferUsage, indexBufferUsage, null, null) { }


        /// <summary>
        /// Creates a new mesh.
        /// </summary>
        /// <param name="vertexBufferUsage"><see cref="BufferUsageHint"/> for the vertex buffer of this mesh.</param>
        /// <param name="indexBufferUsage"><see cref="BufferUsageHint"/> for the index buffer of this mesh.</param>
        /// <param name="vertices">Vertices of this mesh. Empty initialization if null.</param>
        /// <param name="indices">Indices of this mesh. Empty initialization if null.</param>
        public Mesh(BufferUsageHint vertexBufferUsage, BufferUsageHint indexBufferUsage, UnmanagedArray<T> vertices, UnmanagedArray<uint> indices)
            : base(vertexBufferUsage, indexBufferUsage)
        {

            SetupMesh(vertices, indices);
        }


        /// <summary>
        /// Creates a new mesh.
        /// </summary>
        /// <param name="vertexBufferUsage"><see cref="BufferUsageHint"/> for the vertex buffer of this mesh.</param>
        /// <param name="indexBufferUsage"><see cref="BufferUsageHint"/> for the index buffer of this mesh.</param>
        /// <param name="vertices">Vertices of this mesh. Empty initialization if null.</param>
        /// <param name="indices">Indices of this mesh. Empty initialization if null.</param>
        public Mesh(BufferUsageHint vertexBufferUsage, BufferUsageHint indexBufferUsage, T[]? vertices, uint[]? indices)
            : base(vertexBufferUsage, indexBufferUsage)
        {
            SetupMesh(vertices, indices);
        }


        /// <summary>
        /// Creates index and vertex buffers for the given vertices and indices.
        /// </summary>
        private void SetupMesh(T[]? vertices, uint[]? indices)
        {
            GL.CreateVertexArrays(1, out _vao);
            GL.CreateBuffers(1, out _vbo);
            GL.CreateBuffers(1, out _ebo);

            if (vertices != null)
            {
                GL.NamedBufferData(_vbo, vertices.Length * Marshal.SizeOf(typeof(T)), vertices, VertexBufferUsage);
            }

            if (indices != null)
            {
                IndexCount = indices.Length;
                GL.NamedBufferData(_ebo, indices.Length * sizeof(uint), indices, VertexBufferUsage);
            }

            SetupAttribPointers();
        }


        /// <summary>
        /// Creates index and vertex buffers for the given vertices and indices.
        /// </summary>
        private void SetupMesh(UnmanagedArray<T> vertices, UnmanagedArray<uint> indices)
        {
            IndexCount = indices.Length;

            GL.CreateVertexArrays(1, out _vao);
            GL.CreateBuffers(1, out _vbo);
            GL.CreateBuffers(1, out _ebo);

            GL.NamedBufferData(_vbo, vertices.Length * Marshal.SizeOf(typeof(T)), vertices.Pointer, VertexBufferUsage);
            GL.NamedBufferData(_ebo, indices.Length * sizeof(uint), indices.Pointer, IndexBufferUsage);

            SetupAttribPointers();
        }


        /// <summary>
        /// Sets up the vertex attributes, based on provided vertex type <typeparamref name="T"/>.
        /// </summary>
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
        /// <inheritdoc/>
        /// </summary>
        public override void Render()
        {
            GL.BindVertexArray(_vao);
            GL.DrawElements(PrimitiveType.Triangles, IndexCount, DrawElementsType.UnsignedInt, 0);
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void Render(int indexCount)
        {
            GL.BindVertexArray(_vao);
            GL.DrawElements(BeginMode.Triangles, indexCount, DrawElementsType.UnsignedInt, 0);
        }


        /// <summary>
        /// Updates the vertices for this mesh (setting buffer size to required size).
        /// </summary>
        public void BufferVertices(T[] vertices)
        {
            GL.NamedBufferData(_vbo, vertices.Length * Marshal.SizeOf(typeof(T)), vertices, VertexBufferUsage);
        }


        /// <summary>
        /// Updates the vertices and reserves given size for the buffer.
        /// </summary>
        public void BufferVertices(T[]? vertices, int size)
        {
            GL.NamedBufferData(_vbo, size, vertices, VertexBufferUsage);
        }


        /// <summary>
        /// Updates the indices for this mesh (setting buffer size to required size).
        /// </summary>
        public void BufferIndices(uint[] indices)
        {
            IndexCount = indices.Length;
            GL.NamedBufferData(_ebo, indices.Length * sizeof(uint), indices, IndexBufferUsage);
        }


        /// <summary>
        /// Updates the vertices and reserves given size for the buffer.
        /// </summary>
        public void BufferVertices(UnmanagedArray<T> vertices)
        {
            GL.NamedBufferData(_vbo, vertices.Length * Marshal.SizeOf(typeof(T)), vertices.Pointer, VertexBufferUsage);
        }


        /// <summary>
        /// Updates the indices for this mesh.
        /// </summary>
        public void BufferIndices(UnmanagedArray<uint> indices)
        {
            IndexCount = indices.Length;
            GL.NamedBufferData(_ebo, indices.Length * sizeof(uint), indices.Pointer, IndexBufferUsage);
        }


        /// <summary>
        /// Buffers vertex subdata to this mesh.
        /// </summary>
        public void BufferSubData(T[] vertices, int size)
        {
            GL.NamedBufferSubData(_vbo, IntPtr.Zero, size, vertices);
        }


        /// <summary>
        /// Buffers vertex subdata to this mesh.
        /// </summary>
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
        public override void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
