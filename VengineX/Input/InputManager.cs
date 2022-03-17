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
        private readonly Core.Window _window;

        /// <summary>
        /// Is the cursor currently grabbed (and invisible)?
        /// </summary>
        public bool MouseCatched { get => _window.CursorGrabbed; }

        /// <summary>
        /// The current <see cref="OpenTK.Windowing.GraphicsLibraryFramework.MouseState"/> in the current frame.
        /// </summary>
        public MouseState MouseState { get; private set; }

        /// <summary>
        /// The current <see cref="OpenTK.Windowing.GraphicsLibraryFramework.KeyboardState"/> in the current frame.
        /// </summary>
        public KeyboardState KeyboardState { get; private set; }

        /// <summary>
        /// Was any mouse button down the last update?
        /// </summary>
        public bool WasAnyMouseButtonDown { get; private set; }

        /// <summary>
        /// Was any mouse button pressed this frame?
        /// </summary>
        public bool AnyMouseButtonPressed { get; private set; }

        /// <summary>
        /// Was any mouse button released this frame?
        /// </summary>
        public bool AnyMouseButtonReleased { get; private set; }

        /// <summary>
        /// Creates a new InputManager for given window.
        /// </summary>
        /// <param name="window"></param>
        public InputManager(Core.Window window)
        {
            _window = window;
        }

        /// <summary>
        /// Updates <see cref="MouseState"/> and <see cref="KeyboardState"/>.<br/>
        /// Called at the beginning of every frame update from <see cref="Game{T}"/>.
        /// </summary>
        internal void Update()
        {
            // Store if any mouse button was down last update.
            if (MouseState != null)
            {
                WasAnyMouseButtonDown = MouseState.IsAnyButtonDown;
            }


            // Store current state.
            MouseState = _window.MouseState.GetSnapshot();
            KeyboardState = _window.KeyboardState.GetSnapshot();


            // Update MousePressed an MouseReleased
            if (MouseState.IsAnyButtonDown && !WasAnyMouseButtonDown)
            {
                AnyMouseButtonPressed = true;
                AnyMouseButtonReleased = false;
            }
            else if (!MouseState.IsAnyButtonDown && WasAnyMouseButtonDown)
            {
                AnyMouseButtonReleased = true;
                AnyMouseButtonPressed = false;
            }
            else
            {
                AnyMouseButtonPressed = false;
                AnyMouseButtonReleased = false;
            }
        }
    }
}
