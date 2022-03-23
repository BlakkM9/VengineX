using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.UI.Elements.Basic;

namespace VengineX.UI.Layouts
{
    /// <summary>
    /// Aligns the children based on <see cref="HorizontalAlignment"/> and <see cref="VerticalAlignment"/><br/>
    /// And places all children based on <see cref="Orientation"/> and <see cref="Spacing"/> to each other.
    /// </summary>
    public class StackLayout : Layout
    {
        /// <summary>
        /// The horizontal alignment of child elements.
        /// </summary>
        public HorizontalAlignment HorizontalAlignment
        {
            get => Orientation == Orientation.Horizontal ? (HorizontalAlignment)_alignments[0] : (HorizontalAlignment)_alignments[1];
        }

        /// <summary>
        /// The vertical alignment of child elements.
        /// </summary>
        public VerticalAlignment VerticalAlignment
        {
            get => Orientation == Orientation.Vertical ? (VerticalAlignment)_alignments[0] : (VerticalAlignment)_alignments[1];
        }

        /// <summary>
        /// Horizontal and vertical alignments.<br/> Example:
        /// If <see cref="Orientation"/> is horizontal, element at 0 is <see cref="HorizontalAlignment"/><br/>
        /// And the other way around.
        /// </summary>
        private readonly Alignment[] _alignments;

        /// <summary>
        /// The orientatio the children of this layout are arranged.
        /// </summary>
        public Orientation Orientation { get; }



        /// <summary>
        /// Spacing between the children of this layout.
        /// </summary>
        public float Spacing { get; }


        /// <summary>
        /// Creates a new stack layout with given parameters.
        /// </summary>
        public StackLayout(HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, Orientation orientation, float spacing)
        {
            if (orientation == Orientation.Horizontal)
            {
                _alignments = new Alignment[] { (Alignment)horizontalAlignment, (Alignment)verticalAlignment };
            }
            else
            {
                _alignments = new Alignment[] { (Alignment)verticalAlignment, (Alignment)horizontalAlignment };
            }

            Orientation = orientation;
            Spacing = spacing;
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override Vector2 PreferredSize(Element element)
        {
            int axis1 = (int)Orientation;
            int axis2 = ((int)Orientation + 1) % 2;

            Vector2 preferredSize = Vector2.Zero;
            float maxAxis2 = 0;
            bool first = true;
            foreach (Element child in element.EnumerateChildren())
            {
                if (child.IgnoreLayout) { continue; }

                Vector2 childTotalSize = child.TotalSize;

                if (first) { first = false; }
                else { preferredSize[axis1] += Spacing; }
                preferredSize[axis1] += childTotalSize[axis1];

                if (childTotalSize[axis2] > maxAxis2) { maxAxis2 = childTotalSize[axis2]; }
            }

            preferredSize[axis2] = maxAxis2;
            return preferredSize;
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void UpdateLayout(Element element)
        {
            Vector2 containerSize = element.Size;
            Vector2 preferedSize = element.PreferredSize;

            int axis1 = (int)Orientation;
            int axis2 = ((int)Orientation + 1) % 2;


            float pos1 = _alignments[0] switch
            {
                Alignment.Start => 0,
                Alignment.Center => (containerSize[axis1] - preferedSize[axis1]) / 2,
                Alignment.Stretch => (containerSize[axis1] - preferedSize[axis1]) / 2,
                Alignment.End => containerSize[axis1] - preferedSize[axis1],
                _ => throw new NotImplementedException(),
            };


            bool first = true;
            foreach (Element child in element.EnumerateChildren())
            {
                if (child.IgnoreLayout) { continue; }

                if (first) { first = false; }
                else { pos1 += Spacing; }

                Vector2 childSize = child.Size;
                Vector2 targetPos = Vector2.Zero;
                float marginAxis1Start = child.Margin[axis1];
                float marginAxis1End = child.Margin[axis1 + 2];
                float marginAxis2Start = child.Margin[axis2];
                float marginAxis2End = child.Margin[axis2 + 2];

                targetPos[axis1] = pos1 + marginAxis1Start;
                pos1 += childSize[axis1] + marginAxis1End;

                float pos2 = _alignments[1] switch
                {
                    Alignment.Start => marginAxis2Start,
                    Alignment.Center => (containerSize[axis2] - childSize[axis2]) / 2 + marginAxis2Start - marginAxis2End,
                    Alignment.Stretch => marginAxis2Start,
                    Alignment.End => containerSize[axis2] - childSize[axis2] - marginAxis2End,
                    _ => throw new NotImplementedException(),
                };
                targetPos[axis2] = pos2;

                child.Position = targetPos;

                // Strech on non alignment axis
                if (_alignments[1] == Alignment.Stretch)
                {
                    Vector2 targetSize = Vector2.Zero;
                    targetSize[axis1] = child.Size[axis1];
                    targetSize[axis2] = containerSize[axis2] - marginAxis2Start - marginAxis2End;
                    child.Size = targetSize;
                    child.UpdateLayout();
                }
            }
        }
    }
}
