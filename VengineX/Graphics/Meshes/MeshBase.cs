using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Graphics.Meshes
{
    // TODO save indices and vertices in a single buffer (but how to get offset in c#?)
    /// <summary>
    /// Base class for mesh.
    /// </summary>
    public abstract class MeshBase : IDisposable
    {

        /// <summary>
        /// Amount of indices in this mesh.
        /// </summary>
        protected int IndexCount { get; set; }

        /// <summary>
        /// Vertex Buffer Object
        /// </summary>
        protected uint _vbo;

        /// <summary>
        /// Vertex Array Object
        /// </summary>
        protected uint _vao;

        /// <summary>
        /// Element Buffer Object
        /// </summary>
        protected uint _ebo;

        /// <summary>
        /// The buffer usage hint for the vertex buffer of this mesh.
        /// </summary>
        public BufferUsageHint VertexBufferUsage { get; }


        /// <summary>
        /// The buffer usage hint for the index buffer of this mesh.
        /// </summary>
        public BufferUsageHint IndexBufferUsage { get; }


        public MeshBase(BufferUsageHint vertexBufferUsage, BufferUsageHint indexBufferUsage)
        {
            VertexBufferUsage = vertexBufferUsage;
            IndexBufferUsage = indexBufferUsage;
        }


        /// <summary>
        /// Binds this meshs VertexArrayObject and calls GL.DrawElements.
        /// </summary>
        public abstract void Render();


        /// <summary>
        /// Renders the elements of this mesh until the given index.
        /// </summary>
        public abstract void Render(int indexCount);


        /// <summary>
        /// IDisposable
        /// </summary>
        public abstract void Dispose();
    }
}
