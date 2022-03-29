namespace VengineX.ECS
{
    public class ScreenManager
    {
        /// <summary>
        /// Handler for the <see cref="ScreenChanged"/> event.
        /// </summary>
        public delegate void ScreenChangedHandler(Screen? previousScreen, Screen newScreen);

        /// <summary>
        /// Occurs when the screen changed.
        /// </summary>
        public event ScreenChangedHandler? ScreenChanged;

        /// <summary>
        /// Dictionary holding all the screens with their type as key.
        /// </summary>
        private Dictionary<Type, Screen> _screens;


        /// <summary>
        /// Screen that is currently active
        /// </summary>
        public Screen? CurrentScreen { get => _currentScreen; private set => _currentScreen = value; }
        private Screen? _currentScreen;


        /// <summary>
        /// Creates a new screen manager.
        /// </summary>
        public ScreenManager() { 
            _screens = new Dictionary<Type, Screen>();
        }


        /// <summary>
        /// Adds a screen to the screen manager.
        /// </summary>
        public void AddScreen(Screen screen)
        {
            _screens.Add(screen.GetType(), screen);
        }


        /// <summary>
        /// Sets the current screen to the given screen type.
        /// </summary>
        public void SetScreen<T>() where T : Screen
        {
            // Get the screen from dictionary
            if (_screens.TryGetValue(typeof(T), out Screen? newScreen))
            {
                CurrentScreen?.Unload();

                Screen? previousScreen = CurrentScreen;
                CurrentScreen = newScreen;

                newScreen.Load();

                ScreenChanged?.Invoke(previousScreen, newScreen);
            }
        }


        /// <summary>
        /// Updates the current screen.
        /// </summary>
        public void Update(double delta) => CurrentScreen?.Update(delta);


        /// <summary>
        /// Renders the current screen.
        /// </summary>
        public void Render(double delta) => CurrentScreen?.Render(delta);
    }
}
