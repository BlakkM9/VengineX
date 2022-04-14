using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Input
{
    /// <summary>
    /// Types of actions for keys.
    /// </summary>
    public enum KeyActionType
    {
        /// <summary>
        /// Key is down in the frame.
        /// </summary>
        Hold,

        /// <summary>
        /// Key was released in this frame.
        /// </summary>
        Release,

        /// <summary>
        /// Key was pressed in this frame.
        /// </summary>
        Press,

        /// <summary>
        /// Key was pressed the second time in <see cref="InputManager.MaxDoublePressTimeframe"/> timeframe.
        /// </summary>
        DoublePress,
    }
}
