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
    /// Aligns the children based on <see cref="HorizontalAlignment"/> and <see cref="VerticalAlignment"/><br/>
    /// And places all children based on <see cref="Orientation"/> and <see cref="Spacing"/> to each other.
    /// </summary>
    public class StackLayout : Layout
    {
        public HorizontalAlignment HorizontalAlignment
        {
            get => Orientation == Orientation.Horizontal ? (HorizontalAlignment)_alignments[0] : (HorizontalAlignment)_alignments[1];
        }

        public VerticalAlignment VerticalAlignment
        {
            get => Orientation == Orientation.Vertical ? (VerticalAlignment)_alignments[0] : (VerticalAlignment)_alignments[1];
        }

        /// <summary>
        /// Horizontal and vertical alignments.<br/>
        /// If <see cref="Orientation"/> is horizontal, element at 0 is <see cref="HorizontalAlignment"/><br/>
        /// And the other way around.
        /// </summary>
        private Alignment[] _alignments;

        /// <summary>
        /// The orientatio the children of this layout are arranged.
        /// </summary>
        public Orientation Orientation
        {
            get => _orientation;
            set
            {
                _orientation = value;

                if (_orientation == Orientation.Horizontal)
                {
                    _alignments = new Alignment[] { (Alignment)HorizontalAlignment, (Alignment)VerticalAlignment };
                }
                else
                {
                    _alignments = new Alignment[] { (Alignment)VerticalAlignment, (Alignment)HorizontalAlignment };
                }
            }
        }
        private Orientation _orientation;

        /// <summary>
        /// Spacing between the children of this layout.
        /// </summary>
        public float Spacing { get; set; }


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

            _orientation = orientation;
            Spacing = spacing;
        }



        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override Vector2 PreferredSize(UIElement element)
        {
            int axis1 = (int)Orientation;
            int axis2 = ((int)Orientation + 1) % 2;

            Vector2 preferredSize = Vector2.Zero;
            float maxAxis2 = 0;
            bool first = true;
            foreach (UIElement child in element.Children)
            {
                Vector2 childPreferred = child.PreferredSize;

                if (!child.Visible) { continue; }
                if (first) { first = false; }
                else { preferredSize[axis1] += Spacing; }
                preferredSize[axis1] += childPreferred[axis1];

                if (childPreferred[axis2] > maxAxis2) { maxAxis2 = childPreferred[axis2]; }
            }

            preferredSize[axis2] = maxAxis2;
            return preferredSize;
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void UpdateLayout(UIElement element)
        {
            Vector2 containerSize = element.Size;

            int axis1 = (int)Orientation;
            int axis2 = ((int)Orientation + 1) % 2;

            Vector2 preferedSize = element.PreferredSize;

            float pos1 = _alignments[0] switch
            {
                Alignment.Start => 0,
                Alignment.Center => (containerSize[axis1] - preferedSize[axis1]) / 2,
                Alignment.End => containerSize[axis1] - preferedSize[axis1],
                _ => throw new NotImplementedException(),
            };


            bool first = true;
            foreach (UIElement child in element.Children)
            {
                Vector2 childPrefered = child.PreferredSize;
                if (!child.Visible) { continue; }
                if (first) { first = false; }
                else { pos1 += Spacing; }

                Vector2 targetPos = Vector2.Zero;
                targetPos[axis1] = pos1;
                pos1 += childPrefered[axis1];

                float pos2 = _alignments[1] switch
                {
                    Alignment.Start => 0,
                    Alignment.Center => (MathHelper.Max(preferedSize[axis2], element.Size[axis2]) - childPrefered[axis2]) / 2,
                    Alignment.End => MathHelper.Max(preferedSize[axis2], element.Size[axis2]) - childPrefered[axis2],
                    _ => throw new NotImplementedException(),
                };
                targetPos[axis2] = pos2;

                child.Position = targetPos;
            }
        }
    }
}
