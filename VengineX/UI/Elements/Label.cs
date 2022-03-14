using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering;
using VengineX.Graphics.Rendering.Shaders;
using VengineX.Graphics.Rendering.Vertices;
using VengineX.Resources;
using VengineX.UI.Fonts;
using VengineX.Utils;
using VengineX.Wrappers.FreeType;

namespace VengineX.UI.Elements
{
    /// <summary>
    /// Label with text.
    /// </summary>
    public class Label : UIElement, IDisposable
    {
        public static Shader BitmapFontShader { get; private set; }
        public static int ProjectionMatrixLocation { get; private set; }
        public static int ModelMatrixLocation { get; private set; }
        public static int ViewMatrixLocation { get; private set; }
        public static int ColorLocation { get; private set; }

        public Vector4 Color { get => _color; set => _color = value; }
        protected Vector4 _color;

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                _font.CreateMeshData(_text, out UnmanagedArray<UIVertex> vertices, out UnmanagedArray<uint> indices);
                _textMesh.UpdateVertices(ref vertices, ref indices);
                vertices.Free();
                indices.Free();
                Width = _font.CalculateWidth(_text);
            }
        }
        protected string _text;


        private Mesh<UIVertex> _textMesh;
        private BitmapFont _font;

        public Label(BitmapFont font, string text, float x, float y, float size, Vector4 color)
            : base(x, y, font.CalculateWidth(text), size)
        {
            // Lazy shader initialization
            if (BitmapFontShader == null)
            {
                BitmapFontShader = ResourceManager.GetResource<Shader>("shader.bmpfont");
                ProjectionMatrixLocation = BitmapFontShader.GetUniformLocation("P");
                ModelMatrixLocation = BitmapFontShader.GetUniformLocation("M");
                ViewMatrixLocation = BitmapFontShader.GetUniformLocation("V");
                ColorLocation = BitmapFontShader.GetUniformLocation("uColor");
            }

            _font = font;
            _text = text;
            _font.CreateMeshData(_text, out UnmanagedArray<UIVertex> vertices, out UnmanagedArray<uint> indices);
            _textMesh = new Mesh<UIVertex>(Vector3.Zero, BufferUsageHint.StaticDraw, vertices, indices);
            vertices.Free();
            indices.Free();
            _color = color;

            CalculateModelMatrix();
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void Render()
        {
            BitmapFontShader.Bind();
            _font.TextureAtlas.Bind();

            BitmapFontShader.SetUniformMat4(ProjectionMatrixLocation, ref ParentCanvas.ProjectionMatrix);
            BitmapFontShader.SetUniformMat4(ViewMatrixLocation, ref ParentCanvas.ViewMatrix);
            BitmapFontShader.SetUniformMat4(ModelMatrixLocation, ref ModelMatrix);
            BitmapFontShader.SetUniformVec4(ColorLocation, ref _color);

            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            _textMesh.Render();
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void CalculateModelMatrix()
        {
            if (_font != null)
            {
                // Update model matrix
                ModelMatrix = Matrix4.Identity;
                ModelMatrix *= Matrix4.CreateScale(Height / _font.Size, Height / _font.Size, 0);
                ModelMatrix *= Matrix4.CreateTranslation(X, -(Y + Height), 0);
            }
        }


        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _textMesh?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Text()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
