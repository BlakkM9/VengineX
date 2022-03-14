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
using VengineX.Graphics.Rendering.Textures;

namespace VengineX.UI.Elements
{

    public class Image : UIElement
    {
        public Vector4 Tint { get => _tint; set => _tint = value; }
        protected Vector4 _tint;

        private Texture2D _texture;


        public Image(float x, float y, float width, float height, Texture2D texture, Vector4 tint) : base(x, y, width, height)
        {
            Tint = tint;
            _texture = texture;
        }


        public override void Render()
        {
            ParentCanvas.UIShader.Bind();

            _texture.Bind();

            ParentCanvas.UIShader.SetUniformMat4(ParentCanvas.mLoc, ref ModelMatrix);
            ParentCanvas.UIShader.SetUniformVec4(ParentCanvas.colorLocation, ref _tint);

            ParentCanvas.Quad.Render();
        }
    }
}
