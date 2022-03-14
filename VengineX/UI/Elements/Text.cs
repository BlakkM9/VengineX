using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering;
using VengineX.Graphics.Rendering.Shaders;
using VengineX.Graphics.Rendering.Vertices;
using VengineX.Resources;
using VengineX.UI.Fonts;
using VengineX.Wrappers.FreeType;

namespace VengineX.UI.Elements
{
    /// <summary>
    /// A label with text.
    /// </summary>
    public class Text : UIElement
    {
        public static Shader BitmapFontShader { get; private set; } = null;
        public static int ProjectionMatrixLocation { get; private set; }
        public static int ModelMatrixLocation { get; private set; }
        public static int ViewMatrixLocation { get; private set; }
        public static int ColorLocation { get; private set; }

        public Vector4 Color { get => _color; set => _color = value; }
        protected Vector4 _color;

        private Mesh<UIVertex> _textMesh;
        private BitmapFont _font;


        public Text(BitmapFont font, string text, float x, float y, float size, Vector4 color)
            : base(x, y, font.CalculateWidth(text), size)
        {
            // Lazy shader initialization
            if (BitmapFontShader == null)
            {
                BitmapFontShader = ResourceManager.GetResource<Shader>("shader.bmpfont");
                ProjectionMatrixLocation = BitmapFontShader.GetUniformLocation("P");
                ModelMatrixLocation = BitmapFontShader.GetUniformLocation("M");
                ViewMatrixLocation = BitmapFontShader.GetUniformLocation("V");
            }

            _font = font;
            _textMesh = font.CreateMesh(text);
            _color = color;

            CalculateModelMatrix();
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void Render()
        {
            BitmapFontShader.Bind();
            _font.TextureAtlas.Bind();

            BitmapFontShader.SetUniformMat4(ProjectionMatrixLocation, ref ParentCanvas.ProjectionMatrix);
            BitmapFontShader.SetUniformMat4(ViewMatrixLocation, ref ParentCanvas.ViewMatrix);
            BitmapFontShader.SetUniformMat4(ModelMatrixLocation, ref ModelMatrix);
            BitmapFontShader.SetUniformVec4(ColorLocation, ref _color);

            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            _textMesh.Render();
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void CalculateModelMatrix()
        {
            if (_font != null)
            {
                // Update model matrix
                ModelMatrix = Matrix4.Identity;
                ModelMatrix *= Matrix4.CreateScale(Height / _font.Size, Height / _font.Size, 0);
                ModelMatrix *= Matrix4.CreateTranslation(X, -(Y + Height), 0);
            }
        }
    }
}
