using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Core;

namespace VengineX.Input
{
    public class InputManager
    {
        /// <summary>
        /// The current <see cref="OpenTK.Windowing.GraphicsLibraryFramework.MouseState"/> in the current frame.
        /// </summary>
        public MouseState MouseState { get; private set; }

        /// <summary>
        /// The current <see cref="OpenTK.Windowing.GraphicsLibraryFramework.KeyboardState"/> in the current frame.
        /// </summary>
        public KeyboardState KeyboardState { get; private set; }


        /// <summary>
        /// Updates <see cref="MouseState"/> and <see cref="KeyboardState"/>.<br/>
        /// Called at the beginning of every frame update from <see cref="Game{T}"/>.
        /// </summary>
        /// <param name="window">The window to take the input state snapshot from.</param>
        internal void Update(Core.Window window)
        {
            MouseState = window.MouseState.GetSnapshot();
            KeyboardState = window.KeyboardState.GetSnapshot();
        }
    }
}
