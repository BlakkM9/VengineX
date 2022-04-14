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
        /// Dictionary holding all input bindings with their string representation as key.
        /// </summary>
        private readonly Dictionary<string, IBinding> _inputBindings;

        /// <summary>
        /// The maximum amount of time between keypresses to be triggered for<br/>
        /// <see cref="ActionBinding"/>s with <see cref="KeyActionType.DoublePress"/>.
        /// </summary>
        public float MaxDoublePressTimeframe { get; set; } = 0.2f;


        /// <summary>
        /// Creates a new InputManager for given window.
        /// </summary>
        public InputManager(Core.Window window)
        {
            Window = window;

            MouseState = Window.MouseState.GetSnapshot();
            KeyboardState = Window.KeyboardState.GetSnapshot();

            _inputBindings = new Dictionary<string, IBinding>();
        }


        /// <summary>
        /// Adds the given binding with given name to this input manager.
        /// </summary>
        /// <returns>
        /// True if the binding was added, false otherwise.<br/>
        /// Will also return false if the binding (the string key) is already present.
        /// </returns>
        public bool AddBinding(string bindingName, IBinding binding)
        {
            return _inputBindings.TryAdd(bindingName, binding);
        }


        /// <summary>
        /// Removes the input binding with the given name.
        /// </summary>
        /// <returns>
        /// True if the binding was removed, false otherwise.<br/>
        /// Will also return false if the binding does not exist in the input manager.
        /// </returns>
        public bool RemoveBinding(string bindingName)
        {
            return _inputBindings.Remove(bindingName);
        }


        /// <summary>
        /// Returns the input binding with the given name.<br/>
        /// Null if this binding does not exist.
        /// </summary>
        public IBinding? GetBinding(string bindingName)
        {
            _inputBindings.TryGetValue(bindingName, out IBinding? binding);
            return binding;
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

            // Update input bindings.
            foreach (IBinding binding in _inputBindings.Values)
            {
                binding.UpdateValue(this);
            }
        }
    }
}
