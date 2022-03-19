using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.UI.Layouts;

namespace VengineX.UI.Elements.Panels
{
    /// <summary>
    /// Pane that uses <see cref="AlignLayout"/>
    /// </summary>
    public class AlignPane : UIElement
    {
        /// <summary>
        /// Creates a new align pane.
        /// </summary>
        public AlignPane(
            UIElement parent,
            HorizontalAlignment horizontalAlignment,
            VerticalAlignment verticalAlignment) : base(parent)
        {
            Layout = new AlignLayout(horizontalAlignment, verticalAlignment);
        }
    }
}
