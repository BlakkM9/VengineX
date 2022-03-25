using OpenTK.Mathematics;
using VengineX.Graphics.Rendering.Batching;
using VengineX.Resources;
using VengineX.UI.Fonts;

namespace VengineX.UI.Elements.Basic
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
        private float _textSize;

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

        /// <summary>
        /// The font that is used for this label
        /// </summary>
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


        /// <summary>
        /// Creates a new label.
        /// </summary>
        public Label(Element parent) : base(parent)
        {
            // Fallback font
            _font = ResourceManager.GetResource<BitmapFont>("font.opensans");

            UpdateTextMesh();
        }


        /// <summary>
        /// Updates the text mesh to current font and test. Also updates the width.
        /// </summary>
        private void UpdateTextMesh()
        {
            Width = _font.CalculateWidth(_text, TextSize);
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override IEnumerable<QuadVertex> EnumerateQuads()
        {
            return _font.CreateQuads(
                Text,
                new Vector2(AbsolutePosition.X, Canvas.Height - AbsolutePosition.Y - Height),
                TextSize, Color);
        }
    }
}
