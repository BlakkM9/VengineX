using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.UI.Elements;
using VengineX.UI.Elements.Panels;
using VengineX.UI.Layouts;

namespace VengineX.UI
{
    public abstract class UIGroup : UIElement
    {
        protected UIGroup(UIElement parent) : base(parent) { }

        /// <summary>
        /// This function is calld when the <see cref="UIGroup"/> was initialized after loading.
        /// </summary>
        protected abstract void Initialized();
    }
}
