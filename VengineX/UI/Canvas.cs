using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering;
using VengineX.Graphics.Rendering.Batching;
using VengineX.Graphics.Rendering.Shaders;
using VengineX.Graphics.Rendering.UnitModels;
using VengineX.Input;
using VengineX.Resources;
using VengineX.UI.Elements;
using VengineX.UI.Fonts;
using VengineX.Wrappers.FreeType;

namespace VengineX.UI
{
    /// <summary>
    /// This class is the top element of any ui.<br/>
    /// All ui elements and events are handled from this class downwards.
    /// </summary>
    public class Canvas : Element, IRenderable
    {
        #region UI Shaders

        /// <summary>
        /// Shader used for ui images.
        /// </summary>
        public static Shader? ImageShader { get; private set; }

        /// <summary>
        /// Location of the projection matrix uniform in <see cref="ImageShader"/>.
        /// </summary>
        public static Uniform ImageProjectionMatrixUniform { get; private set; }

        /// <summary>
        /// Location of the model matrix uniform in <see cref="ImageShader"/>.
        /// </summary>
        public static Uniform ImageModelMatrixUniform { get; private set; }

        /// <summary>
        /// Location of the view matrix uniform in <see cref="ImageShader"/>.
        /// </summary>
        public static Uniform ImageViewMatrixUniform { get; private set; }

        /// <summary>
        /// Location of the uTint uniform in <see cref="ImageShader"/>.
        /// </summary>
        public static Uniform TintLocationUniform { get; private set; }


        public static Shader? ColorShader { get; private set; }

        /// <summary>
        /// Location of the projection matrix uniform in <see cref="ImageShader"/>.
        /// </summary>
        public static Uniform ColorProjectionMatrixUniform { get; private set; }

        /// <summary>
        /// Location of the model matrix uniform in <see cref="ImageShader"/>.
        /// </summary>
        public static Uniform ColorModelMatrixUniform { get; private set; }

        /// <summary>
        /// Location of the view matrix uniform in <see cref="ImageShader"/>.
        /// </summary>
        public static Uniform ColorViewMatrixUniform { get; private set; }

        /// <summary>
        /// Location of the uTint uniform in <see cref="ImageShader"/>.
        /// </summary>
        public static Uniform ColorUniform { get; private set; }

        /// <summary>
        /// The shader that is used to render text.
        /// </summary>
        public static Shader BitmapFontShader { get; private set; }

        /// <summary>
        /// Project matrix uniform location of the font shader.
        /// </summary>
        public static Uniform FontProjectionMatrixUniform { get; private set; }

        /// <summary>
        /// Model matrix uniform location of the font shader.
        /// </summary>
        public static Uniform FontModelMatrixUniform { get; private set; }

        /// <summary>
        /// View matrix uniform location of the font shader.
        /// </summary>
        public static Uniform FontViewMatrixUniform { get; private set; }

        /// <summary>
        /// uColor uniform location of the font shader.
        /// </summary>
        public static Uniform FontColorUniform { get; private set; }

        #endregion

        private UIBatchRenderer _batchRenderer;

        /// <summary>
        /// The projection matrix of this canvas
        /// </summary>
        public ref Matrix4 ProjectionMatrix { get => ref _projectionMatrix; }
        private Matrix4 _projectionMatrix;

        /// <summary>
        /// The view matrix of this canvas. I don't think this is actually needed.
        /// </summary>
        public ref Matrix4 ViewMatrix { get => ref _viewMatrix; }
        private Matrix4 _viewMatrix = Matrix4.Identity;

        /// <summary>
        /// The <see cref="UI.EventSystem"/> that handles (and indirectly invokes) all the<br/>
        /// input events for ui elements within this canvas.
        /// </summary>
        public EventSystem EventSystem { get; }

        /// <summary>
        /// The quad for rendering all the UI elements (that can be rendered onto a quad).
        /// </summary>
        public Quad Quad { get; }


        /// <summary>
        /// Creates a new canvas to render UI elements on.
        /// </summary>
        public Canvas(float width, float height, InputManager input) : base(null)
        {
            // Lazy shader initialization
            if (ImageShader == null)
            {
                ImageShader = ResourceManager.GetResource<Shader>("shader.ui.image");
                ColorShader = ResourceManager.GetResource<Shader>("shader.ui.color");
                BitmapFontShader = ResourceManager.GetResource<Shader>("shader.ui.bmpfont");

                ImageProjectionMatrixUniform = ImageShader.GetUniform("P");
                ImageModelMatrixUniform = ImageShader.GetUniform("M");
                ImageViewMatrixUniform = ImageShader.GetUniform("V");
                ColorProjectionMatrixUniform = ColorShader.GetUniform("P");
                ColorModelMatrixUniform = ColorShader.GetUniform("M");
                ColorViewMatrixUniform = ColorShader.GetUniform("V");
                FontProjectionMatrixUniform = BitmapFontShader.GetUniform("P");
                FontModelMatrixUniform = BitmapFontShader.GetUniform("M");
                FontViewMatrixUniform = BitmapFontShader.GetUniform("V");

                TintLocationUniform = ImageShader.GetUniform("uTint");
                ColorUniform = ColorShader.GetUniform("uColor");
                FontColorUniform = BitmapFontShader.GetUniform("uColor");
            }

            _batchRenderer = new UIBatchRenderer(1000);
            EventSystem = new EventSystem(input, this);
            Size = new Vector2(width, height);
            Quad = new Quad();
            _projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, Width, -Height, 0, -1, 1);

            font = ResourceManager.GetResource<BitmapFont>("font.opensans");
        }


        /// <summary>
        /// Call every frame to update mouse move events<br/>
        /// for the elements in this canvas.
        /// </summary>
        public void UpdateEvents()
        {
            EventSystem.UpdateMouseMove();
        }


        BitmapFont font;
        public override void Render()
        {


            base.Render();
        }


        /// <summary>
        /// Resizes the canvas.
        /// </summary>
        public void Resize(float newWidth, float newHeight)
        {
            Size = new Vector2(newWidth, newHeight);
            _projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, Width, 0, Height, -1, 1);
        }
    }
}
