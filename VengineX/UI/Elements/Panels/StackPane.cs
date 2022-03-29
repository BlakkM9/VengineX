using VengineX.UI.Elements.Basic;
using VengineX.UI.Layouts;

namespace VengineX.UI.Elements.Panels
{
    /// <summary>
    /// Pane that uses <see cref="StackLayout"/>
    /// </summary>
    public class StackPane : Pane
    {
        /// <summary>
        /// Horizontal layout of the children in this pane.
        /// </summary>
        public HorizontalAlignment HorizontalAlignment
        {
            get => _horizontalAlignment;
            set
            {
                _horizontalAlignment = value;
                Layout = new StackLayout(_horizontalAlignment, _verticalAlignment, _orientation, _spacing);
            }
        }
        /// <summary>
        /// Internal field for <see cref="HorizontalAlignment"/>.
        /// </summary>
        protected HorizontalAlignment _horizontalAlignment;

        /// <summary>
        /// Vertical layout of the children in this pane.
        /// </summary>
        public VerticalAlignment VerticalAlignment
        {
            get => _verticalAlignment;
            set
            {
                _verticalAlignment = value;
                Layout = new StackLayout(_horizontalAlignment, _verticalAlignment, _orientation, _spacing);
            }
        }
        /// <summary>
        /// Internal field for <see cref="VerticalAlignment"/>.
        /// </summary>
        protected VerticalAlignment _verticalAlignment;

        /// <summary>
        /// The orientation of children in this pane.
        /// </summary>
        public Orientation Orientation
        {
            get => _orientation;
            set
            {
                _orientation = value;
                Layout = new StackLayout(_horizontalAlignment, _verticalAlignment, _orientation, _spacing);
            }
        }
        /// <summary>
        /// Internal field for <see cref="Orientation"/>.
        /// </summary>
        protected Orientation _orientation;

        /// <summary>
        /// Spacing between the children in this pane.
        /// </summary>
        public float Spacing
        {
            get => _spacing;
            set
            {
                _spacing = value;
                Layout = new StackLayout(_horizontalAlignment, _verticalAlignment, _orientation, _spacing);
            }
        }
        /// <summary>
        /// Internal field for <see cref="Spacing"/>.
        /// </summary>
        protected float _spacing;

        /// <summary>
        /// Creates a new stack pane.
        /// </summary>
        public StackPane(Element parent) : base(parent) { }
    }
}
