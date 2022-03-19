﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.UI.Layouts;
using VengineX.UI.Serialization;

namespace VengineX.UI.Elements.Panels
{
    /// <summary>
    /// Pane that uses <see cref="AlignLayout"/>
    /// </summary>
    public class AlignPane : Pane
    {
        public HorizontalAlignment HorizontalAlignment
        {
            get => _horizontalAlignment;
            set
            {
                _horizontalAlignment = value;
                Layout = new AlignLayout(_horizontalAlignment, _verticalAlignment);
            }
        }
        protected HorizontalAlignment _horizontalAlignment;
        
        public VerticalAlignment VerticalAlignment
        {
            get => _verticalAlignment;
            set
            {
                _verticalAlignment = value;
                Layout = new AlignLayout(_horizontalAlignment, _verticalAlignment);
            }
        }
        protected VerticalAlignment _verticalAlignment;

        /// <summary>
        /// Creates a new align pane.
        /// </summary>
        public AlignPane(
            UIElement parent,
            HorizontalAlignment horizontalAlignment,
            VerticalAlignment verticalAlignment) : base(parent)
        {
            _horizontalAlignment = horizontalAlignment;
            _verticalAlignment = verticalAlignment;
            Layout = new AlignLayout(_horizontalAlignment, _verticalAlignment);
        }


        /// <summary>
        /// Empty constructor for creating the instance with <see cref="UISerializer"/>
        /// </summary>
        /// <param name="parent"></param>
        public AlignPane(UIElement parent) : base(parent) { }
    }
}
