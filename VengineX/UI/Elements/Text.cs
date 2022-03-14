using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.UI.Fonts;
using VengineX.Wrappers.FreeType;

namespace VengineX.UI.Elements
{
    /// <summary>
    /// A label with text.
    /// </summary>
    public class Text : UIElement
    {
        public Text(BitmapFont font, string text, float x, float y, float size, Vector3 color)
            : base(x, y, font.CalculateWidth(text), size)
        {

        }

        public override void Render()
        {
            throw new NotImplementedException();
        }
    }
}
