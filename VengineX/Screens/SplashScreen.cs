using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using VengineX.Core;
using VengineX.Graphics.Rendering.Pipelines;
using VengineX.Graphics.Rendering.Textures;
using VengineX.Input;
using VengineX.Resources;
using VengineX.Tweening;
using VengineX.UI;
using VengineX.UI.Layouts;
using VengineX.Wrappers.Stbi;
using Image = VengineX.UI.Elements.Image;

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

        private Sequence _splashSequence;
        private Texture2D _logo;


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
                FilePath = "res/textures/VengineX.png",
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

            //_logoImage = new Image(0, 0, 512, 512, _logo);
            _pipeline.OverlayUI.Layout = new AlignLayout(HorizontalAlignment.Center, VerticalAlignment.Center);
            Image logoImage = new Image(_pipeline.OverlayUI, _logo);
            logoImage.Size = new Vector2(512, 512);
            _pipeline.OverlayUI.UpdateLayout();


            // Clear color
            _pipeline.ClearColor = Vector4.Zero;


            // Create logo animation
            float startSize = logoImage.Size.X;
            float sizeChange = 40;
            float aChange = 1;
            Tween inAnim = new Tween(1.5f, EasingFunction.EaseOutCubic, (t) =>
                {
                    logoImage.Tint = new Vector4(1, 1, 1, t * aChange);
                    logoImage.Width = startSize + t * sizeChange;
                    logoImage.Height = startSize + t * sizeChange;
                    _pipeline.OverlayUI.UpdateLayout();
                });
            Tween outAnim = new Tween(0.5f, EasingFunction.EaseOutCubic, (t) =>
            {
                logoImage.Tint = new Vector4(1, 1, 1, 1 - (t * aChange));
            });


            _splashSequence = new Sequence(inAnim, Tween.Delay(0.5f), outAnim);
            _splashSequence.Stopped += (_) => Finished?.Invoke();
            _splashSequence.Start();
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Update(double delta) {

            // Skip splash screen with ESC
            KeyboardState kbs = _input.KeyboardState;
            MouseState ms = _input.MouseState;

            if (kbs.IsAnyKeyDown || ms.IsAnyButtonDown)
            {
                _splashSequence.Stop();
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
