using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Core
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
