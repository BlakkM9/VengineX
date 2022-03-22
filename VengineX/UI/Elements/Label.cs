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
    /// <summary>
    /// UIElement that is used to render text.<br/>
    /// No background, just the text itself.
    /// </summary>
    public class Label : Element
    {
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

        
        private Mesh<UIVertex> _textMesh;


        public Label(Element parent, BitmapFont font, string text, float textSize, Vector4 color) : this(parent)
        {
            _textSize = textSize;
            _font = font;
            _text = text;
            _color = color;

            UpdateTextMesh();
        }


        /// <summary>
        /// Constructor for <see cref="UISerializer"/>.
        /// </summary>
        public Label(Element parent) : base(parent)
        {
            _textMesh = new Mesh<UIVertex>(BufferUsageHint.DynamicDraw, BufferUsageHint.DynamicDraw);
        }


        /// <summary>
        /// Updates the text mesh to current font and test. Also updates the width.
        /// </summary>
        private void UpdateTextMesh()
        {
            _font.CreateMeshData(_text, out UnmanagedArray<UIVertex> vertices, out UnmanagedArray<uint> indices);
            _textMesh.BufferData(vertices);
            _textMesh.BufferData(indices);
            vertices.Free();
            indices.Free();
            Width = _font.CalculateWidth(_text, TextSize);
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void Render()
        {
            if (Visible)
            {
                CalculateModelMatrix();

                Canvas.BitmapFontShader.Bind();
                _font.TextureAtlas.Bind();

                Canvas.FontProjectionMatrixUniform.SetMat4(ref ParentCanvas.ProjectionMatrix);
                Canvas.FontViewMatrixUniform.SetMat4(ref ParentCanvas.ViewMatrix);
                Canvas.FontModelMatrixUniform.SetMat4(ref ModelMatrix);
                Canvas.FontColorUniform.Set4(ref _color);

                _textMesh.Render();
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
                ModelMatrix = Matrix4.CreateScale(Height / _font.Size, Height / _font.Size, 0);
                ModelMatrix *= Matrix4.CreateTranslation(AbsolutePosition.X, -(AbsolutePosition.Y + Height), 0);
            }
        }
    }
}
