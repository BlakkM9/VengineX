using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.UI.LWUI.Layouts
{
    /// <summary>
    /// Generalized alignment
    /// </summary>
    public enum Alignment
    {
        /// <summary>
        /// Alignment left / top.
        /// </summary>
        Start = 0,
        /// <summary>
        /// Aligned center.
        /// </summary>
        Center = 1,
        /// <summary>
        /// Aligned right / bottom.
        /// </summary>
        End = 2,
    }


    /// <summary>
    /// Describes horizontal alignment (must remain castable to <see cref="Alignment"/>.
    /// </summary>
    public enum HorizontalAlignment
    {
        /// <summary>
        /// Aligns left.
        /// </summary>
        Left = 0,
        /// <summary>
        /// Aligns center.
        /// </summary>
        Center = 1,
        /// <summary>
        /// Aligns right.
        /// </summary>
        Right = 2,
    }


    /// <summary>
    /// Describes vertical alignment (must remain castable to <see cref="Alignment"/>.
    /// </summary>
    public enum VerticalAlignment
    {
        /// <summary>
        /// Aligns top.
        /// </summary>
        Top = 0,
        /// <summary>
        /// Aligns center.
        /// </summary>
        Center = 1,
        /// <summary>
        /// Aligns bottom.
        /// </summary>
        Bottom = 2,
    }
}
