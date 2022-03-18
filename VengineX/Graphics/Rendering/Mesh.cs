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
    /// <summary>
    /// 
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
        /// The buffer usage hint for this mesh.
        /// </summary>
        public BufferUsageHint BufferUsage { get; }

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


        public Mesh(Vector3 position, BufferUsageHint bufferUsage, T[] vertices, uint[] indices)
        {
            _modelMatrix = Matrix4.Identity;
            Position = position;
            BufferUsage = bufferUsage;

            SetupMesh(vertices, indices);
        }


        public Mesh(Vector3 position, BufferUsageHint bufferUsage, UnmanagedArray<T> vertices, UnmanagedArray<uint> indices)
        {
            _modelMatrix = Matrix4.Identity;
            Position = position;
            BufferUsage = bufferUsage;

            SetupMesh(vertices, indices);
        }


        private void SetupMesh(T[] vertices, uint[] indices)
        {
            _numIndices = indices.Length;

            GL.CreateVertexArrays(1, out _vao);
            GL.CreateBuffers(1, out _vbo);
            GL.CreateBuffers(1, out _ebo);

            GL.NamedBufferData(_vbo, vertices.Length * Marshal.SizeOf(typeof(T)), vertices, BufferUsage);
            GL.NamedBufferData(_ebo, indices.Length * sizeof(uint), indices, BufferUsage);

            SetupAttribPointers();
        }


        private void SetupMesh(UnmanagedArray<T> vertices, UnmanagedArray<uint> indices)
        {
            _numIndices = indices.Length;

            GL.CreateVertexArrays(1, out _vao);
            GL.CreateBuffers(1, out _vbo);
            GL.CreateBuffers(1, out _ebo);

            GL.NamedBufferData(_vbo, vertices.Length * Marshal.SizeOf(typeof(T)), vertices.Pointer, BufferUsage);
            GL.NamedBufferData(_ebo, indices.Length * sizeof(uint), indices.Pointer, BufferUsage);

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
            //GL.MultiDrawElements(PrimitiveType.Triangles, new int[] { _numIndices }, DrawElementsType.UnsignedInt, new int[] { 0 }, 1);
            //GL.BindVertexArray(0);
        }


        /// <summary>
        /// Updates the vertices and indices for this mesh
        /// </summary>
        public void UpdateVertices(T[] vertices, uint[] indices)
        {
            _numIndices = indices.Length;

            GL.NamedBufferData(_vbo, vertices.Length * Marshal.SizeOf(typeof(T)), vertices, BufferUsage);
            GL.NamedBufferData(_ebo, indices.Length * sizeof(uint), indices, BufferUsage);
        }


        /// <summary>
        /// Updates the vertices and indices for this mesh
        /// </summary>
        public void UpdateVertices(UnmanagedArray<T> vertices, UnmanagedArray<uint> indices)
        {
            _numIndices = indices.Length;

            GL.NamedBufferData(_vbo, vertices.Length * Marshal.SizeOf(typeof(T)), vertices.Pointer, BufferUsage);
            GL.NamedBufferData(_ebo, indices.Length * sizeof(uint), indices.Pointer, BufferUsage);
        }


        #region IDisposable

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    GL.DeleteBuffer(_vbo);
                    GL.DeleteBuffer(_vao);
                    GL.DeleteBuffer(_ebo);
                }

                //Vertices = null;
                //Indices = null;
                _disposedValue = true;
            }
        }


        ~Mesh()
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

        #endregion
    }
}
