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

namespace VengineX.UI.Elements
{
    public class Image : UIElement
    {
        public static Shader ImageShader { get; private set; }
        public static int ProjectionMatrixLocation { get; private set; }
        public static int ModelMatrixLocation { get; private set; }
        public static int ViewMatrixLocation { get; private set; }
        public static int ColorLocation { get; private set; }
        public static int TintLocation { get; private set; }

        public Vector4 Color { get => _color; set => _color = value; }
        protected Vector4 _color;

        public Vector4 Tint { get => _tint; set => _tint = value; }
        protected Vector4 _tint;

        private Texture2D? _texture;

        public Image(UIElement parent, Texture2D texture, Vector4 color, Vector4 tint)
            : base(parent)
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

            _color = color;
            _tint = tint;
            _texture = texture;
        }

        public Image(UIElement parent, Vector4 color)
            : this(parent, null, color, new Vector4(1, 1, 1, 0)) { }


        public Image(UIElement parent, Texture2D texture, Vector4 tint)
            : this(parent, texture, Vector4.Zero, tint) { }


        public Image(UIElement parent, Texture2D texture)
            : this(parent, texture, Vector4.Zero, Vector4.One) { }


        // TODO batch rendering with instanced quads if same texture is used
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void Render()
        {
            // Render self
            ImageShader.Bind();

            if (_texture == null)
            {
                GL.BindTextureUnit(0, 0);
            }
            else
            {
                _texture.Bind();
            }

            ImageShader.SetUniformMat4(ProjectionMatrixLocation, ref ParentCanvas.ProjectionMatrix);
            ImageShader.SetUniformMat4(ViewMatrixLocation, ref ParentCanvas.ViewMatrix);
            ImageShader.SetUniformMat4(ModelMatrixLocation, ref ModelMatrix);
            ImageShader.SetUniformVec4(ColorLocation, ref _color);
            ImageShader.SetUniformVec4(TintLocation, ref _tint);

            ParentCanvas.Quad.Render();

            base.Render();
        }
    }
}
