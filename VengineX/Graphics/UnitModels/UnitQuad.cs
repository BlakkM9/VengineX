using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using VengineX.Graphics.Meshes;
using VengineX.Graphics.Vertices;

namespace VengineX.Graphics.UnitModels
{
    /// <summary>
    /// A simple quad from -1 to 1.<br/>
    /// Might be used for postprocessing.
    /// </summary>
    public static class UnitQuad
    {
        private static readonly float[] VERTICES = new float[] {
            // position         // uvs
            -1.0f,  1.0f, 0.0f, 0.0f, 1.0f,
            -1.0f, -1.0f, 0.0f, 0.0f, 0.0f,
             1.0f,  1.0f, 0.0f, 1.0f, 1.0f,
             1.0f, -1.0f, 0.0f, 1.0f, 0.0f,
        };


        private static uint _vao;
        private static uint _vbo;
        private static bool _initialized = false;


        internal static void Initialize()
        {
            if (_initialized) { return; }
            _initialized = true;
            int stride = sizeof(float) * 5;

            GL.CreateVertexArrays(1, out _vao);
            GL.CreateBuffers(1, out _vbo);

            GL.NamedBufferData(_vbo, VERTICES.Length * stride, VERTICES, BufferUsageHint.StaticDraw);

            // Position
            GL.EnableVertexArrayAttrib(_vao, 0);
            GL.VertexArrayAttribBinding(_vao, 0, 0);
            GL.VertexArrayAttribFormat(_vao, 0, 3, VertexAttribType.Float, false, 0);

            // UVs
            GL.EnableVertexArrayAttrib(_vao, 1);
            GL.VertexArrayAttribBinding(_vao, 1, 0);
            GL.VertexArrayAttribFormat(_vao, 1, 2, VertexAttribType.Float, false, 3 * sizeof(float));


            GL.VertexArrayVertexBuffer(_vao, 0, _vbo, IntPtr.Zero, stride);
        }


        public static void Render()
        {
            GL.BindVertexArray(_vao);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
        }


        internal static void Dispose()
        {
            if (!_initialized) { return; }
            _initialized = false;

            GL.DeleteVertexArray(_vao);
            GL.DeleteBuffer(_vbo);
        }
    }
}
