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
        public override Vector2 PreferredSize(Element element)
        {
            float maxWidth = 0;
            float maxHeight = 0;

            foreach (Element child in element.EnumerateChildren())
            {
                if (child.IgnoreLayout) { continue; }

                float childTotalW = child.TotalWidth;
                float childTotalH = child.TotalHeight;

                if (childTotalW > maxWidth) { maxWidth = childTotalW; }
                if (childTotalH > maxHeight) { maxHeight = childTotalH; }
            }

            return new Vector2(maxWidth, maxHeight);
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void UpdateLayout(Element element)
        {
            foreach (Element child in element.EnumerateChildren())
            {
                if (child.IgnoreLayout) { continue; }


                float posX = HorizontalAlignment switch
                {
                    HorizontalAlignment.Left => child.MarginLeft,
                    HorizontalAlignment.Center => (element.Width - child.Width) / 2 + child.MarginLeft - child.MarginRight,
                    HorizontalAlignment.Stretch =>  child.MarginLeft,
                    HorizontalAlignment.Right => element.Width - child.Width - child.MarginRight,
                    _ => throw new NotImplementedException(),
                };


                float posY = VerticalAlignment switch
                {
                    VerticalAlignment.Top => child.MarginTop,
                    VerticalAlignment.Center => (element.Height - child.Height) / 2 + child.MarginLeft - child.MarginRight,
                    VerticalAlignment.Stretch => child.MarginTop,
                    VerticalAlignment.Bottom => element.Height - child.Height - child.MarginBottom,
                    _ => throw new NotImplementedException(),
                };

                child.Position = new Vector2(posX, posY);


                // Stretch to parent size. Children need to update layout again if size changed.
                if (HorizontalAlignment == HorizontalAlignment.Stretch)
                {
                    child.Width = element.Width - child.MarginLeft - child.MarginRight;
                    child.UpdateLayout();
                }
                if (VerticalAlignment == VerticalAlignment.Stretch)
                {
                    child.Height = element.Height - child.MarginTop - child.MarginBottom;
                    child.UpdateLayout();
                }
            }
        }
    }
}
