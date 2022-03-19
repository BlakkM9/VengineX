using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering.Shaders;
using VengineX.Graphics.Rendering.Textures;
using VengineX.Resources;
using VengineX.UI.Serialization;

namespace VengineX.UI.Elements
{
    /// <summary>
    /// Class representing an image on the ui.<br/>
    /// This is pretty much always used when there is something to render on the ui.
    /// </summary>
    public class Image : UIElement
    {
        /// <summary>
        /// Shader used for ui images.
        /// </summary>
        public static Shader? ImageShader { get; private set; }

        /// <summary>
        /// Location of the projection matrix uniform in <see cref="ImageShader"/>.
        /// </summary>
        public static int ProjectionMatrixLocation { get; private set; }

        /// <summary>
        /// Location of the model matrix uniform in <see cref="ImageShader"/>.
        /// </summary>
        public static int ModelMatrixLocation { get; private set; }

        /// <summary>
        /// Location of the view matrix uniform in <see cref="ImageShader"/>.
        /// </summary>
        public static int ViewMatrixLocation { get; private set; }

        /// <summary>
        /// Location of the uColor uniform in <see cref="ImageShader"/>.
        /// </summary>
        public static int ColorLocation { get; private set; }

        /// <summary>
        /// Location of the uTint uniform in <see cref="ImageShader"/>.
        /// </summary>
        public static int TintLocation { get; private set; }

        /// <summary>
        /// The color that is used if there is no texture.<br/>
        /// This should be <see cref="Vector4.Zero"/> if there is any texture present.<br/>
        /// If you want to tint the texture use <see cref="Tint"/> instead.
        /// </summary>
        public Vector4 Color { get => _color; set => _color = value; }
        private Vector4 _color = Vector4.Zero;

        /// <summary>
        /// Tint of this image.
        /// </summary>
        public Vector4 Tint { get => _tint; set => _tint = value; }
        private Vector4 _tint = Vector4.One;

        /// <summary>
        /// The texture of this image.
        /// </summary>
        public Texture2D? Texture { get; set; } = null;


        /// <summary>
        /// Constructor for creating instance with <see cref="UISerializer"/>.
        /// </summary>
        public Image(UIElement parent) : base(parent)
        {
            // Lazy shader initialization
            if (ImageShader == null)
            {
                ImageShader = ResourceManager.GetResource<Shader>("shader.ui.image");
                ProjectionMatrixLocation = ImageShader.GetUniformLocation("P");
                ModelMatrixLocation = ImageShader.GetUniformLocation("M");
                ViewMatrixLocation = ImageShader.GetUniformLocation("V");
                ColorLocation = ImageShader.GetUniformLocation("uColor");
                TintLocation = ImageShader.GetUniformLocation("uTint");
            }
        }

        /// <summary>
        /// Creates a new image ui element from given parameters.
        /// </summary>
        public Image(UIElement parent, Texture2D? texture, Vector4 color, Vector4 tint)
            : this(parent)
        {
            Color = color;
            Tint = tint;
            Texture = texture;
        }

        /// <summary>
        /// Overload for <see cref="Image(UIElement, Texture2D, Vector4, Vector4)"/>.
        /// </summary>
        public Image(UIElement parent, Vector4 color)
            : this(parent, null, color, new Vector4(1, 1, 1, 0)) { }

        /// <summary>
        /// Overload for <see cref="Image(UIElement, Texture2D, Vector4, Vector4)"/>.
        /// </summary>
        public Image(UIElement parent, Texture2D texture, Vector4 tint)
            : this(parent, texture, Vector4.Zero, tint) { }

        /// <summary>
        /// Overload for <see cref="Image(UIElement, Texture2D, Vector4, Vector4)"/>.
        /// </summary>
        public Image(UIElement parent, Texture2D texture)
            : this(parent, texture, Vector4.Zero, Vector4.One) { }


        // TODO batch rendering with instanced quads if same texture is used
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void Render()
        {
            if (Visible)
            {
                // Render self
                ImageShader.Bind();
                Texture?.Bind();

                ImageShader.SetUniformMat4(ProjectionMatrixLocation, ref ParentCanvas.ProjectionMatrix);
                ImageShader.SetUniformMat4(ViewMatrixLocation, ref ParentCanvas.ViewMatrix);
                ImageShader.SetUniformMat4(ModelMatrixLocation, ref ModelMatrix);
                ImageShader.SetUniformVec4(ColorLocation, ref _color);
                ImageShader.SetUniformVec4(TintLocation, ref _tint);

                ParentCanvas.Quad.Render();
            }


            base.Render();
        }
    }
}
