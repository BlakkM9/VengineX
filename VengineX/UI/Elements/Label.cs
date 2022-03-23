using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering;
using VengineX.Graphics.Rendering.Batching;
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

        public BitmapFont Font
        {
            get => _font;
            set
            {
                _font = value;
                UpdateTextMesh();
            }
        }
        private BitmapFont _font;


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
            // Fallback font
            _font = ResourceManager.GetResource<BitmapFont>("font.opensans");
        }


        /// <summary>
        /// Updates the text mesh to current font and test. Also updates the width.
        /// </summary>
        private void UpdateTextMesh()
        {
            Width = _font.CalculateWidth(_text, TextSize);
        }


        public override IEnumerable<UIBatchQuad> EnumerateQuads()
        {
            return _font.CreateQuads(
                Text,
                new Vector2(AbsolutePosition.X, Canvas.Height - AbsolutePosition.Y - Height),
                TextSize, Color);
        }
    }
}
