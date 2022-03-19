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
using VengineX.UI.Serialization;
using VengineX.Utils;

namespace VengineX.UI.Elements
{

    // TODO: Label layout is not properly updated when text changed.
    // The workaround is to call update twice on the parent element.
    // but there is proabably some edge case that needs to be fixed.
    public class Label : UIElement, IDisposable
    {
        /// <summary>
        /// The shader that is used to render text.
        /// </summary>
        public static Shader BitmapFontShader { get; private set; }

        /// <summary>
        /// Project matrix uniform location of the font shader.
        /// </summary>
        public static int ProjectionMatrixLocation { get; private set; }

        /// <summary>
        /// Model matrix uniform location of the font shader.
        /// </summary>
        public static int ModelMatrixLocation { get; private set; }

        /// <summary>
        /// View matrix uniform location of the font shader.
        /// </summary>
        public static int ViewMatrixLocation { get; private set; }

        /// <summary>
        /// uColor uniform location of the font shader.
        /// </summary>
        public static int ColorLocation { get; private set; }

        /// <summary>
        /// The text color of this label
        /// </summary>
        public Vector4 Color { get => _color; set => _color = value; }
        private Vector4 _color = Vector4.One;

        /// <summary>
        /// Size of the text.
        /// </summary>
        public float TextSize
        {
            get => _textSize;
            set
            {
                _textSize = value;
                Size = new Vector2(_font.CalculateWidth(_text, _textSize), _textSize);
            }
        }

        private float _textSize = 20;

        /// <summary>
        /// Sets and gets the text of this label.
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                UpdateTextMesh();
            }
        }
        private string _text = string.Empty;

        public BitmapFont? Font
        {
            get => _font;
            set
            {
                _font = value;
                UpdateTextMesh();
            }
        }
        private BitmapFont? _font = null;

        
        private Mesh<UIVertex>? _textMesh = null;


        public Label(UIElement parent, BitmapFont font, string text, float textSize, Vector4 color) : this(parent)
        {
            TextSize = textSize;
            _font = font;
            _text = text;
            _color = color;

            UpdateTextMesh();
        }


        /// <summary>
        /// Constructor for <see cref="UISerializer"/>.
        /// </summary>
        public Label(UIElement parent) : base(parent) {
            // Lazy shader initialization
            if (BitmapFontShader == null)
            {
                BitmapFontShader = ResourceManager.GetResource<Shader>("shader.ui.bmpfont");
                ProjectionMatrixLocation = BitmapFontShader.GetUniformLocation("P");
                ModelMatrixLocation = BitmapFontShader.GetUniformLocation("M");
                ViewMatrixLocation = BitmapFontShader.GetUniformLocation("V");
                ColorLocation = BitmapFontShader.GetUniformLocation("uColor");
            }

        }

        //public override void UpdateLayout()
        //{
        //    Width = _font.CalculateWidth(_text, TextSize);
        //    base.UpdateLayout();
        //}


        /// <summary>
        /// Updates the text mesh to current font and test. Also updates the width.
        /// </summary>
        private void UpdateTextMesh()
        {
            _font.CreateMeshData(_text, out UnmanagedArray<UIVertex> vertices, out UnmanagedArray<uint> indices);
            _textMesh = new Mesh<UIVertex>(Vector3.Zero, BufferUsageHint.DynamicDraw, vertices, indices);
            vertices.Free();
            indices.Free();
            Width = _font.CalculateWidth(_text, TextSize);

            // HACK: only for update layout only needed to called once after mesh changed.
            Parent.UpdateLayout();
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void Render()
        {
            if (Visible)
            {
                BitmapFontShader.Bind();
                _font.TextureAtlas.Bind();

                BitmapFontShader.SetUniformMat4(ProjectionMatrixLocation, ref ParentCanvas.ProjectionMatrix);
                BitmapFontShader.SetUniformMat4(ViewMatrixLocation, ref ParentCanvas.ViewMatrix);
                BitmapFontShader.SetUniformMat4(ModelMatrixLocation, ref ModelMatrix);
                BitmapFontShader.SetUniformVec4(ColorLocation, ref _color);

                _textMesh.Render();
                _font.TextureAtlas.Unbind();
            }


            base.Render();
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
                ModelMatrix *= Matrix4.CreateTranslation(AbsolutePosition.X, -(AbsolutePosition.Y + Height), 0);
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
                    // Dispose managed state (managed objects)
                    _textMesh.Dispose();
                }

                // Free unmanaged resources (unmanaged objects) and override finalizer
                // Set large fields to null
                _disposedValue = true;
            }
        }

        // // Override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Label()
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
