using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.UI.Elements;

namespace VengineX.UI.Layouts
{
    /// <summary>
    /// Base class for all layouts. Layouts are used to control the layout of child ui elements.<br/>
    /// Layouts should not only contain public immutable properties. <br/>
    /// If the properties of the layout need to change, create a new layout (needed for updating dirty flag in <see cref="Element"/>).
    /// </summary>
    public abstract class Layout
    {
        /// <summary>
        /// Calculates and returns the <see cref="Element.PreferredSize"/> for <paramref name="element"/>.
        /// </summary>
        public abstract Vector2 PreferredSize(Element element);

        /// <summary>
        /// Calculates and sets the layout (<see cref="Element.Size"/> and <see cref="Element.Position"/>) for all children of <paramref name="element"/>.
        /// </summary>
        public abstract void UpdateLayout(Element element);
    }
}
