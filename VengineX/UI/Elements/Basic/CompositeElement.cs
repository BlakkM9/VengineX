using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering.Batching;

namespace VengineX.UI.Elements.Basic
{
    /// <summary>
    /// Base class for <see cref="Elements"/> that are composed of a variety of other elements.<br/>
    /// Derive from this class when creating new UI elements and want to load their layout from file.<br/>
    /// (At compile-time)
    /// </summary>
    public abstract partial class CompositeElement : Element
    {
        /// <summary>
        /// Creates a new composite element. Calls <see cref="InitializeChildren"/>,<br/>
        /// Followed by a layout update call of the canvas.
        /// </summary>
        /// <param name="parent"></param>
        protected CompositeElement(Element? parent) : base(parent)
        {
            InitializeChildren();
            Canvas.UpdateLayout();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override IEnumerable<UIBatchQuad> EnumerateQuads()
        {
            yield break;
        }


        /// <summary>
        /// Initializes all the children (recursive) of this element. This method is filled by the<br/>
        /// source generators when created from xml.
        /// </summary>
        protected virtual void InitializeChildren() { }
    }
}
