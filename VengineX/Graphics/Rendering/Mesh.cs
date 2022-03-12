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
    public class Mesh<T> : IDisposable, IRenderable
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

        private int _vbo;
        private int _vao;
        private int _ebo;

        /// <summary>
        /// ModelMatrix of this mesh.
        /// </summary>
        public ref Matrix4 ModelMatrix
        {
            get { return ref _modelMatrix; }
        }
        private Matrix4 _modelMatrix;


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
        /// Array holding all vertices of this mesh.
        /// </summary>
        public PBRVertex[] Vertices { get; private set; }


        /// <summary>
        /// Array holding all indices of this mesh.
        /// </summary>
        public uint[] Indices { get; private set; }


        public Mesh(Vector3 position, PBRVertex[] vertices, uint[] indices)
        {
            _modelMatrix = Matrix4.Identity;

            Position = position;
            Vertices = vertices;
            Indices = indices;

            SetupMesh();
        }


        private void SetupMesh()
        {
            // Create VertexBufferObject
            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * Marshal.SizeOf(typeof(PBRVertex)), Vertices, BufferUsageHint.StaticDraw);

            // Create VertexArrayObject
            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

            // Create ElementBufferObject
            // ebo is a property of vao so vao needs to be binded when we bind ebo
            _ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);


            SetupAttribPointers();
        }


        private void SetupAttribPointers()
        {
            Type vertexType = typeof(T);
            int location = 0;
            bool normalized = false;
            VertexAttribPointerType type = VertexAttribPointerType.Float;
            int stride = Marshal.SizeOf(typeof(T));
            int offset = 0;

            foreach (FieldInfo field in vertexType.GetFields())
            {
                if (!ALLOWED_VERTEX_FIELD_TYPES.Contains(field.FieldType))
                {
                    Logger.Log(Severity.Fatal, "Mesh Vertex fields do currently not support " + field.FieldType.FullName + "!");
                }

                int fieldSize = Marshal.SizeOf(field.FieldType);
                int fieldCount = fieldSize / sizeof(float);

                GL.VertexAttribPointer(location, fieldCount, type, normalized, stride, offset);
                GL.EnableVertexAttribArray(location);

                location++;
                offset += fieldSize;
            }

            // Unbind vao
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }


        /// <summary>
        /// Binds this meshs VertexArrayObject and calls GL.DrawElements.
        /// </summary>
        public void Render()
        {
            GL.BindVertexArray(_vao);
            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
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

                Vertices = null;
                Indices = null;
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
