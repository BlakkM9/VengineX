using OpenTK.Windowing.GraphicsLibraryFramework;
using VengineX.Core;

namespace VengineX.Input
{
    /// <summary>
    /// Class that handles the input for this game.
    /// </summary>
    public class InputManager
    {
        /// <summary>
        /// The window this InputManager receives its events from.
        /// </summary>
        public Core.Window Window { get; }

        /// <summary>
        /// Is the cursor currently grabbed (and invisible)?
        /// </summary>
        public bool MouseCatched { get => Window.CursorGrabbed; }

        /// <summary>
        /// The current <see cref="OpenTK.Windowing.GraphicsLibraryFramework.MouseState"/> in the current frame.
        /// </summary>
        public MouseState MouseState { get; private set; }

        /// <summary>
        /// The current <see cref="OpenTK.Windowing.GraphicsLibraryFramework.KeyboardState"/> in the current frame.
        /// </summary>
        public KeyboardState KeyboardState { get; private set; }


        /// <summary>
        /// Creates a new InputManager for given window.
        /// </summary>
        public InputManager(Core.Window window)
        {
            Window = window;

            MouseState = Window.MouseState.GetSnapshot();
            KeyboardState = Window.KeyboardState.GetSnapshot();
        }


        /// <summary>
        /// Updates <see cref="MouseState"/> and <see cref="KeyboardState"/>.<br/>
        /// Called at the beginning of every frame update from <see cref="Game{T}"/>.
        /// </summary>
        internal void Update()
        {
            // Store current keyboard and mouse state.
            MouseState = Window.MouseState.GetSnapshot();
            KeyboardState = Window.KeyboardState.GetSnapshot();
        }
    }
}
