using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering;

namespace VengineX.UI.Elements
{
    /// <summary>
    /// Base class for all UI elements.
    /// </summary>
    public abstract class UIElement : IRenderable
    {

        /// <summary>
        /// The absolute position in the canvas.
        /// </summary>
        public float X { get; set; }

        public float Y { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }



        /// <summary>
        /// Renders this ui element.
        /// </summary>
        public abstract void Render();
    }
}
