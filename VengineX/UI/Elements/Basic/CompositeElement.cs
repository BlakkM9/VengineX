using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering.Batching;

namespace VengineX.UI.Elements.Basic
{
    public abstract partial class CompositeElement : Element
    {
        protected CompositeElement(Element? parent) : base(parent)
        {
            InitializeChildren();
            Canvas.UpdateLayout();
        }

        public override IEnumerable<UIBatchQuad> EnumerateQuads()
        {
            yield break;
        }

        protected virtual void InitializeChildren() { }
    }
}
