using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Core;
using VengineX.Graphics.Rendering.Shaders;
using VengineX.Graphics.Rendering.Textures;
using VengineX.Resources;

namespace VengineX.UI.Elements
{

    public class Image : EventDrivenUIElement
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


        public Image(float x, float y, float width, float height, Texture2D texture, Vector4 color, Vector4 tint) : base(x, y, width, height)
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


        public Image(float x, float y, float width, float height, Vector4 color)
            : this(x, y, width, height, null, color, Vector4.One) { }


        public Image(float x, float y, float width, float height, Texture2D texture, Vector4 tint)
            : this(x, y, width, height, texture, Vector4.Zero, tint) { }


        public Image(float x, float y, float width, float height, Texture2D texture)
            : this(x, y, width, height, texture, Vector4.Zero, Vector4.One) { }


        // TODO batch rendering with instanced quads if same texture is used
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void Render()
        {
            if (ParentCanvas != null)
            {
                // Render self
                ImageShader.Bind();

                if (_texture == null)
                {
                    GL.BindTexture(TextureTarget.Texture2D, 0);
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


                // Render children
                foreach (EventDrivenUIElement child in Children)
                {
                    child.Render();
                }
            }
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void CalculateModelMatrix()
        {
            // Update model matrix
            ModelMatrix = Matrix4.Identity;
            ModelMatrix *= Matrix4.CreateScale(Width / 2f, Height / 2f, 0);
            ModelMatrix *= Matrix4.CreateTranslation(Width / 2f + X, -(Height / 2f + Y), 0);
        }
    }
}
