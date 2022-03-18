using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.UI.Layouts;

namespace VengineX.UI.Elements.Panels
{
    /// <summary>
    /// Pane that uses <see cref="StackLayout"/>
    /// </summary>
    public class StackPane : UIElement
    {
        /// <summary>
        /// Creates a new stack pane.
        /// </summary>
        public StackPane(
            UIElement parent,
            HorizontalAlignment horizontalAlignment,
            VerticalAlignment verticalAlignment,
            Orientation orientation, float spacing)
            : base(parent)
        {
            Layout = new StackLayout(horizontalAlignment, verticalAlignment, orientation, spacing);
        }
    }
}
