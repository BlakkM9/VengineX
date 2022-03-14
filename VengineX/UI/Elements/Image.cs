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

    public class Image : UIElement
    {
        public static Shader ImageShader { get; private set; }
        public static int ProjectionMatrixLocation { get; private set; }
        public static int ModelMatrixLocation { get; private set; }
        public static int ViewMatrixLocation { get; private set; }
        public static int ColorLocation { get; private set; }

        public Vector4 Color { get => _color; set => _color = value; }
        protected Vector4 _color;

        private Texture2D _texture;


        public Image(float x, float y, float width, float height, Texture2D texture, Vector4 color) : base(x, y, width, height)
        {
            // Lazy shader initialization
            if (ImageShader == null)
            {
                ImageShader = ResourceManager.GetResource<Shader>("shader.image");
                ProjectionMatrixLocation = ImageShader.GetUniformLocation("P");
                ModelMatrixLocation = ImageShader.GetUniformLocation("M");
                ViewMatrixLocation = ImageShader.GetUniformLocation("V");
                ColorLocation = ImageShader.GetUniformLocation("uColor");
            }

            _color = color;
            _texture = texture;
        }


        public override void Render()
        {
            ImageShader.Bind();

            _texture.Bind();

            ImageShader.SetUniformMat4(ProjectionMatrixLocation, ref ParentCanvas.ProjectionMatrix);
            ImageShader.SetUniformMat4(ViewMatrixLocation, ref ParentCanvas.ViewMatrix);
            ImageShader.SetUniformMat4(ModelMatrixLocation, ref ModelMatrix);
            ImageShader.SetUniformVec4(ColorLocation, ref _color);

            ParentCanvas.Quad.Render();
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
