using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering.Batching;
using VengineX.UI.Elements;
using VengineX.UI.Elements.Panels;
using VengineX.UI.Layouts;
using VengineX.UI.Serialization;

namespace VengineX.UI
{
    /// <summary>
    /// Base class for a group of UIElements that can be loaded from an XML file.<br/>
    /// This will be the root element of the loaded UI. For loading the UI use <see cref="UISerializer.LoadFromXML{T}(Element, string)"/>.
    /// </summary>
    public abstract class LoadableUITemplate : Element
    {
        protected LoadableUITemplate(Element parent) : base(parent) { }

        public override IEnumerable<UIBatchQuad> EnumerateQuads()
        {
            yield break;
        }

        /// <summary>
        /// This function is calld when the <see cref="LoadableUITemplate"/> was initialized after loading.
        /// </summary>
        protected abstract void Initialized();
    }
}
