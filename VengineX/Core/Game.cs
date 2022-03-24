using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using VengineX.Config;
using VengineX.Debugging.Logging;
using VengineX.ECS;
using VengineX.Graphics.Rendering.Shaders;
using VengineX.Input;
using VengineX.Resources;
using VengineX.Tweening;
using VengineX.UI.Fonts;
using VengineX.Utils;
using VengineX.Wrappers.FreeType;
using CharacterRange = VengineX.Wrappers.FreeType.CharacterRange;

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
        public static T Settings { get; private set; }

        /// <summary>
        /// The <see cref="InputManager"/> for this game.
        /// </summary>
        public static InputManager Input { get; private set; }

        /// <summary>
        /// The <see cref="ScreenManager"/> for this game.
        /// </summary>
        public static ScreenManager ScreenManager { get; private set; }

        /// <summary>
        /// Registry that handles all the entities and components in the game.
        /// </summary>
        public static Registry Registry { get; private set; }

        /// <summary>
        /// Default constructor, intialising game's window with settings file and default logger configuration.<br/>
        /// If no settings are found, creating default settings file.
        /// </summary>
        public Game() : this(LoggerConfiguration.DEFAULT) { }


        /// <summary>
        /// Intialising game's window with settings file and given logger configuration.<br/>
        /// If no settings are found, creating default settings file.
        /// </summary>
        public Game(LoggerConfiguration loggerConfiguration)
        {
            // Game settings.
            Settings = new T();
            Settings.LoadOrDefault();

            // Show console when in debugging mode and overwrite close handler.
            if (Settings.DebugMode)
            {
                ConsoleUtils.ShowConsoleWindow();
            }


            // Create settings for game and native window.
            GameWindowSettings gwSettings = new GameWindowSettings()
            {
                RenderFrequency = Settings.RenderFrequency,
                UpdateFrequency = Settings.UpdateFrequency,
            };

            NativeWindowSettings nwSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(Settings.ResolutionX, Settings.ResolutionY),
                CurrentMonitor = Monitors.GetMonitors()[Settings.TargetMonitor].Handle,
                Location = new Vector2i(Settings.LocationX, Settings.LocationY),
            };

            // Configure WindowMode.
            switch (Settings.WindowMode)
            {
                case WindowMode.Fullscreen:
                    nwSettings.WindowState = WindowState.Fullscreen;
                    nwSettings.WindowBorder = WindowBorder.Hidden;
                    break;
                case WindowMode.Borderless:
                    nwSettings.WindowState = WindowState.Maximized;
                    nwSettings.WindowBorder = WindowBorder.Hidden;
                    break;
                case WindowMode.Windowed:
                    nwSettings.WindowState = WindowState.Normal;
                    nwSettings.WindowBorder = WindowBorder.Fixed;
                    break;
            }


            // Set logger configuration.
            Logger.Configuration = loggerConfiguration;

            // Log window initialization settings
            Logger.Log(Severity.Info, Tag.Initialization, "Initializing Window...");
            Logger.Log(Severity.Info, Tag.Initialization, $"RenderFrequency: {gwSettings.RenderFrequency}");
            Logger.Log(Severity.Info, Tag.Initialization, $"UpdateFrequency: {gwSettings.UpdateFrequency}");
            Logger.Log(Severity.Info, Tag.Initialization, $"Size: {nwSettings.Size}");
            Logger.Log(Severity.Info, Tag.Initialization, $"Monitor: {Monitors.GetMonitors()[Settings.TargetMonitor].HorizontalResolution}x{Monitors.GetMonitors()[Settings.TargetMonitor].VerticalResolution}");
            Logger.Log(Severity.Info, Tag.Initialization, $"WindowState: {nwSettings.WindowState}");
            Logger.Log(Severity.Info, Tag.Initialization, $"WindowBorder: {nwSettings.WindowBorder}");

            // Create window and add event hooks.
            Window = new Window(gwSettings, nwSettings);

            Logger.Log(Severity.Info, Tag.Initialization, "Done:");
            Logger.Log(Severity.Info, Tag.Initialization, $"API: {Window.API}");
            Logger.Log(Severity.Info, Tag.Initialization, $"API Version: {Window.APIVersion}");

            RegisterWindowHooks();

            // Create screen manager
            ScreenManager = new ScreenManager();

            // Create registry
            Registry = new Registry();

            // Create input manager
            Input = new InputManager(Window);
        }


        /// <summary>
        /// Opens the window and starts the game logic.
        /// </summary>
        public void Start() => Window.Run();


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
            FreeTypeWrapper.InitFreeType();

            // Load base resources.
            LoadBaseResources();

            // Set default GL clears.
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.ClearDepth(1.0f);

            Load();
        }


        /// <summary>
        /// EventHandler for <see cref="GameWindow.UpdateFrame"/>.
        /// </summary>
        private void Window_UpdateFrame(FrameEventArgs args)
        {
            Time.Update(args.Time);
            Input.Update();
            TweenManager.Update();
            Update(args.Time);
        }


        /// <summary>
        /// EventHandler for <see cref="GameWindow.RenderFrame"/>.
        /// </summary>
        private void Window_RenderFrame(FrameEventArgs args)
        {
            Time.Render(args.Time);
            Render(args.Time);
        }


        /// <summary>
        /// EventHandler for <see cref="NativeWindow.Resize"/>.
        /// </summary>
        private void Window_Resize(ResizeEventArgs args) => Resize(args.Width, args.Height);


        /// <summary>
        /// EventHandler for <see cref="GameWindow.Unload"/>.
        /// </summary>
        private void Window_Unload()
        {
            Unload();

            Window.Dispose();

            // Unload systems.
            FreeTypeWrapper.DoneFreeType();

            // Unload all resources.
            ResourceManager.UnloadAllResources();

            // Close log file stream.
            Logger.CloseCurrenLogFileStream();
        }


        /// <summary>
        /// Loads all the base resources that should be loaded before startup.<br/>
        /// This also includes shaders for the ui/loading bitmap fonts etc.<br/>
        /// Override this if you don't need all these resources but be sure that they<br/>
        /// are not used internally by the engine, proceed with caution.<br/><br/>
        /// All these resources are not explicitly disposed but disposed via <see cref="ResourceManager.UnloadAllResources"/><br/>
        /// when the engine is shutting down.
        /// </summary>
        protected virtual void LoadBaseResources()
        {




            //// White texture
            //byte[] white = new byte[]
            //{
            //    0xff, 0xff, 0xff, 0xff
            //};
            //GCHandle pinned = GCHandle.Alloc(white, GCHandleType.Pinned);

            //Texture2DParameters texParams = new Texture2DParameters()
            //{
            //    Height = 1,
            //    Width = 1,
            //    PixelFormat = PixelFormat.Rgba,
            //    InternalFormat = SizedInternalFormat.Rgba8,
            //    PixelType = PixelType.UnsignedByte,
            //    WrapModeS = TextureWrapMode.Repeat,
            //    WrapModeT = TextureWrapMode.Repeat,
            //    GenerateMipmaps = false,
            //    PixelData = pinned.AddrOfPinnedObject(),

            //};
            //Texture2D whiteTexture = new Texture2D(ref texParams);
            //ResourceManager.CacheResource("texture2d.white", whiteTexture);
            //Console.WriteLine(whiteTexture.Handle);
            //pinned.Free();


            // Shaders
            // Bitmap fonts shader
            ResourceManager.LoadResource<Shader>("shader.ui.bmpfont", new ShaderLoadingParameters()
            {
                VertexPath = "res/shader/ui/ui.vs.glsl",
                FragmentPath = "res/shader/ui/bmpfont.fs.glsl"
            });


            // Shader for batch ui rendering
            ResourceManager.LoadResource<Shader>("shader.ui.batch", new ShaderLoadingParameters()
            {
                VertexPath = "res/shader/ui/ui.vs.glsl",
                FragmentPath = "res/shader/ui/batch.fs.glsl"
            });


            // Fonts
            // OpenSans
            ResourceManager.LoadResource<BitmapFont>("font.opensans", new BitmapFontLoadingParameters()
            {
                FontPath = "res/font/OpenSans.ttf",
                Size = 50,
                Ranges = new CharacterRange[]
                {
                    CharacterRange.BasicLatin,
                    CharacterRange.Latin1Supplement,
                    CharacterRange.LatinExtendedA,
                    CharacterRange.LatinExtendedB,

                    CharacterRange.Cyrillic,
                    CharacterRange.CyrillicSupplementary,
                }
            });
        }


        /// <summary>
        /// <see cref="GameWindow.Close()"/> shortcut.
        /// </summary>
        public static void Exit() => Window.Close();


        /// <summary>
        /// Called before the game's window is displayed for the first time.
        /// </summary>
        public abstract void Load();


        /// <summary>
        /// Called when it is time to update a frame.
        /// </summary>
        public abstract void Update(double delta);


        /// <summary>
        /// Called when it is time to render a frame.
        /// </summary>
        public abstract void Render(double delta);


        /// <summary>
        /// Called when the game's window is resized.
        /// </summary>
        public abstract void Resize(int width, int height);


        /// <summary>
        /// Called before the game's window is destroyed.
        /// </summary>
        public abstract void Unload();
    }
}
