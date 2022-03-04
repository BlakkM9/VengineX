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
    /// The game.
    /// </summary>
    public class Game
    {
        public Window Window { get; private set; }


        /// <summary>
        /// Default constructor, initialisating game's window with default values.<br/>
        /// This should only be used for quick testing.
        /// </summary>
        public Game()
        {
            GameWindowSettings gwSettings = new GameWindowSettings()
            {
                RenderFrequency = 60.0,
                UpdateFrequency = 60.0
            };

            NativeWindowSettings nwSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 600)
            };


            Window = new Window(gwSettings, nwSettings);
        }


        /// <summary>
        /// Starts the game and opens the window.
        /// </summary>
        public void Start()
        {
            Window.Run();
        }
    }
}
