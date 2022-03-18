using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.UI.LWUI.Elements;

namespace VengineX.UI.LWUI.Layouts
{
    /// <summary>
    /// Like <see cref="StackLayout"/>, except all elements are drawn on the same position.<br/>
    /// Use this to layout single elements.
    /// </summary>
    public class AlignLayout : Layout
    {
        public HorizontalAlignment HorizontalAlignment { get; set; }

        public VerticalAlignment VerticalAlignment { get; set; }


        /// <summary>
        /// Creates new align layout.
        /// </summary>
        public AlignLayout(HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
        {
            HorizontalAlignment = horizontalAlignment;
            VerticalAlignment = verticalAlignment;
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override Vector2 PreferredSize(UIElement element)
        {
            float maxX = 0;
            float maxY = 0;

            bool first = true;
            foreach (UIElement child in element.Children)
            {
                Vector2 childPreferred = child.PreferredSize;

                if (!child.Visible) { continue; }
                if (first) { first = false; }

                if (childPreferred.X > maxX) { maxX = childPreferred.X; }
                if (childPreferred.Y > maxY) { maxY = childPreferred.Y; }
            }

            return new Vector2(maxX, maxY);
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void UpdateLayout(UIElement element)
        {
            Vector2 containerSize = element.Size;

            foreach (UIElement child in element.Children)
            {
                Vector2 childPreferred = child.PreferredSize;

                float posX = HorizontalAlignment switch
                {
                    HorizontalAlignment.Left => 0,
                    HorizontalAlignment.Center => (containerSize.X - childPreferred.X) / 2,
                    HorizontalAlignment.Right => containerSize.X - childPreferred.X,
                    _ => throw new NotImplementedException(),
                };


                float posY = VerticalAlignment switch
                {
                    VerticalAlignment.Top => 0,
                    VerticalAlignment.Center => (containerSize.Y - childPreferred.Y) / 2,
                    VerticalAlignment.Bottom => containerSize.Y - childPreferred.Y,
                    _ => throw new NotImplementedException(),
                };

                child.Position = new Vector2(posX, posY);
            }
        }
    }
}
