using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Wrappers.FreeType;

namespace VengineX.UI.Elements
{
    /// <summary>
    /// A label with text.
    /// </summary>
    public class Label : UIElement
    {
        public Label(FreeTypeFont font, string text, float x, float y, float size, Vector3 color)
        {

        }

        public override void Render()
        {
            throw new NotImplementedException();
        }
    }
}
