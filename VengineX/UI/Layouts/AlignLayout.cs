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
        public HorizontalAlignment HorizontalAlignment { get; }

        /// <summary>
        /// Vertical alignment of child elements
        /// </summary>
        public VerticalAlignment VerticalAlignment { get; }


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
                if (child.Width > maxWidth) { maxWidth = child.Width; }
                if (child.Height > maxHeight) { maxHeight = child.Height; }
            }

            return new Vector2(maxWidth, maxHeight);
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void UpdateLayout(UIElement element)
        {
            //Vector2 containerSize = element.Size;

            foreach (UIElement child in element.Children)
            {
                //Vector2 childSize = child.Size;

                float posX = HorizontalAlignment switch
                {
                    HorizontalAlignment.Left => 0,
                    HorizontalAlignment.Center => (element.Width - child.Width) / 2,
                    HorizontalAlignment.Stretch =>  0,
                    HorizontalAlignment.Right => element.Width - child.Width,
                    _ => throw new NotImplementedException(),
                };


                float posY = VerticalAlignment switch
                {
                    VerticalAlignment.Top => 0,
                    VerticalAlignment.Center => (element.Height - child.Height) / 2,
                    VerticalAlignment.Stretch => 0,
                    VerticalAlignment.Bottom => element.Height - child.Height,
                    _ => throw new NotImplementedException(),
                };

                child.Position = new Vector2(posX, posY);


                // Stretch to parent size. Children need to update layout again if size changed.
                if (HorizontalAlignment == HorizontalAlignment.Stretch)
                {
                    child.Width = element.Width;
                    child.UpdateLayout();
                }
                if (VerticalAlignment == VerticalAlignment.Stretch)
                {
                    child.Height = element.Height;
                    child.UpdateLayout();
                }
            }
        }
    }
}
