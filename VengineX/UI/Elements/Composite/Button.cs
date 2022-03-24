using OpenTK.Mathematics;
using VengineX.UI.Elements.Basic;



namespace VengineX.UI.Elements.Composite
{
    public partial class Button : CompositeElement
    {
        public Vector4 BackgroundColor { set => _image.Color = value; }
        public Vector4 HoverColor { set => _hoverImage.Color = value; }
        public Vector4 PressedColor { set => _pressedImage.Color = value; }
        public string Text { set => _label.Text = value; }
        public Vector4 TextColor { set => _label.Color = value; }
        public Button(Element? parent) : base(parent) { }
    }
}