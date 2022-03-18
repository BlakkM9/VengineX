using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.UI.Elements;

namespace VengineX.UI.Layouts
{
    public abstract class Layout
    {
        /// <summary>
        /// Calculates and returns the preferred size for <paramref name="element"/>.
        /// </summary>
        public abstract Vector2 PreferredSize(UIElement element);

        /// <summary>
        /// Calculates and sets the layout for all children of <paramref name="element"/>.
        /// </summary>
        public abstract void UpdateLayout(UIElement element);
    }
}
