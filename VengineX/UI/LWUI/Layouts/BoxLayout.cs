using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.UI.LWUI.Elements;

namespace VengineX.UI.LWUI.Layouts
{
    public class BoxLayout : Layout
    {
        public Orientation Orientation { get; set; }
        public Alignment Alignment { get; set; }

        public float Margin { get; set; }

        public float Spacing { get; set; }

        public BoxLayout(Orientation orientation, Alignment alignment, float margin, float spacing)
        {
            Orientation = orientation;
            Alignment = alignment;
            Margin = margin;
            Spacing = spacing;
        }


        public override Vector2 PreferredSize(UIElement element)
        {
            Vector2 size = new Vector2(Margin * 2);

            bool first = true;
            int axis1 = (int)Orientation;
            int axis2 = ((int)Orientation + 1) % 2;

            foreach (UIElement child in element.Children)
            {
                if (!child.Visible) { continue; }
                if (first) { first = false; }
                else { size[axis1] += Spacing; }

                Vector2 preferredSize = child.PreferredSize;
                Vector2 fixedSize = child.FixedSize;
                Vector2 targetSize = new Vector2(
                    fixedSize[0] != 0 ? fixedSize[0] : preferredSize[0],
                    fixedSize[1] != 0 ? fixedSize[1] : preferredSize[1]);

                size[axis1] += targetSize[axis1];
                size[axis2] = MathHelper.Max(size[axis2], targetSize[axis2] + 2 * Margin);
                first = false;
            }

            return size;
        }


        public override void PerformLayout(UIElement element)
        {
            Vector2 fixedSizeChild = element.FixedSize;
            Vector2 containerSize = new Vector2(
                fixedSizeChild[0] != 0 ? fixedSizeChild[0] : element.Width,
                fixedSizeChild[1] != 0 ? fixedSizeChild[1] : element.Height);

            int axis1 = (int)Orientation;
            int axis2 = ((int)Orientation + 1) % 2;
            float position = Margin;

            bool first = true;
            foreach (UIElement child in element.Children)
            {
                if (!child.Visible) { continue; }
                if (first) { first = false; }
                else { position += Spacing; }

                Vector2 preferredSize = element.PreferredSize;
                Vector2 fixedSize = element.FixedSize;
                Vector2 targetSize = new Vector2(
                    fixedSize[0] != 0 ? fixedSize[0] : preferredSize[0],
                    fixedSize[1] != 0 ? fixedSize[1] : preferredSize[1]);

                Vector2 pos = Vector2.Zero;

                pos[axis1] = position;

                switch (Alignment)
                {
                    case Alignment.Minimum:
                        pos[axis2] += Margin;
                        break;
                    case Alignment.Middle:
                        pos[axis2] += (containerSize[axis2] - targetSize[axis2]) / 2;
                        break;
                    case Alignment.Maximum:
                        pos[axis2] += containerSize[axis2] - targetSize[axis2] - Margin * 2;
                        break;
                    case Alignment.Fill:
                        pos[axis2] += Margin;
                        targetSize[axis2] = fixedSize[axis2] != 0 ? fixedSize[axis2] : (containerSize[axis2] - Margin * 2);
                        break;
                }

                element.Position = pos;
                element.Size = targetSize;
                element.PerformLayout();
                position += targetSize[axis1];
            }
        }
    }
}
