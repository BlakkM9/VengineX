using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Core;
using VengineX.Graphics.Rendering.Pipelines;
using VengineX.Graphics.Rendering.Textures;
using VengineX.Resources;
using VengineX.UI;
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

        /// <summary>
        /// Occurs when the SplashScreens animation is finished.
        /// </summary>
        public event Action? Finished;

        private bool _splashScreenFinished = false;
        private Texture2D? _logo;
        private Image? _logoImage;


        /// <summary>
        /// Creates the splash screen and uses given render pipline for it.
        /// </summary>
        /// <param name="pipeline"></param>
        public SplashScreen(RenderPipelineBase pipeline)
        {
            _pipeline = pipeline;
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
                    PixelInternalFormat = PixelInternalFormat.Rgba8,
                    PixelFormat = PixelFormat.Rgba,
                    PixelType = PixelType.UnsignedByte,
                    MinFilter = TextureMinFilter.Linear,
                    MagFilter = TextureMagFilter.Linear,
                    WrapModeS = TextureWrapMode.Repeat,
                    WrapModeT = TextureWrapMode.Repeat,
                    GenerateMipmaps = false
                },
            });

            _logoImage = new Image(0, 0, 512, 512, _logo, Vector4.One);
            _pipeline.OverlayUI.AddChild(
                _logoImage,
                HorizontalOrientation.Center,
                VerticalOrientation.Center);

            _logoImage.Width = _logoImage.Height;
            startSize = _logoImage.Width;
            _logoImage.VerticalOrientation = VerticalOrientation.Center;
            _logoImage.UpdateLayout();

            // Clear color
            _pipeline.ClearColor = Vector4.Zero;
        }


        float animTime = 5.0f;
        float currentTime = 0;
        float startSize;
        float sizeChange = 15;
        float aChange = 1;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Update(double delta)
        {
            if (currentTime < animTime)
            {
                currentTime += (float)Time.DeltaUpdate;

                float t = currentTime / animTime;

                _logoImage.Color = new Vector4(1, 1, 1, t * aChange);
                _logoImage.Width = startSize + t * sizeChange;
                _logoImage.Height = startSize + t * sizeChange;
                _logoImage.UpdateLayout();
            }
            else if (!_splashScreenFinished)
            {
                _splashScreenFinished = true;
                Finished?.Invoke();
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
        public void Resize(int width, int height)
        {
            //throw new NotImplementedException();
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Unload()
        {
            // Unload the logo
            ResourceManager.UnloadResource(_logo);

            // Clear the OverlayUI in the rendering pipeline
            _pipeline.OverlayUI.ClearChildren(true);
        }
    }
}
