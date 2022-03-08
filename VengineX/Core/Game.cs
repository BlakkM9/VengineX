using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Config;
using VengineX.Debugging.Logging;
using VengineX.Resources;

namespace VengineX.Core
{
    /// <summary>
    /// Abstract class to derive your game from.
    /// </summary>
    public abstract class Game<T> : IScreen where T : GameSettings, new()
    {
        /// <summary>
        /// The OpenGL window of the game.
        /// </summary>
        public static Window Window { get; private set; }

        /// <summary>
        /// The settings for the game (without keymap).
        /// </summary>
        public static T? Settings { get; private set; }


        /// <summary>
        /// Default constructor, intialising game's window with settings file and default logger configuration.<br/>
        /// If no settings are found, creating default settings file.
        /// </summary>
        public Game() :this(LoggerConfiguration.DEFAULT) { }


        /// <summary>
        /// Intialising game's window with settings file and given logger configuration.<br/>
        /// If no settings are found, creating default settings file.
        /// </summary>
        public Game(LoggerConfiguration loggerConfiguration)
        {   
            // Game settings
            Settings = new T();
            Settings.LoadOrDefault();

            GameWindowSettings gwSettings = new GameWindowSettings()
            {
                RenderFrequency = Settings.RenderFrequency,
                UpdateFrequency = Settings.UpdateFrequency,
            };

            NativeWindowSettings nwSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(Settings.ResolutionX, Settings.ResolutionY),
                CurrentMonitor = Monitors.GetMonitors()[Settings.TargetMonitor].Handle,
            };

            // WindowMode
            switch (Settings.WindowMode)
            {
                case WindowMode.Fullscreen:
                    nwSettings.WindowState = WindowState.Fullscreen;
                    nwSettings.WindowBorder = WindowBorder.Fixed;
                    break;
                case WindowMode.Borderless:
                    nwSettings.WindowState = WindowState.Normal;
                    nwSettings.WindowBorder = WindowBorder.Hidden;
                    break;
                case WindowMode.Windowed:
                    nwSettings.WindowState = WindowState.Normal;
                    nwSettings.WindowBorder = WindowBorder.Fixed;
                    break;
            }


            // Logger config
            Logger.Configuration = loggerConfiguration;


            Window = new Window(gwSettings, nwSettings);
            RegisterWindowHooks();
        }


        /// <summary>
        /// Opens the window and starts the game logic.
        /// </summary>
        public void Start()
        {
            Window.Run();
        }


        /// <summary>
        /// Registers all hooks to the game's window events.
        /// </summary>
        private void RegisterWindowHooks()
        {
            Window.Load += Window_Load;
            Window.UpdateFrame += Window_UpdateFrame;
            Window.RenderFrame += Window_RenderFrame;
            Window.Resize += Window_Resize;
            Window.Unload += Window_Unload;
        }


        /// <summary>
        /// EventHandler for <see cref="GameWindow.Load"/>.
        /// </summary>
        private void Window_Load()
        {
            // Initialize systems.
            Logger.Initialize();

            // Set default GL clears.
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.ClearDepth(1.0f);

            Load();
        }


        /// <summary>
        /// EventHandler for <see cref="GameWindow.UpdateFrame"/>.
        /// </summary>
        private void Window_UpdateFrame(FrameEventArgs args) => Update(args.Time);


        /// <summary>
        /// EventHandler for <see cref="GameWindow.RenderFrame"/>.
        /// </summary>
        private void Window_RenderFrame(FrameEventArgs args) => Render(args.Time);


        /// <summary>
        /// EventHandler for <see cref="NativeWindow.Resize"/>.
        /// </summary>
        private void Window_Resize(ResizeEventArgs args) => Resize(args.Width, args.Height);


        /// <summary>
        /// EventHandler for <see cref="Game.Window.Unload"/>.
        /// </summary>
        private void Window_Unload()
        {
            Unload();

            // Unload all resources
            ResourceManager.UnloadAllResources();

            // Close log file stream.
            Logger.CloseCurrenLogFileStream();

            Window.Dispose();
        }


        public abstract void Load();
        public abstract void Update(double delta);
        public abstract void Render(double delta);
        public abstract void Resize(int width, int height);
        public abstract void Unload();
    }
}
