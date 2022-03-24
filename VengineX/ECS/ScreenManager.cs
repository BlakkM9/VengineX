namespace VengineX.ECS
{
    public class ScreenManager
    {

        /// <summary>
        /// Screen that is currently active
        /// </summary>
        public IScreen? CurrentScreen
        {
            get => _currentScreen;
            set
            {
                CurrentScreen?.Unload();
                _currentScreen = value;
                CurrentScreen?.Load();
            }
        }
        protected IScreen? _currentScreen;
    }
}
