using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.UI.Elements.Basic;
using VengineX.UI.Layouts;

namespace VengineX.UI.Elements.Panels
{
    /// <summary>
    /// Pane that uses <see cref="StackLayout"/>
    /// </summary>
    public class StackPane : Pane
    {
        /// <summary>
        /// Creates a new stack pane.
        /// </summary>
        public StackPane(
            Element parent,
            HorizontalAlignment horizontalAlignment,
            VerticalAlignment verticalAlignment,
            Orientation orientation, float spacing)
            : base(parent)
        {
            Layout = new StackLayout(horizontalAlignment, verticalAlignment, orientation, spacing);
        }
    }
}
