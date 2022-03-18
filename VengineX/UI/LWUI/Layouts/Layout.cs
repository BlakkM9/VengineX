using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.UI.LWUI.Elements;

namespace VengineX.UI.LWUI.Layouts
{
    public abstract class Layout
    {

        public abstract void PerformLayout(UIElement element);

        public abstract Vector2 PreferredSize(UIElement element);
    }
}
