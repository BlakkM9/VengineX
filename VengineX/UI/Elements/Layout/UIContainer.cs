using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.UI.Elements.Layout
{
    /// <summary>
    /// A canvas-like container that groups ui elements together and<br/>
    /// might easily be added to an canvas.
    /// </summary>
    public class UIContainer : EventDrivenUIElement
    {
        public UIContainer(float x, float y, float width, float height)
            : base(x, y, width, height) { }


        public override void Render()
        {
            foreach (UIElement child in Children)
            {
                child.Render();
            }
        }


        protected override void CalculateModelMatrix()
        {
            // Update model matrix
            ModelMatrix = Matrix4.Identity;
            ModelMatrix *= Matrix4.CreateScale(Width / 2f, Height / 2f, 0);
            ModelMatrix *= Matrix4.CreateTranslation(Width / 2f + X, -(Height / 2f + Y), 0);
        }
    }
}
