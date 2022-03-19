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
    /// Like <see cref="StackLayout"/>, except all elements are drawn on the same position.<br/>
    /// Use this to layout single elements.
    /// </summary>
    public class AlignLayout : Layout
    {
        /// <summary>
        /// Horizontal alignment of child elements
        /// </summary>
        public HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// Vertical alignment of child elements
        /// </summary>
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
            float maxWidth = 0;
            float maxHeight = 0;

            foreach (UIElement child in element.Children)
            {
                Vector2 childPreferred = child.PreferredSize;

                if (childPreferred.X > maxWidth) { maxWidth = childPreferred.X; }
                if (childPreferred.Y > maxHeight) { maxHeight = childPreferred.Y; }
            }

            //maxWidth = MathHelper.Max(maxWidth, element.Width);
            //maxHeight = MathHelper.Max(maxHeight, element.Height);

            return new Vector2(maxWidth, maxHeight);
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
