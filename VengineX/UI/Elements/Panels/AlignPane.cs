using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.UI.Elements.Basic;
using VengineX.UI.Layouts;

namespace VengineX.UI.Elements.Panels
{
    /// <summary>
    /// Pane that uses <see cref="AlignLayout"/>
    /// </summary>
    public class AlignPane : Pane
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
                Layout = new AlignLayout(_horizontalAlignment, _verticalAlignment);
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
                Layout = new AlignLayout(_horizontalAlignment, _verticalAlignment);
            }
        }
        /// <summary>
        /// Internal field for <see cref="VerticalAlignment"/>.
        /// </summary>
        protected VerticalAlignment _verticalAlignment;


        /// <summary>
        /// Creates a new align pane.
        /// </summary>
        public AlignPane(Element parent) : base(parent) { }
    }
}
