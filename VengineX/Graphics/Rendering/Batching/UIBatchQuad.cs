using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering.Textures;

namespace VengineX.Graphics.Rendering.Batching
{
    public struct UIBatchQuad
    {
        public Vector2 size = Vector2.One;
        public Vector2 positon = Vector2.Zero;
        public Vector2 uv0 = new Vector2(0, 1);
        public Vector2 uv1 = new Vector2(0, 0);
        public Vector2 uv2 = new Vector2(1, 1);
        public Vector2 uv3 = new Vector2(1, 0);
        public Vector4 color = Vector4.One;
        public Texture2D? texture = null;

        public UIBatchQuad() { }
    }
}
