using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering;
using VengineX.UI.Elements;

namespace VengineX.UI.Canvases
{
    /// <summary>
    /// Canvas is the root of any UI.<br/>
    /// It is re-rendered onto famebuffers texture if needed.
    /// </summary>
    public class Canvas : IRenderable
    {
        /// <summary>
        /// Width of the canvas.
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Height of the canvas.
        /// </summary>
        public float Height { get; set; }

        // TODO implement
        /// <summary>
        /// Inner padding of the canas.<br/>
        /// Padding means that the Children can only be moved<br/>
        /// within the inner space even if the canvas is larger than that.
        /// </summary>
        public float Padding { get; set; }

        public bool Dirty { get; set; }

        public List<UIElement> Children { get; private set; }


        public Canvas(float width, float height)
        {
            Width = width;
            Height = height;
            Children = new List<UIElement>();
        }


        public void Render()
        {
            if (Dirty)
            {
                foreach (UIElement child in Children)
                {
                    child.Render();
                }

                Dirty = false;
            }

        }

        public void Resize(float newWidth, float newHeight)
        {
            Width = newWidth;
            Height = newHeight;
            Dirty = true;
        }
    }
}
