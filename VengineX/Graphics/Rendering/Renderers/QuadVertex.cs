using OpenTK.Mathematics;
using VengineX.Graphics.Rendering.Textures;

namespace VengineX.Graphics.Rendering.Renderers
{
    public struct QuadVertex
    {
        public Vector2 size = Vector2.One;
        public Vector2 position = Vector2.Zero;
        public Vector2 uv0 = new Vector2(0, 1);
        public Vector2 uv1 = new Vector2(0, 0);
        public Vector2 uv2 = new Vector2(1, 1);
        public Vector2 uv3 = new Vector2(1, 0);
        public Vector4 color = Vector4.One;
        public Texture2D? texture = null;

        public QuadVertex() { }

        public override string ToString()
        {
            return $"size: {size}, pos: {position}, uvs: {uv0}, {uv1}, {uv2}, {uv3}, color: {color}, texture: {texture?.ResourcePath}";
        }
    }
}
