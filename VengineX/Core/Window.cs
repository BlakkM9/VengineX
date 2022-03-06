using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Core
{
    /// <summary>
    /// The game's window.
    /// </summary>
    public class Window : GameWindow
    {
        /// <summary>
        /// Gets and sets the <see cref="WindowMode"/> of the this window.
        /// </summary>
        public WindowMode WindowMode
        {
            get => _windowMode;
            set
            {
                switch (value)
                {
                    case WindowMode.Fullscreen:
                        WindowState = OpenTK.Windowing.Common.WindowState.Fullscreen;
                        WindowBorder = OpenTK.Windowing.Common.WindowBorder.Fixed;
                        break;
                    case WindowMode.Borderless:
                        WindowState = OpenTK.Windowing.Common.WindowState.Normal;
                        WindowBorder = OpenTK.Windowing.Common.WindowBorder.Hidden;
                        break;
                    case WindowMode.Windowed:
                        WindowState = OpenTK.Windowing.Common.WindowState.Normal;
                        WindowBorder = OpenTK.Windowing.Common.WindowBorder.Fixed;
                        break;
                }

                _windowMode = value;
            }
        }
        private WindowMode _windowMode;


        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings) { }
    }
}
