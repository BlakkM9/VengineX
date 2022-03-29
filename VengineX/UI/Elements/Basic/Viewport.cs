using VengineX.Graphics.Rendering.Renderers;

namespace VengineX.UI.Elements.Basic
{
    public class Viewport : Element
    {
        public Viewport(Element? parent) : base(parent)
        {
        }

        protected override IEnumerable<Element> AllChildren()
        {
            return base.AllChildren();
        }


        public override IEnumerable<QuadVertex> EnumerateQuads()
        {
            throw new NotImplementedException();
        }
    }
}
