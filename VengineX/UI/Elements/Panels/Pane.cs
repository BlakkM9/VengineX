using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.UI.Elements.Panels
{
    /// <summary>
    /// Most basic pane that does not have any specified layout.
    /// </summary>
    public class Pane : UIElement
    {
        public Pane(UIElement parent) : base(parent) { }
    }
}
