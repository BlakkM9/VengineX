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
    public class Label : EventDrivenUIElement, IDisposable
    {
        public static Shader BitmapFontShader { get; private set; }
        public static int ProjectionMatrixLocation { get; private set; }
        public static int ModelMatrixLocation { get; private set; }
        public static int ViewMatrixLocation { get; private set; }
        public static int ColorLocation { get; private set; }

        public Vector4 Color { get => _color; set => _color = value; }
        protected Vector4 _color;

        public float Size { get; }

        /// <summary>
        /// Sets and gets the text of this label.
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                _font.CreateMeshData(_text, out UnmanagedArray<UIVertex> vertices, out UnmanagedArray<uint> indices);
                _textMesh.UpdateVertices(vertices, indices);
                vertices.Free();
                indices.Free();
                Width = _font.CalculateWidth(_text, Size);
            }
        }
        protected string _text;

        private Mesh<UIVertex> _textMesh;
        private BitmapFont _font;


        public Label(BitmapFont font, string text, float x, float y, float size, Vector4 color)
            : base(x, y, font.CalculateWidth(text, size), size)
        {
            // Lazy shader initialization
            if (BitmapFontShader == null)
            {
                BitmapFontShader = ResourceManager.GetResource<Shader>("shader.ui.bmpfont");
                ProjectionMatrixLocation = BitmapFontShader.GetUniformLocation("P");
                ModelMatrixLocation = BitmapFontShader.GetUniformLocation("M");
                ViewMatrixLocation = BitmapFontShader.GetUniformLocation("V");
                ColorLocation = BitmapFontShader.GetUniformLocation("uColor");
            }

            Size = size;
            _font = font;
            _text = text;
            _font.CreateMeshData(_text, out UnmanagedArray<UIVertex> vertices, out UnmanagedArray<uint> indices);
            _textMesh = new Mesh<UIVertex>(Vector3.Zero, BufferUsageHint.DynamicDraw, vertices, indices);
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
            if (ParentCanvas != null)
            {
                BitmapFontShader.Bind();
                _font.TextureAtlas.Bind();

                BitmapFontShader.SetUniformMat4(ProjectionMatrixLocation, ref ParentCanvas.ProjectionMatrix);
                BitmapFontShader.SetUniformMat4(ViewMatrixLocation, ref ParentCanvas.ViewMatrix);
                BitmapFontShader.SetUniformMat4(ModelMatrixLocation, ref ModelMatrix);
                BitmapFontShader.SetUniformVec4(ColorLocation, ref _color);

                _textMesh.Render();
            }
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


        #region IDisposable

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

        #endregion
    }
}
