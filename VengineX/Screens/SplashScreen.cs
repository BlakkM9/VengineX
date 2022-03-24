using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using VengineX.ECS;
using VengineX.Graphics.Rendering.Pipelines;
using VengineX.Graphics.Rendering.Textures;
using VengineX.Input;
using VengineX.Resources;
using VengineX.Tweening;
using VengineX.UI.Layouts;
using VengineX.Wrappers.Stbi;
using Image = VengineX.UI.Elements.Basic.Image;

namespace VengineX.Screens
{
    /// <summary>
    /// A simple splash screen, showing the VengineX logo.<br/>
    /// This shows a simple example how a screen could be implemented.<br/>
    /// You don't have to use this when using the engine but we would be happy if you do so!<br/>
    /// Or also if you create your own splash screen, displaying the engine's logo!
    /// </summary>
    public class SplashScreen : IScreen
    {
        private RenderPipelineBase _pipeline;
        private InputManager _input;

        /// <summary>
        /// Occurs when the SplashScreens animation is finished.
        /// </summary>
        public event Action? Finished;

        private Texture2D _logo;
        private SplashScreenUI _splashScreenUI;


        /// <summary>
        /// Creates the splash screen and uses given render pipline for it.
        /// </summary>
        public SplashScreen(RenderPipelineBase pipeline, InputManager input)
        {
            _pipeline = pipeline;
            _input = input;
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Load()
        {
            // Logo
            _logo = ResourceManager.LoadResource<Texture2D>("texture2d.logo", new Texture2DLoadingParameters()
            {
                FilePath = "res/texture/VengineX.png",
                LoadingFunction = LoadingFunction.Load,
                TextureParameters = new Texture2DParameters()
                {
                    InternalFormat = (SizedInternalFormat)All.Rgba8,
                    PixelFormat = PixelFormat.Rgba,
                    PixelType = PixelType.UnsignedByte,
                    MinFilter = TextureMinFilter.Linear,
                    MagFilter = TextureMagFilter.Linear,
                    WrapModeS = TextureWrapMode.Repeat,
                    WrapModeT = TextureWrapMode.Repeat,
                    GenerateMipmaps = false
                },
            });

            _splashScreenUI = new SplashScreenUI(_pipeline.OverlayUI);
            _splashScreenUI.Finished += () => Finished?.Invoke();

            // Clear color
            _pipeline.ClearColor = Vector4.Zero;
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Update(double delta)
        {

            // Skip splash screen with ESC
            KeyboardState kbs = _input.KeyboardState;
            MouseState ms = _input.MouseState;

            if (kbs.IsAnyKeyDown || ms.IsAnyButtonDown)
            {
                _splashScreenUI.Skip();
            }
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Render(double delta)
        {
            _pipeline.Render();
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Resize(int width, int height) { }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Unload()
        {
            // Unload the logo
            ResourceManager.UnloadResource(_logo);

            // Clear the OverlayUI in the rendering pipeline
            _pipeline.OverlayUI.RemoveChild(0);
        }
    }
}
